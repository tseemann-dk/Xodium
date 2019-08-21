using System;
using System.Reactive;
using ReactiveUI;
using Sidekick.Extensions;
using Sidekick.Features.Shopper.Models;
using Xodium.Mvvm;
using Xodium.Mvvm.ReactiveUI;

namespace Sidekick.Features.Shopper.ViewModels
{
    public class ComponentLookupViewModel : ReactiveViewModelBase<IObservable<ComponentLookup>>
    {
        private bool isVisible;
        private string searchText;

        public ComponentLookupViewModel(IObservable<ComponentLookup> model, IExecutionEnvironment executionEnvironment) 
            : base(model, executionEnvironment)
        {
            CancelCommand = ReactiveCommand.Create(() => Cancel());
            CommitCommand = ReactiveCommand.Create(() => Commit());

            Model.Subscribe(state => ApplyState(state));
        }

        public string SearchText
        {
            get => searchText;
            set => this.RaiseAndSetIfChanged(ref searchText, value);
        }

        public ReactiveCommand<Unit, Unit> CancelCommand { get; }
        public ReactiveCommand<Unit, Unit> CommitCommand { get; }

        private void ApplyState(ComponentLookup state)
        {
            SearchText = state.SearchText;

            SetVisible(state.IsVisible);
        }

        private async void SetVisible(bool value)
        {
            if (value == isVisible) return;

            try
            {
                if (value)
                {
                    await ExecutionEnvironment.NavigationService.OpenPopup(this);
                    isVisible = true;
                }
                else
                {
                    await ExecutionEnvironment.NavigationService.GoBack();
                    isVisible = false;
                }
            }
            catch (Exception exception)
            {
                await this.HandleException(exception);
            }
        }

        private void Cancel()
        {
            this.DispatchAction(new Actions.ComponentLookup.HideAction());
        }

        private void Commit()
        {
            this.DispatchAction(new Actions.ComponentLookup.CommitAction());
        }

        public void ChangeSearchText(string value)
        {
            if (value == SearchText) return;
            this.DispatchAction(new Actions.ComponentLookup.ChangeSearchTextAction(value));
        }
    }
}
