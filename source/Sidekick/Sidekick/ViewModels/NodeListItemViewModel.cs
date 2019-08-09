using ReactiveUI;
using Sidekick.Models;
using System;
using System.Reactive;
using Xodium.Mvvm;
using Xodium.Productivity.Content.Models;

namespace Sidekick.ViewModels
{
    public class NodeListItemViewModel : ViewModelBase<IArchiveNode>
    {
        public NodeListItemViewModel(IArchiveNode model, IExecutionEnvironment executionEnvironment) 
            : base(model, executionEnvironment)
        {
            OpenCommand = ReactiveCommand.Create(RequestOpen);
        }

        public string Id => Model.Id;
        public string DisplayNumber => Model.ReferenceNumber;
        public string Text => Model.Text;
        public double Value => Model.Value;

        public ReactiveCommand<Unit, Unit> OpenCommand { get; }

        public event EventHandler OpenRequested;

        public bool IsFirstNodeIn(IFolder folder) => Model.IsFirstChildOf(folder);
        public bool IsLastNodeIn(IFolder folder) => Model.IsLastChildOf(folder);
        public bool IsSameNode(INode node) => node == Model;

        protected void OnOpenRequested()
        {
            OpenRequested?.Invoke(this, EventArgs.Empty);
        }

        private void RequestOpen()
        {
            OnOpenRequested();
        }
    }
}
