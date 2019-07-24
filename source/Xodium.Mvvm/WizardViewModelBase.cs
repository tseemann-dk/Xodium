using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Xodium.Mvvm
{
    public class WizardViewModelBase : ViewModelBase, IWizardViewModel
    {
        private readonly Stack<IWizardStepViewModel> history = new Stack<IWizardStepViewModel>();
        private IWizardStepViewModel currentStep;
        private string title;

        public WizardViewModelBase(IExecutionEnvironment environment, IParentViewModel parentViewModel = null)
            : base(environment, parentViewModel)
        {
            InitializeCommands();
        }

        #region Commands

        public ICommand GoBackCommand { get; private set; }
        public ICommand GoForwardCommand { get; private set; }
        public ICommand GoToFirstStepCommand { get; private set; }
        public ICommand GoToLastStepCommand { get; private set; }
        public ICommand FinishCommand { get; private set; }

        public virtual bool CanGoBack => history.Any();
        public virtual bool CanGoForward => !IsAtLastStep && IsCurrentStepDone;
        public virtual bool CanGoToFirstStep => !IsAtFirstStep && FirstStep != null;
        public virtual bool CanGoToLastStep => !IsAtLastStep && LastStep != null;
        public virtual bool CanFinish => IsAtLastStep && IsCurrentStepDone;

        private void InitializeCommands()
        {
            GoBackCommand = AddCommand(new Command(GoBack, () => CanGoBack));
            GoForwardCommand = AddCommand(new Command(GoForward, () => CanGoForward));
            GoToFirstStepCommand = AddCommand(new Command(GoToFirstStep, () => CanGoToFirstStep));
            GoToLastStepCommand = AddCommand(new Command(GoToLastStep, () => CanGoToLastStep));
            FinishCommand = AddCommand(new Command(Finish, () => CanFinish));
        }

        #endregion

        #region Properties

        public bool AllowGoToFirst => CurrentStep?.AllowGoToFirst ?? false;
        public bool AllowGoToLast => CurrentStep?.AllowGoToLast ?? false;

        public IWizardStepViewModel CurrentStep
        {
            get => currentStep ?? FirstStep;
            set
            {
                if (!SetProperty(ref currentStep, value)) return;
                OnPropertyChanged(nameof(CurrentStepIndex));
                OnCurrentStepChanged();
                UpdateCommands();
            }
        }

        public int CurrentStepIndex => Steps.IndexOf(CurrentStep);
        public int LastStepIndex => Steps.Count - 1;

        public IWizardStepViewModel FirstStep => GetNextStep(-1);
        public IWizardStepViewModel LastStep => GetPriorStep(Steps.Count);
        public IWizardStepViewModel PreviousStep => history.Any() ? history.Peek() : null;
        public IWizardStepViewModel NextStep => GetNextStep(CurrentStepIndex);

        public bool IsAtFirstStep => CurrentStep != null && CurrentStep == FirstStep;
        public bool IsAtLastStep => CurrentStep != null && CurrentStep == LastStep;
        public bool IsGoBackVisible => PreviousStep != null;
        public bool IsGoForwardVisible => NextStep != null;
        public bool IsGoToFirstVisible => !IsAtFirstStep && AllowGoToFirst;
        public bool IsGoToLastVisible => !IsAtLastStep && AllowGoToLast;
        public bool IsFinishVisible => IsAtLastStep;
        public bool IsCurrentStepDone => CurrentStep == null || GetIsStepDone(CurrentStep);

        public List<IWizardStepViewModel> Steps { get; } = new List<IWizardStepViewModel>();
        IReadOnlyList<IWizardStepViewModel> IWizardViewModel.Steps => Steps;

        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        #endregion

        #region Public Methods

        public void ClearHistory()
        {
            history.Clear();
        }

        public async Task Finish()
        {
            if (!CanFinish) return;
            await OnFinish();
        }

        public async Task GoBack()
        {
            if (!CanGoBack) return;
            await GoToStep(RecallStep(), NavigationDirection.Backward);
        }

        public async Task GoForward()
        {
            if (!CanGoForward) return;
            RememberStep();
            await GoToStep(NextStep, NavigationDirection.Forward);
        }

        public async Task GoForwardTo(IWizardStepViewModel step)
        {
            if (!IsCurrentStepDone) return;
            RememberStep();
            await EnterStep(step, NavigationDirection.Forward);
        }

        public async Task GoToFirstStep()
        {
            if (!CanGoToFirstStep) return;
            ClearHistory();
            await GoToStep(FirstStep, NavigationDirection.Forward);
        }

        public async Task GoToLastStep()
        {
            if (!CanGoToLastStep) return;
            ClearHistory();
            await GoToStep(LastStep, NavigationDirection.Forward);
        }

        public async Task GoToStep(IWizardStepViewModel step, NavigationDirection direction)
        {
            if (CurrentStep != null)
            {
                await LeaveStep(CurrentStep, direction);
            }

            CurrentStep = step;

            if (step != null)
            {
                await EnterStep(step, direction);
            }
        }

        public void Update()
        {
            UpdateCommands();

            foreach (var step in ChildViewModels.OfType<IWizardStepViewModel>())
            {
                step.Update();
            }
        }

        #endregion

        #region Protected Methods

        protected virtual bool GetIsStepEnabled(IWizardStepViewModel step) => step?.IsStepEnabled ?? false;
        protected virtual bool GetIsStepDone(IWizardStepViewModel step) => step?.IsDone ?? true;

        protected virtual IWizardStepViewModel GetNextStep(int index) => GetNextStep(index, 1);
        protected virtual IWizardStepViewModel GetPriorStep(int index) => GetNextStep(index, -1);

        protected virtual void OnCurrentStepChanged() { }
        protected virtual Task OnEnterStep(IWizardStepViewModel step, NavigationDirection direction) => Task.CompletedTask;
        protected virtual Task OnLeaveStep(IWizardStepViewModel step, NavigationDirection direction) => Task.CompletedTask;

        protected virtual Task OnFinish()
        {
            return Task.CompletedTask;
        }

        public override async Task OnNavigateFrom(NavigationDirection direction)
        {
            if (CurrentStep != null)
            {
                if (direction == NavigationDirection.Forward)
                {
                    await CurrentStep.NavigateFrom();
                }
                else
                {
                    await CurrentStep.NavigateBackFrom();
                }
            }

            await base.OnNavigateFrom(direction);
        }

        public override async Task OnNavigateTo(NavigationDirection direction)
        {
            await base.OnNavigateTo(direction);

            if (CurrentStep == null) return;

            if (direction == NavigationDirection.Forward)
            {
                await CurrentStep.NavigateTo();
            }
            else
            {
                await CurrentStep.NavigateBackTo();
            }
        }

        #endregion

        #region Private Methods

        private IWizardStepViewModel GetNextStep(int index, int increment)
        {
            for (var i = index + increment; i >= 0 && i < Steps.Count; i += increment)
            {
                var step = Steps[i];

                if (GetIsStepEnabled(step))
                {
                    return step;
                }
            }

            return null;
        }

        private async Task EnterStep(IWizardStepViewModel step, NavigationDirection direction)
        {
            await OnEnterStep(step, direction);

            if (direction == NavigationDirection.Forward)
            {
                await step.NavigateTo();
            }
            else
            {
                await step.NavigateBackTo();
            }
        }

        private async Task LeaveStep(IWizardStepViewModel step, NavigationDirection direction)
        {
            if (direction == NavigationDirection.Forward)
            {
                await step.NavigateFrom();
            }
            else
            {
                await step.NavigateBackFrom();
            }

            await OnLeaveStep(step, direction);
        }

        private void RememberStep()
        {
            if (CurrentStep == null) return;
            history.Push(CurrentStep);
        }

        private IWizardStepViewModel RecallStep()
        {
            return history.Any() ? history.Pop() : null;
        }

        #endregion
    }
}
