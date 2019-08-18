using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using ReactiveUI;
using Sidekick.Extensions;
using Sidekick.Features.Shopper.Actions.ShoppingList;
using Sidekick.Features.Shopper.Actions.ShoppingSession;
using Sidekick.Features.Shopper.Models;
using Xodium.Collections;
using Xodium.Mvvm;
using Xodium.Mvvm.ReactiveUI;
using Xodium.Productivity.Content.Models;

namespace Sidekick.Features.Shopper.ViewModels
{
    public class ShoppingGroupViewModel : ReactiveViewModelBase<IObservable<ShoppingSession>>
    {
        private string title;
        private IShoppingGroup currentGroup;
        private IShoppingList currentShoppingList;
        private ShoppingNodeListItemViewModel focusedNode;
        private readonly ObservableAsPropertyHelper<string> focusedNodeText;

        public ShoppingGroupViewModel(IObservable<ShoppingSession> model, IExecutionEnvironment executionEnvironment) 
            : base(model, executionEnvironment)
        {
            Nodes = new ObservableCollection<ShoppingNodeListItemViewModel>();

            var focusedNodeChanges = this
                .WhenAnyValue(x => x.FocusedNode);

            var currentGroupChanges = this
                .WhenAnyValue(x => x.CurrentGroup);

            var currentGroupOrShoppingListChanges = this
                .WhenAnyValue(x => x.CurrentGroup, x => x.CurrentShoppingList, 
                (group, shoppingList) => (group, shoppingList));

            var currentGroupOrFocusedNodeChanges = this
                .WhenAnyValue(x => x.CurrentGroup, x => x.FocusedNode, 
                (group, node) => (group, node));

            var hasFocusedNode = focusedNodeChanges
                .Select(x => x != null);

            var isAtRoot = currentGroupOrShoppingListChanges
                .Select(x => x.group == x.shoppingList?.Content);

            var isNotAtRoot = isAtRoot
                .Select(x => !x);

            var hasCurrentGroup = currentGroupChanges
                .Select(x => x != null);

            var focusedNodeIsGroup = focusedNodeChanges
                .Select(x => x?.Model is ShoppingGroup);

            var focusedNodeIsNotFirst = currentGroupOrFocusedNodeChanges
                .Select(x => !(x.node?.IsFirstNodeIn(x.group) ?? true));

            var focusedNodeIsNotLast = currentGroupOrFocusedNodeChanges
                .Select(x => !(x.node?.IsLastNodeIn(x.group) ?? true));

            AddNewGroupCommand = ReactiveCommand.Create(() => AddNewGroup());
            AddNewItemCommand = ReactiveCommand.Create(() => AddNewItem());
            ChangeTitleCommand = ReactiveCommand.Create(() => ChangeTitle(), hasCurrentGroup);
            DeleteNodeCommand = ReactiveCommand.Create(() => DeleteNode(), hasFocusedNode);
            EnterGroupCommand = ReactiveCommand.Create(() => EnterFocusedGroup(), focusedNodeIsGroup);
            ExitGroupCommand = ReactiveCommand.Create(() => ExitGroup(), isNotAtRoot);
            MoveNodeDownCommand = ReactiveCommand.Create(() => MoveFocusedNodeDown(), focusedNodeIsNotLast);
            MoveNodeUpCommand = ReactiveCommand.Create(() => MoveFocusedNodeUp(), focusedNodeIsNotFirst);

            //AddNewItemCommand.ThrownExceptions.Subscribe(e => this.HandleException(e));

            focusedNodeText = this.WhenAnyValue(x => x.FocusedNode)
                .Select(x => x?.Text)
                .ToProperty(this, x => x.FocusedNodeText);

            Model.Subscribe(state => ApplyState(state));
        }

