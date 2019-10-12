using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;
using Sidekick.Actions;
using Sidekick.Shopper.Models;
using Sidekick.Shopper.State;
using Sidekick.Extensions;
using Xodium.Collections;
using Xodium.Mvvm;
using Xodium.Mvvm.ReactiveUI;
using Xodium.DataStructures;

namespace Sidekick.Shopper.ViewModels
{
    public class ShoppingFolderViewModel : ReactiveViewModelBase<IObservable<ShoppingSession>>
    {
        private ShoppingSession session;
        private string title;
        private IShoppingFolder currentFolder;
        private IShoppingList currentShoppingList;
        private ShoppingNodeListItemViewModel focusedNode;
        private readonly ObservableAsPropertyHelper<string> focusedNodeText;

        public ShoppingFolderViewModel(IObservable<ShoppingSession> model, IExecutionEnvironment executionEnvironment) 
            : base(model, executionEnvironment)
        {
            Nodes = new ObservableCollection<ShoppingNodeListItemViewModel>();

            var focusedNodeChanges = this
                .WhenAnyValue(x => x.FocusedNode);

            var currentFolderChanges = this
                .WhenAnyValue(x => x.CurrentFolder);

            var currentFolderOrShoppingListChanges = this
                .WhenAnyValue(x => x.CurrentFolder, x => x.CurrentShoppingList, 
                (folder, shoppingList) => (folder, shoppingList));

            var currentFolderOrFocusedNodeChanges = this
                .WhenAnyValue(x => x.CurrentFolder, x => x.FocusedNode, 
                (folder, node) => (folder, node));

            var hasFocusedNode = focusedNodeChanges
                .Select(x => x != null);

            var isAtRoot = currentFolderOrShoppingListChanges
                .Select(x => x.folder == x.shoppingList?.Content);

            var isNotAtRoot = isAtRoot
                .Select(x => !x);

            var hasCurrentFolder = currentFolderChanges
                .Select(x => x != null);

            var focusedNodeIsFolder = focusedNodeChanges
                .Select(x => x?.Model is ShoppingFolder);

            var focusedNodeIsNotFirst = currentFolderOrFocusedNodeChanges
                .Select(x => !(x.node?.IsFirstNodeIn(x.folder) ?? true));

            var focusedNodeIsNotLast = currentFolderOrFocusedNodeChanges
                .Select(x => !(x.node?.IsLastNodeIn(x.folder) ?? true));

            AddNewFolderCommand = ReactiveCommand.Create(() => AddNewFolder());
            ChangeTitleCommand = ReactiveCommand.Create(() => ChangeTitle(), hasCurrentFolder);
            DeleteNodeCommand = ReactiveCommand.Create(() => DeleteNode(), hasFocusedNode);
            EnterFolderCommand = ReactiveCommand.Create(() => EnterFocusedFolder(), focusedNodeIsFolder);
            ExitFolderCommand = ReactiveCommand.Create(() => ExitFolder(), isNotAtRoot);
            MoveNodeDownCommand = ReactiveCommand.Create(() => MoveFocusedNodeDown(), focusedNodeIsNotLast);
            MoveNodeUpCommand = ReactiveCommand.Create(() => MoveFocusedNodeUp(), focusedNodeIsNotFirst);
            PerformLookupCommand = ReactiveCommand.Create(() => PerformLookup(), hasCurrentFolder);

            ComponentLookup = new ComponentLookupViewModel(
                Model
                    .Select(x => x.ComponentLookup)
                    .DistinctUntilChanged(),
                ExecutionEnvironment
            );

            focusedNodeText = this.WhenAnyValue(x => x.FocusedNode)
                .Select(x => x?.Text)
                .ToProperty(this, x => x.FocusedNodeText);

            Model.Subscribe(state => ApplyState(state));
        }

        #region Properties

        public ComponentLookupViewModel ComponentLookup { get; }

        public IShoppingFolder CurrentFolder
        {
            get => currentFolder;
            set => this.RaiseAndSetIfChanged(ref currentFolder, value);
        }

        public IShoppingList CurrentShoppingList
        {
            get => currentShoppingList;
            set => this.RaiseAndSetIfChanged(ref currentShoppingList, value);
        }

