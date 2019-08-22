using Sidekick.Features.Shopper.Models;
using Xodium.Mvvm;
using Xodium.Mvvm.ReactiveUI;

namespace Sidekick.Features.Shopper.ViewModels
{
    public class ComponentDescriptorViewModel : ReactiveViewModelBase<IComponentDescriptor>
    {
        public ComponentDescriptorViewModel(IComponentDescriptor model, IExecutionEnvironment executionEnvironment) 
            : base(model, executionEnvironment)
        {
        }

        public string ComponentNumber => Model.Reference.ComponentNumber;
        public string Text => Model.Text;
        public string Price => Model.Price.ToString();
    }
}