        private void ApplyState(ShoppingSession state)
        {
            CurrentShoppingList = state.ShoppingList;

            CurrentGroup = state.ShoppingList.Content
                .FindNode<IShoppingGroup>(x => x.Id == state.CurrentGroupId);

            Title = CurrentGroup?.Title;

            Nodes.MorphTo(
                CurrentGroup?.Nodes.OfType<IShoppingNode>().ToArray() ?? new IShoppingNode[0],
                (x, y) => x.Id == y.Id,
                (x, y) => x.IsSameNode(y),
                CreateNodeViewModel);

            FocusedNode = Nodes.FirstOrDefault(x => x.Id == state.FocusedNodeId);
        }

        public ReactiveCommand<Unit, Unit> AddNewGroupCommand { get; }
        public ReactiveCommand<Unit, Task<Unit>> AddNewItemCommand { get; }
        public ReactiveCommand<Unit, Unit> ChangeTitleCommand { get; }
        public ReactiveCommand<Unit, Unit> DeleteNodeCommand { get; }
        public ReactiveCommand<Unit, Unit> EnterGroupCommand { get; }
        public ReactiveCommand<Unit, Unit> ExitGroupCommand { get; }
        public ReactiveCommand<Unit, Unit> MoveNodeDownCommand { get; }
        public ReactiveCommand<Unit, Unit> MoveNodeUpCommand { get; }

        public IShoppingGroup CurrentGroup
        {
            get => currentGroup;
            set => this.RaiseAndSetIfChanged(ref currentGroup, value);
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

        public void EnterGroup(ShoppingNodeListItemViewModel node)
        {
            if (!(node.Model is IShoppingGroup group)) return;

            this.DispatchAction(new EnterGroupAction(group.Id));
        }

        public void FocusNode(ShoppingNodeListItemViewModel node)
        {
            if (node?.Id == FocusedNode?.Id) return;

            this.DispatchAction(new FocusNodeAction(node?.Id));
        }

        private void AddNewGroup()
        {
            var groupNumber = this.GetAppState().Global.NextGroupNumber;
            var title = $"Group {groupNumber}";

            this.DispatchAction(new AddGroupAction(
                CurrentGroup.Id, 
                new ShoppingGroup($"G{groupNumber}", title, 1), 
                FocusedNode?.Id)
            );
        }

        private async Task<Unit> AddNewItem()
        {
            try
            {
                var vm = new ShopVisitViewModel(Model.Select(x => x.ShopVisit).DistinctUntilChanged(), ExecutionEnvironment);
                await this.OpenPopup(vm);
            }
            catch (Exception exception)
            {
                await this.HandleException(exception);
            }

            return Unit.Default;

            /*
            var componentNumber = this.GetAppState().Global.NextComponentNumber;
            var component = new Component(ShopIdentity.Internal, componentNumber.ToString(), $"Component {componentNumber}", 10);
            var item = new ShoppingItem(component, 1);

            this.DispatchAction(new AddComponentAction(component));
            this.DispatchAction(new AddItemAction(CurrentGroup.Id, item, insertAfterNodeId: FocusedNode?.Id));
            */
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

            this.DispatchAction(new ChangeGroupTitleAction(CurrentGroup.Id, newTitle));
        }

        private ShoppingNodeListItemViewModel CreateNodeViewModel(IShoppingNode node)
        {
            var vm = new ShoppingNodeListItemViewModel(node, ExecutionEnvironment);
            vm.OpenRequested += (s, e) => EnterGroup(vm);
            return vm;
        }

        private void DeleteNode()
        {
            if (focusedNode == null) return;

            this.DispatchAction(new DeleteNodeAction(CurrentGroup.Id, FocusedNode.Id));
        }

        private void EnterFocusedGroup()
        {
            EnterGroup(focusedNode);
        }

        private void ExitGroup()
        {
            this.DispatchAction(new ExitGroupAction());
        }

        private void MoveFocusedNodeDown()
        {
            this.DispatchAction(new MoveNodeDownAction(CurrentGroup.Id, FocusedNode?.Id));
        }

        private void MoveFocusedNodeUp()
        {
            this.DispatchAction(new MoveNodeUpAction(CurrentGroup.Id, FocusedNode?.Id));
        }
    }
}
