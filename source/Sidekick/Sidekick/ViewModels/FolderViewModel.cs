using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using ReactiveUI;
using Sidekick.Extensions;
using Sidekick.Actions;
using Sidekick.Models;
using Xodium.Collections;
using Xodium.Mvvm;
using Xodium.Mvvm.ReactiveUI;
using Xodium.Productivity.Content.Models;
using System.Reactive.Linq;

namespace Sidekick.ViewModels
{
    public class FolderViewModel : ReactiveViewModelBase<IObservable<ProjectState>>
    {
        private string title;
        private IFolder currentFolder;
        private IProject currentProject;
        private NodeListItemViewModel selectedNode;

        public FolderViewModel(IObservable<ProjectState> model, IExecutionEnvironment executionEnvironment) 
            : base(model, executionEnvironment)
        {
            Nodes = new ObservableCollection<NodeListItemViewModel>();

            var selectedNodeChanges = this
                .WhenAnyValue(x => x.SelectedNode);

            var isAtRoot = this
                .WhenAnyValue(x => x.CurrentProject, x => x.CurrentFolder)
                .Select(x => x.Item1?.Content == x.Item2);

            var isNotAtRoot = isAtRoot
                .Select(x => !x);

            var hasCurrentFolder = this
                .WhenAnyValue(x => x.CurrentFolder)
                .Select(x => x != null);

            var hasSelectedNode = selectedNodeChanges
                .Select(x => x != null);

            var selectedNodeIsFolder = selectedNodeChanges
                .Select(x => x?.Model is Folder);

            AddNewFolderCommand = ReactiveCommand.Create(() => AddNewFolder());
            AddNewLineCommand = ReactiveCommand.Create(() => AddNewLine());
            ChangeTitleCommand = ReactiveCommand.Create(() => ChangeTitle(), hasCurrentFolder);
            DeleteNodeCommand = ReactiveCommand.Create(() => DeleteNode(), hasSelectedNode);
            EnterFolderCommand = ReactiveCommand.Create(() => EnterFolder(), selectedNodeIsFolder);
            ExitFolderCommand = ReactiveCommand.Create(() => ExitFolder(), isNotAtRoot);

            Model.Subscribe(state =>
            {
                CurrentProject = state.Document;

                CurrentFolder = state.Document.Content
                    .FindNode<IFolder>(x => x.Id == state.CurrentFolderId);

                var newNodes = CurrentFolder.Nodes
                    .OfType<IProjectNode>()
                    .Select(x => new NodeListItemViewModel(x, ExecutionEnvironment))
                    .ToList();

                // TODO: 
                // Invoke on UI thread via injected platform-specific dispatcher
                // in order to keep VM layer independent of Xamarin Forms

                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    Title = CurrentFolder.Text;

                    Nodes.MorphTo(
                        CurrentFolder.Nodes.OfType<IProjectNode>().ToArray(),
                        (x, y) => x.Id == y.Id,
                        (x, y) => x.IsSameNode(y),
                        x => new NodeListItemViewModel(x, ExecutionEnvironment));

                    SelectedNode = Nodes.FirstOrDefault(x => x.Id == state.SelectedNodeId);
                });
            });
        }

        public ReactiveCommand<Unit, Unit> AddNewFolderCommand { get; }
        public ReactiveCommand<Unit, Unit> AddNewLineCommand { get; }
        public ReactiveCommand<Unit, Unit> ChangeTitleCommand { get; }
        public ReactiveCommand<Unit, Unit> DeleteNodeCommand { get; }
        public ReactiveCommand<Unit, Unit> EnterFolderCommand { get; }
        public ReactiveCommand<Unit, Unit> ExitFolderCommand { get; }

        public string Title
        {
            get => title;
            set => this.RaiseAndSetIfChanged(ref title, value);
        }

        public IFolder CurrentFolder
        {
            get => currentFolder;
            set => this.RaiseAndSetIfChanged(ref currentFolder, value);
        }

        public IProject CurrentProject
        {
            get => currentProject;
            set => this.RaiseAndSetIfChanged(ref currentProject, value);
        }

        public NodeListItemViewModel SelectedNode
        {
            get => selectedNode;
            set => this.RaiseAndSetIfChanged(ref selectedNode, value);
        }

        public ObservableCollection<NodeListItemViewModel> Nodes { get; }

        public void SelectNode(NodeListItemViewModel node)
        {
            if (node?.Id == SelectedNode?.Id) return;

            this.DispatchAction(new SelectNodeAction(node?.Id));
        }

        private void AddNewFolder()
        {
            var number = this.GetAppState().Global.NextFolderNumber;
            var text = $"Folder {number}";

            this.DispatchAction(new AddFolderAction(CurrentFolder.Id, $"F{number}", text, 1, selectedNode?.Id));
        }

        private void AddNewLine()
        {
            var number = this.GetAppState().Global.NextLineNumber;
            var text = $"Line {number}";
            var value = 10;

            this.DispatchAction(new AddLineAction(CurrentFolder.Id, DateTime.Today, text, 1, value, selectedNode?.Id));
        }

        private void ChangeTitle()
        {
            var words = Title.Split();

            if (!int.TryParse(words.Last(), out var count))
            {
                count = 0;
            }

            var prefix = string.Join(" ", words.Take(words.Length - 1));
            var newTitle = $"{prefix} {++count}";

            this.DispatchAction(new ChangeFolderTitleAction(CurrentFolder.Id, newTitle));
        }

        private void DeleteNode()
        {
            if (selectedNode == null) return;

            this.DispatchAction(new DeleteNodeAction(CurrentFolder.Id, selectedNode.Id));
        }

        private void EnterFolder()
        {
            if (!(selectedNode.Model is IFolder folder)) return;

            this.DispatchAction(new EnterFolderAction(folder.Id));
        }

        private void ExitFolder()
        {
            this.DispatchAction(new ExitFolderAction());
        }
    }
}