        public ShoppingNodeListItemViewModel FocusedNode
        {
            get => focusedNode;
            set => this.RaiseAndSetIfChanged(ref focusedNode, value);
        }

        public string FocusedNodeText => focusedNodeText.Value;

        public ObservableCollection<ShoppingNodeListItemViewModel> Nodes { get; }

        public string Title
        {
            get => title;
            set => this.RaiseAndSetIfChanged(ref title, value);
        }

        #endregion

        #region Commands

        public ReactiveCommand<Unit, Unit> AddNewFolderCommand { get; }
        public ReactiveCommand<Unit, Unit> ChangeTitleCommand { get; }
        public ReactiveCommand<Unit, Unit> DeleteNodeCommand { get; }
        public ReactiveCommand<Unit, Unit> EnterFolderCommand { get; }
        public ReactiveCommand<Unit, Unit> ExitFolderCommand { get; }
        public ReactiveCommand<Unit, Unit> MoveNodeDownCommand { get; }
        public ReactiveCommand<Unit, Unit> MoveNodeUpCommand { get; }
        public ReactiveCommand<Unit, Unit> PerformLookupCommand { get; }

        private void AddNewFolder()
        {
            this.DispatchAction(GlobalActionCreator.GetNextFolderNumber());

            var folderNumber = this.GetAppState().Global.FolderNumber;
            var title = $"Folder {folderNumber}";

            this.DispatchAction(Actions.ShoppingListActionCreator.AddFolder(
                CurrentFolder.Id,
                new ShoppingFolder($"G{folderNumber}", title, 1),
                FocusedNode?.Id)
            );
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

            this.DispatchAction(Actions.ShoppingListActionCreator.ChangeFolderTitle(CurrentFolder.Id, newTitle));
        }

        private void DeleteNode()
        {
            if (focusedNode == null) return;

            this.DispatchAction(Actions.ShoppingListActionCreator.DeleteNode(CurrentFolder.Id, FocusedNode.Id));
        }

        private void EnterFocusedFolder()
        {
            EnterFolder(focusedNode);
        }

        public void EnterFolder(ShoppingNodeListItemViewModel node)
        {
            if (!(node.Model is IShoppingFolder folder)) return;

            this.DispatchAction(Actions.ShoppingSessionActionCreator.EnterFolder(folder.Id));
        }

        private void ExitFolder()
        {
            this.DispatchAction(Actions.ShoppingSessionActionCreator.ExitFolder());
        }

        public void FocusNode(ShoppingNodeListItemViewModel node)
        {
            if (node?.Id == FocusedNode?.Id) return;

            this.DispatchAction(Actions.ShoppingSessionActionCreator.FocusNode(node?.Id));
        }

        private void MoveFocusedNodeDown()
        {
            this.DispatchAction(Actions.ShoppingListActionCreator.MoveNodeDown(CurrentFolder.Id, FocusedNode?.Id));
        }

        private void MoveFocusedNodeUp()
        {
            this.DispatchAction(Actions.ShoppingListActionCreator.MoveNodeUp(CurrentFolder.Id, FocusedNode?.Id));
        }

        private void PerformLookup()
        {
            this.DispatchAction(Actions.ComponentLookupActionCreator.ShowLookup());
        }

        #endregion

        #region Internals

        private void ApplyState(ShoppingSession state)
        {
            session = state;
            CurrentShoppingList = state.ShoppingList;
            CurrentFolder = state.ShoppingList.Content
                .FindNode<IShoppingFolder>(x => x.Id == state.CurrentFolderId);

            Title = CurrentFolder?.Title;

            Nodes.MorphTo(
                CurrentFolder?.Nodes.OfType<IShoppingNode>().ToArray() ?? new IShoppingNode[0],
                (x, y) => x.Id == y.Id,
                (x, y) => x.IsSameNode(y),
                CreateNodeViewModel);

            FocusedNode = Nodes.FirstOrDefault(x => x.Id == state.FocusedNodeId);
        }

        private ShoppingNodeListItemViewModel CreateNodeViewModel(IShoppingNode node)
        {
            var vm = new ShoppingNodeListItemViewModel(node, ExecutionEnvironment);
            vm.OpenRequested += (s, e) => EnterFolder(vm);
            return vm;
        }

        #endregion
    }
}
