using System.Threading.Tasks;
using System.Windows.Input;

namespace Xodium.Mvvm.Mvvm
{
    public class WizardStepViewModelBase : ViewModelBase, IWizardStepViewModel
    {
        private string title;
        private bool isStepEnabled = true;

        public WizardStepViewModelBase(WizardViewModelBase wizard)
            : base(wizard.ExecutionEnvironment)
        {
            Wizard = wizard;
            InitializeCommands();
        }

        #region Commands

        public ICommand GoBackCommand { get; private set; }
        public ICommand GoForwardCommand { get; private set; }
        public ICommand GoToFirstStepCommand { get; private set; }
        public ICommand GoToLastStepCommand { get; private set; }
        public ICommand FinishCommand { get; private set; }

        private Task GoBack() => Wizard.GoBack();
        private Task GoForward() => Wizard.GoForward();
        private Task GoToFirstStep() => Wizard.GoToFirstStep();
        private Task GoToLastStep() => Wizard.GoToLastStep();
        private Task Finish() => Wizard.Finish();

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

        public virtual bool AllowGoToFirst => false;
        public virtual bool AllowGoToLast => false;

        public bool CanGoBack => Wizard.CanGoBack;
        public bool CanGoForward => Wizard.CanGoForward;
        public bool CanGoToFirstStep => Wizard.CanGoToFirstStep;
        public bool CanGoToLastStep => Wizard.CanGoToLastStep;
        public bool CanFinish => Wizard.CanFinish;

        public bool IsGoBackVisible => Wizard.IsGoBackVisible;
        public bool IsGoForwardVisible => Wizard.IsGoForwardVisible;
        public bool IsGoToFirstVisible => Wizard.IsGoToFirstVisible;
        public bool IsGoToLastVisible => Wizard.IsGoToLastVisible;
        public bool IsFinishVisible => Wizard.IsFinishVisible;

        public bool IsStepEnabled
        {
            get => isStepEnabled;
            set
            {
                if (!SetProperty(ref isStepEnabled, value)) return;
                Wizard.Update();
            }
        }

        public virtual bool IsDone => true;

        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        public IWizardViewModel Wizard { get; }

        #endregion

        public void Update()
        {
            UpdateCommands();
        }
    }
}
