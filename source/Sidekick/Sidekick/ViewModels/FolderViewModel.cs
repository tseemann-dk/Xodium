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

            var currentFolderChanges = this
                .WhenAnyValue(x => x.CurrentFolder);

            var currentFolderOrCurrentArchiveChanges = this
                .WhenAnyValue(x => x.CurrentFolder, x => x.CurrentArchive, 
                (folder, archive) => (folder, archive));

            var currentFolderOrFocusedNodeChanges = this
                .WhenAnyValue(x => x.CurrentFolder, x => x.FocusedNode, 
                (folder, node) => (folder, node));

            var hasFocusedNode = focusedNodeChanges
                .Select(x => x != null);

            var isAtRoot = currentFolderOrCurrentArchiveChanges
                .Select(x => x.folder == x.archive?.Content);

            var isNotAtRoot = isAtRoot
                .Select(x => !x);

            var hasCurrentFolder = currentFolderChanges
                .Select(x => x != null);

            var focusedNodeIsFolder = focusedNodeChanges
                .Select(x => x?.Model is Folder);

            var focusedNodeIsNotFirst = currentFolderOrFocusedNodeChanges
                .Select(x => !(x.node?.IsFirstNodeIn(x.folder) ?? true));

            var focusedNodeIsNotLast = currentFolderOrFocusedNodeChanges
                .Select(x => !(x.node?.IsLastNodeIn(x.folder) ?? true));

            AddNewFolderCommand = ReactiveCommand.Create(() => AddNewFolder());
            AddNewShortcutCommand = ReactiveCommand.Create(() => AddNewShortcut());
            ChangeTitleCommand = ReactiveCommand.Create(() => ChangeTitle(), hasCurrentFolder);
            DeleteNodeCommand = ReactiveCommand.Create(() => DeleteNode(), hasFocusedNode);
            EnterFolderCommand = ReactiveCommand.Create(() => EnterFocusedFolder(), focusedNodeIsFolder);
            ExitFolderCommand = ReactiveCommand.Create(() => ExitFolder(), isNotAtRoot);
            MoveNodeDownCommand = ReactiveCommand.Create(() => MoveFocusedNodeDown(), focusedNodeIsNotLast);
            MoveNodeUpCommand = ReactiveCommand.Create(() => MoveFocusedNodeUp(), focusedNodeIsNotFirst);

            Model.Subscribe(state => ApplyState(state));
        }

        private void ApplyState(ArchiveState state)
        {
            CurrentArchive = state.Document;

            CurrentFolder = state.Document.Content
                .FindNode<IFolder>(x => x.Id == state.CurrentFolderId);

            var newNodes = CurrentFolder.Nodes
                .OfType<IArchiveNode>()
                .Select(CreateNodeItemViewModel)
                .ToList();

            Title = CurrentFolder.Text;

            Nodes.MorphTo(
                CurrentFolder.Nodes.OfType<IArchiveNode>().ToArray(),
                (x, y) => x.Id == y.Id,
                (x, y) => x.IsSameNode(y),
                CreateNodeItemViewModel);

            FocusedNode = Nodes.FirstOrDefault(x => x.Id == state.FocusedNodeId);

            //ExecutionEnvironment.SynchronizerService.BeginInvokeOnMainThread(() =>
            //{
            //});
        }

        public ReactiveCommand<Unit, Unit> AddNewFolderCommand { get; }
        public ReactiveCommand<Unit, Unit> AddNewShortcutCommand { get; }
        public ReactiveCommand<Unit, Unit> ChangeTitleCommand { get; }
        public ReactiveCommand<Unit, Unit> DeleteNodeCommand { get; }
        public ReactiveCommand<Unit, Unit> EnterFolderCommand { get; }
        public ReactiveCommand<Unit, Unit> ExitFolderCommand { get; }
        public ReactiveCommand<Unit, Unit> MoveNodeDownCommand { get; }
        public ReactiveCommand<Unit, Unit> MoveNodeUpCommand { get; }

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

        public void EnterFolder(NodeListItemViewModel node)
        {
            if (!(node.Model is IFolder folder)) return;

            this.DispatchAction(new EnterFolderAction(folder.Id));
        }

        public void FocusNode(NodeListItemViewModel node)
        {
            if (node?.Id == FocusedNode?.Id) return;

            this.DispatchAction(new FocusNodeAction(node?.Id));
        }

        private void AddNewFolder()
        {
            var number = this.GetAppState().Global.NextFolderNumber;
            var text = $"Folder {number}";

            this.DispatchAction(new AddFolderAction(CurrentFolder.Id, $"F{number}", text, 1, FocusedNode?.Id));
        }

        private void AddNewShortcut()
        {
            var number = this.GetAppState().Global.NextElementNumber;
            var element = new Element(number.ToString(), $"Shortcut {number}", 10);

            this.DispatchAction(new AddShortcutAction(CurrentFolder.Id, element, 1, insertAfterNodeId: FocusedNode?.Id));
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

        private NodeListItemViewModel CreateNodeItemViewModel(IArchiveNode node)
        {
            var vm = new NodeListItemViewModel(node, ExecutionEnvironment);
            vm.OpenRequested += (s, e) => EnterFolder(vm);
            return vm;
        }

        private void DeleteNode()
        {
            if (focusedNode == null) return;

            this.DispatchAction(new DeleteNodeAction(CurrentFolder.Id, FocusedNode.Id));
        }

        private void EnterFocusedFolder()
        {
            EnterFolder(focusedNode);
        }

        private void ExitFolder()
        {
            this.DispatchAction(new ExitFolderAction());
        }

        private void MoveFocusedNodeDown()
        {
            this.DispatchAction(new MoveNodeDownAction(CurrentFolder.Id, FocusedNode?.Id));
        }

        private void MoveFocusedNodeUp()
        {
            this.DispatchAction(new MoveNodeUpAction(CurrentFolder.Id, FocusedNode?.Id));
        }
    }
}
