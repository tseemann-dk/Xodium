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

namespace Sidekick.ViewModels
{
    public class FolderViewModel : ReactiveViewModelBase<IObservable<ProjectState>>
    {
        private IFolder folder;
        private string title;
        private NodeListItemViewModel selectedNode;

        public FolderViewModel(IObservable<ProjectState> model, IExecutionEnvironment executionEnvironment) 
            : base(model, executionEnvironment)
        {
            Nodes = new ObservableCollection<NodeListItemViewModel>();

            AddNewLineCommand = ReactiveCommand.Create(() => AddNewLine());
            DeleteNodeCommand = ReactiveCommand.Create(() => DeleteNode());
            ChangeTitleCommand = ReactiveCommand.Create(() => ChangeTitle());

            Model.Subscribe(state =>
            {
                folder = state.Document.Content
                    .FindNode<IFolder>(x => x.Id == state.CurrentFolderId);

                var newNodes = folder.Nodes
                    .OfType<IProjectNode>()
                    .Select(x => new NodeListItemViewModel(x, ExecutionEnvironment))
                    .ToList();

                // TODO: 
                // Invoke on UI thread via injected platform-specific dispatcher
                // in order to keep VM layer independent of Xamarin Forms

                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    Title = folder.Text;

                    Nodes.MorphTo(
                        folder.Nodes.OfType<IProjectNode>().ToArray(),
                        (x, y) => x.Id == y.Id,
                        (x, y) => x.IsSameNode(y),
                        x => new NodeListItemViewModel(x, ExecutionEnvironment));

                    SelectedNode = Nodes.FirstOrDefault(x => x.Id == state.SelectedNodeId);
                });
            });
        }

        public ReactiveCommand<Unit, Unit> AddNewLineCommand { get; }
        public ReactiveCommand<Unit, Unit> DeleteNodeCommand { get; }
        public ReactiveCommand<Unit, Unit> ChangeTitleCommand { get; }

        public string Title
        {
            get => title;
            set => this.RaiseAndSetIfChanged(ref title, value);
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

        private void AddNewLine()
        {
            var number = this.GetAppState().Global.NextLineNumber;
            var text = $"Line {number}";
            var value = 10;

            this.DispatchAction(new AddLineAction(folder.Id, DateTime.Today, text, 1, value, selectedNode?.Id));
        }

        private void DeleteNode()
        {
            if (selectedNode == null) return;

            this.DispatchAction(new DeleteNodeAction(folder.Id, selectedNode.Id));
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

            this.DispatchAction(new ChangeFolderTitleAction(folder.Id, newTitle));
        }
    }
}
