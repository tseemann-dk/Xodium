namespace Xodium.Mvvm
{
    public interface IWizardStepViewModel : IViewModel
    {
        string Title { get; set; }
        IWizardViewModel Wizard { get; }

        bool AllowGoToFirst { get; }
        bool AllowGoToLast { get; }

        bool CanGoBack { get; }
        bool CanGoForward { get; }
        bool CanGoToFirstStep { get; }
        bool CanGoToLastStep { get; }
        bool CanFinish { get; }

        bool IsDone { get; }
        bool IsFinishVisible { get; }
        bool IsGoBackVisible { get; }
        bool IsGoForwardVisible { get; }
        bool IsGoToFirstVisible { get; }
        bool IsGoToLastVisible { get; }
        bool IsStepEnabled { get; set; }

        void Update();
    }
}