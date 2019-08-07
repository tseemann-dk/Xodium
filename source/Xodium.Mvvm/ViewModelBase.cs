using System.Collections.Generic;
using System.Threading.Tasks;

namespace Xodium.Mvvm
{
    public abstract class ViewModelBase : ObservableObject, IParentViewModel
    {
        private readonly ViewModelBranch branch;
        private readonly CommandCollection commands;

        public ViewModelBase(IExecutionEnvironment executionEnvironment, IParentViewModel parentViewModel = null)
        {
            ExecutionEnvironment = executionEnvironment ?? throw new System.ArgumentNullException(nameof(executionEnvironment));
            branch = new ViewModelBranch(this, parentViewModel);
            commands = new CommandCollection();
        }

        public IExecutionEnvironment ExecutionEnvironment { get; }
        public IParentViewModel ParentViewModel => branch.Parent;
        public IReadOnlyList<IViewModel> ChildViewModels => branch.Children;

        public event NavigationEventHandler NavigatedTo;
        public event NavigationEventHandler NavigatedFrom;

        public void AddChild(IViewModel child) => branch.AddChild(child);
        public IAsyncCommand AddCommand(IAsyncCommand command) => commands.AddCommand(command);

        public void UpdateCommands() => commands.Update();

        #region Navigation

        public Task NavigateTo() => branch.NavigateTo();
        public Task NavigateBackTo() => branch.NavigateBackTo();
        public Task NavigateFrom() => branch.NavigateFrom();
        public Task NavigateBackFrom() => branch.NavigateBackFrom();

        public virtual async Task OnNavigateTo(NavigationDirection direction)
        {
            if (direction == NavigationDirection.Forward)
            {
                await OnArrival();
            }

            OnNavigatedTo(direction);
        }

        public virtual async Task OnNavigateFrom(NavigationDirection direction)
        {
            if (direction == NavigationDirection.Backward)
            {
                await OnDeparture();
            }

            OnNavigatedFrom(direction);
        }

        protected virtual Task OnArrival() => Task.CompletedTask;
        protected virtual Task OnDeparture() => Task.CompletedTask;


        private void  OnNavigatedTo(NavigationDirection direction)
        {
            NavigatedTo?.Invoke(this, new NavigationEventArgs(direction));
        }

        private void OnNavigatedFrom(NavigationDirection direction)
        {
            NavigatedFrom?.Invoke(this, new NavigationEventArgs(direction));
        }

        #endregion
    }

    public abstract class ViewModelBase<TModel> : ViewModelBase
    {
        public ViewModelBase(TModel model, IExecutionEnvironment executionEnvironment, IParentViewModel parentViewModel = null)
            : base(executionEnvironment, parentViewModel)
        {
            Model = model;
        }

        public TModel Model { get; }
    }
}
