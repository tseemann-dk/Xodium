using Sidekick.Models;
using System;
using Xodium.Mvvm;
using Xodium.Productivity.Content.Models;

namespace Sidekick.ViewModels
{
    public class NodeListItemViewModel : ViewModelBase<IProjectNode>
    {
        public NodeListItemViewModel(IProjectNode model, IExecutionEnvironment executionEnvironment) 
            : base(model, executionEnvironment)
        {
        }

        public string Id => Model.Id;
        public DateTime? Date => Model is ILine line ? line.Date : (DateTime?)null;
        public string Text => Model.Text;

        public bool IsSameNode(INode node) => node == Model;
    }
}
