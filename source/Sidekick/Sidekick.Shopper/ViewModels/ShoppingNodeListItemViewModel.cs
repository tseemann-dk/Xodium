using System;
using System.Reactive;
using ReactiveUI;
using Sidekick.Shopper.Models;
using Sidekick.UI.Resources;
using Xodium.Mvvm;
using Xodium.Productivity.Content.Models;

namespace Sidekick.Shopper.ViewModels
{
    public class ShoppingNodeListItemViewModel : ViewModelBase<IShoppingNode>
    {
        public ShoppingNodeListItemViewModel(IShoppingNode model, IExecutionEnvironment executionEnvironment) 
            : base(model, executionEnvironment)
        {
            OpenCommand = ReactiveCommand.Create(RequestOpen);
        }

        public string Id => Model.Id;
        public string DisplayNumber => Model.ReferenceNumber;
        public string Glyph => IsGroup ? MaterialDesignIcon.Folder : MaterialDesignIcon.File;
        public bool IsGroup => Model is IShoppingGroup;
        public bool IsItem => Model is IShoppingItem;
        public string Text => Model.Text;
        public Uri ThumbnailUri => (Model is IShoppingItem item && item.ThumbnailUrl != null) ? new Uri(item.ThumbnailUrl) : null;
        public double Value => Model.Price;

        public ReactiveCommand<Unit, Unit> OpenCommand { get; }

        public event EventHandler OpenRequested;

        public bool IsFirstNodeIn(IShoppingGroup group) => Model.IsFirstChildOf(group);
        public bool IsLastNodeIn(IShoppingGroup group) => Model.IsLastChildOf(group);
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
