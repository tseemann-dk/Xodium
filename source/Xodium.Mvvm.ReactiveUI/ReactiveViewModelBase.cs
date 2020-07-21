using ReactiveUI;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Xodium.Mvvm.ReactiveUI
{
    public abstract class ReactiveViewModelBase : ReactiveObject, IParentViewModel, INavigationDestination
    {
        private readonly ViewModelBranch branch;

        public ReactiveViewModelBase(IExecutionEnvironment executionEnvironment, IParentViewModel parentViewModel = null)
        {
            ExecutionEnvironment = executionEnvironment ?? throw new System.ArgumentNullException(nameof(executionEnvironment));
            branch = new ViewModelBranch(this, parentViewModel);
        }

        public IExecutionEnvironment ExecutionEnvironment { get; }
        public IParentViewModel ParentViewModel => branch.Parent;
        public IReadOnlyList<IViewModel> ChildViewModels => branch.Children;

        public void AddChild(IViewModel child) => branch.AddChild(child);

        public virtual Task NavigateFrom() => branch.NavigateFrom();
        public virtual Task NavigateTo() => branch.NavigateTo();
        public virtual Task NavigateBackFrom() => branch.NavigateBackFrom();
        public virtual Task NavigateBackTo() => branch.NavigateBackTo();

        public virtual Task OnNavigateFrom(NavigationDirection direction) => Task.CompletedTask;
        public virtual Task OnNavigateTo(NavigationDirection direction) => Task.CompletedTask;
    }

    public abstract class ReactiveViewModelBase<TModel> : ReactiveViewModelBase
    {
        public ReactiveViewModelBase(TModel model, IExecutionEnvironment executionEnvironment, IParentViewModel parentViewModel = null)
            : base(executionEnvironment, parentViewModel)
        {
            Model = model;
        }

        protected TModel Model { get; }
    }
}
