using System.Collections.Generic;
using System.Threading.Tasks;

namespace Xodium.Mvvm
{
    public interface IWizardViewModel : IViewModel
    {
        string Title { get; set; }

        IReadOnlyList<IWizardStepViewModel> Steps { get; }
        IWizardStepViewModel CurrentStep { get; set; }
        IWizardStepViewModel NextStep { get; }
        IWizardStepViewModel PreviousStep { get; }
        IWizardStepViewModel FirstStep { get; }
        IWizardStepViewModel LastStep { get; }

        bool AllowGoToFirst { get; }
        bool AllowGoToLast { get; }

        bool CanFinish { get; }
        bool CanGoBack { get; }
        bool CanGoForward { get; }
        bool CanGoToFirstStep { get; }
        bool CanGoToLastStep { get; }

        bool IsAtFirstStep { get; }
        bool IsAtLastStep { get; }
        bool IsCurrentStepDone { get; }
        bool IsFinishVisible { get; }
        bool IsGoBackVisible { get; }
        bool IsGoForwardVisible { get; }
        bool IsGoToFirstVisible { get; }
        bool IsGoToLastVisible { get; }

        void ClearHistory();

        Task GoBack();
        Task GoForward();
        Task GoForwardTo(IWizardStepViewModel step);
        Task GoToFirstStep();
        Task GoToLastStep();
        Task GoToStep(IWizardStepViewModel step, NavigationDirection direction);
        Task Finish();

        void Update();
    }
}