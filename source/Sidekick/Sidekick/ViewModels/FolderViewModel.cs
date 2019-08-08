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
    public class FolderViewModel : ReactiveViewModelBase<IObservable<ArchiveState>>
    {
        private string title;
        private IFolder currentFolder;
        private IArchive currentArchive;
        private NodeListItemViewModel focusedNode;

        public FolderViewModel(IObservable<ArchiveState> model, IExecutionEnvironment executionEnvironment) 
            : base(model, executionEnvironment)
        {
            Nodes = new ObservableCollection<NodeListItemViewModel>();

            var focusedNodeChanges = this
                .WhenAnyValue(x => x.FocusedNode);

            var isAtRoot = this
                .WhenAnyValue(x => x.CurrentArchive, x => x.CurrentFolder)
                .Select(x => x.Item1?.Content == x.Item2);

            var isNotAtRoot = isAtRoot
                .Select(x => !x);

            var hasCurrentFolder = this
                .WhenAnyValue(x => x.CurrentFolder)
                .Select(x => x != null);

            var hasFocusedNode = focusedNodeChanges
                .Select(x => x != null);

            var focusedNodeIsFolder = focusedNodeChanges
                .Select(x => x?.Model is Folder);

            AddNewFolderCommand = ReactiveCommand.Create(() => AddNewFolder());
            AddNewShortcutCommand = ReactiveCommand.Create(() => AddNewShortcut());
            ChangeTitleCommand = ReactiveCommand.Create(() => ChangeTitle(), hasCurrentFolder);
            DeleteNodeCommand = ReactiveCommand.Create(() => DeleteNode(), hasFocusedNode);
            EnterFolderCommand = ReactiveCommand.Create(() => EnterFolder(), focusedNodeIsFolder);
            ExitFolderCommand = ReactiveCommand.Create(() => ExitFolder(), isNotAtRoot);

            Model.Subscribe(state =>
            {
                CurrentArchive = state.Document;

                CurrentFolder = state.Document.Content
                    .FindNode<IFolder>(x => x.Id == state.CurrentFolderId);

                var newNodes = CurrentFolder.Nodes
                    .OfType<IArchiveNode>()
                    .Select(x => new NodeListItemViewModel(x, ExecutionEnvironment))
                    .ToList();

                // TODO: 
                // Invoke on UI thread via injected platform-specific dispatcher
                // in order to keep VM layer independent of Xamarin Forms

                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    Title = CurrentFolder.Text;

                    Nodes.MorphTo(
                        CurrentFolder.Nodes.OfType<IArchiveNode>().ToArray(),
                        (x, y) => x.Id == y.Id,
                        (x, y) => x.IsSameNode(y),
                        x => new NodeListItemViewModel(x, ExecutionEnvironment));

                    FocusedNode = Nodes.FirstOrDefault(x => x.Id == state.FocusedNodeId);
                });
            });
        }

        public ReactiveCommand<Unit, Unit> AddNewFolderCommand { get; }
        public ReactiveCommand<Unit, Unit> AddNewShortcutCommand { get; }
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

        public IArchive CurrentArchive
        {
            get => currentArchive;
            set => this.RaiseAndSetIfChanged(ref currentArchive, value);
        }

        public NodeListItemViewModel FocusedNode
        {
            get => focusedNode;
            set => this.RaiseAndSetIfChanged(ref focusedNode, value);
        }

        public ObservableCollection<NodeListItemViewModel> Nodes { get; }

        public void FocusNode(NodeListItemViewModel node)
        {
            if (node?.Id == FocusedNode?.Id) return;

            this.DispatchAction(new FocusNodeAction(node?.Id));
        }

        private void AddNewFolder()
        {
            var number = this.GetAppState().Global.NextFolderNumber;
            var text = $"Folder {number}";

            this.DispatchAction(new AddFolderAction(CurrentFolder.Id, $"F{number}", text, 1, focusedNode?.Id));
        }

        private void AddNewShortcut()
        {
            var number = this.GetAppState().Global.NextElementNumber;
            var element = new Element(number.ToString(), $"Shortcut {number}", 10);

            this.DispatchAction(new AddShortcutAction(CurrentFolder.Id, element, 1, insertAfterNodeId: focusedNode?.Id));
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
            if (focusedNode == null) return;

            this.DispatchAction(new DeleteNodeAction(CurrentFolder.Id, focusedNode.Id));
        }

        private void EnterFolder()
        {
            if (!(focusedNode.Model is IFolder folder)) return;

            this.DispatchAction(new EnterFolderAction(folder.Id));
        }

        private void ExitFolder()
        {
            this.DispatchAction(new ExitFolderAction());
        }
    }
}
