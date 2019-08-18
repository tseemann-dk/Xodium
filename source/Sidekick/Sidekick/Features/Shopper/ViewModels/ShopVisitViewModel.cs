using Sidekick.Features.Shopper.Models;
using System;
using Xodium.Mvvm;
using Xodium.Mvvm.ReactiveUI;

namespace Sidekick.Features.Shopper.ViewModels
{
    public class ShopVisitViewModel : ReactiveViewModelBase<IObservable<ShopVisit>>
    {
        public ShopVisitViewModel(IObservable<ShopVisit> model, IExecutionEnvironment executionEnvironment) 
            : base(model, executionEnvironment)
        {
            Model.Subscribe(state => ApplyState(state));
        }

        private void ApplyState(ShopVisit state)
        {
        }
    }
}
