using System;
using ReactiveUI;
using Sidekick.Features.Shopper.Actions.ShopVisit;
using Sidekick.Features.Shopper.Models;
using Xodium.Mvvm;
using Xodium.Mvvm.ReactiveUI;

namespace Sidekick.Features.Shopper.ViewModels
{
    public class ShopVisitViewModel : ReactiveViewModelBase<IObservable<ShopVisit>>
    {
        private string searchText;

        public ShopVisitViewModel(IObservable<ShopVisit> model, IExecutionEnvironment executionEnvironment) 
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
            this.DispatchAction(new ChangeSearchTextAction(value));
        }

        private void ApplyState(ShopVisit state)
        {
            SearchText = state.SearchText;
        }
    }
}
