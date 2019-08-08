using Sidekick.Models;
using System;
using Xodium.Mvvm;
using Xodium.Productivity.Content.Models;

namespace Sidekick.ViewModels
{
    public class NodeListItemViewModel : ViewModelBase<IArchiveNode>
    {
        public NodeListItemViewModel(IArchiveNode model, IExecutionEnvironment executionEnvironment) 
            : base(model, executionEnvironment)
        {
        }

        public string Id => Model.Id;
        public string DisplayNumber => Model.ReferenceNumber;
        public string Text => Model.Text;

        public bool IsSameNode(INode node) => node == Model;
    }
}
