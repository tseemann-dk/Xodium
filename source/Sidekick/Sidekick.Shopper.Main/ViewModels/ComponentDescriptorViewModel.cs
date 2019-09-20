using System;
using Sidekick.Shopper.Models;
using Xodium.Mvvm;
using Xodium.Mvvm.ReactiveUI;

namespace Sidekick.Shopper.ViewModels
{
    public class ComponentDescriptorViewModel : ReactiveViewModelBase<IComponentDescriptor>
    {
        public ComponentDescriptorViewModel(IComponentDescriptor model, IExecutionEnvironment executionEnvironment) 
            : base(model, executionEnvironment)
        {
        }

        public string ComponentNumber => Model.Reference.ComponentNumber;
        public string Text => Model.Text;
        public Uri ThumbnailUri => new Uri(Model.ThumbnailUrl);
        public string Price => Model.Price.ToString();
    }
}
