using System;
using ReactiveUI;
using Sidekick.Features.Shopper.Actions.ComponentLookup;
using Sidekick.Features.Shopper.Models;
using Xodium.Mvvm;
using Xodium.Mvvm.ReactiveUI;

namespace Sidekick.Features.Shopper.ViewModels
{
    public class ComponentLookupViewModel : ReactiveViewModelBase<IObservable<ComponentLookup>>
    {
        private string searchText;

        public ComponentLookupViewModel(IObservable<ComponentLookup> model, IExecutionEnvironment executionEnvironment) 
            : base(model, executionEnvironment)
        {
            Model.Subscribe(state => ApplyState(state));
        }

        public string SearchText
        {
            get => searchText;
            set => this.RaiseAndSetIfChanged(ref searchText, value);
        }

        public void ChangeSearchText(string value)
        {
            if (value == SearchText) return;
            this.DispatchAction(new ChangeSearchTextAction(value));
        }

        private void ApplyState(ComponentLookup state)
        {
            SearchText = state.SearchText;
        }
    }
}
