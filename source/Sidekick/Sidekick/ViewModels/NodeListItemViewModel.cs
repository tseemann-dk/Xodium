using Sidekick.Models;
using Xodium.Mvvm;
using Xodium.Productivity.Content.Models;

namespace Sidekick.ViewModels
{
    public class NodeListItemViewModel : ViewModelBase<IQuantitativeNode>
    {
        public NodeListItemViewModel(IQuantitativeNode model, IExecutionEnvironment executionEnvironment) 
            : base(model, executionEnvironment)
        {
        }

        public string Id => Model.Id;
        public string Number => Model.Number;
        public string Text => Model.Text;

        public bool IsSameNode(INode node) => node == Model;
    }
}
