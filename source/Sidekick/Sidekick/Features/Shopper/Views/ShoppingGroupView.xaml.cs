using System;
using System.Reactive.Disposables;
using ReactiveUI;
using ReactiveUI.XamForms;
using Sidekick.Features.Shopper.ViewModels;
using Sidekick.Resources;
using Xamarin.Forms.Xaml;

namespace Sidekick.Features.Shopper.Views
{
    public abstract class ShoppingGroupViewBase : ReactiveContentView<ShoppingGroupViewModel> { }

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShoppingGroupView : ShoppingGroupViewBase
    {
        public ShoppingGroupView(ShoppingGroupViewModel viewModel)
        {
            InitializeComponent();

            AddItemButton.Glyph = MaterialDesignIcon.Plus;
            AddGroupButton.Glyph = MaterialDesignIcon.FolderPlus;
            DeleteButton.Glyph = MaterialDesignIcon.Delete;
            GoBackButton.Glyph = MaterialDesignIcon.ArrowLeftBold;
            GoForwardButton.Glyph = MaterialDesignIcon.ArrowRightBold;
            MoveDownButton.Glyph = MaterialDesignIcon.ArrowDown;
            MoveUpButton.Glyph = MaterialDesignIcon.ArrowUp;
            RenameButton.Glyph = MaterialDesignIcon.RenameBox;

            ViewModel = viewModel;

            this.WhenActivated(disposables =>
            {
                // Title
                this.OneWayBind(ViewModel,
                    vm => vm.Title,
                    v => v.TitleLabel.Text)
                    .DisposeWith(disposables);

                // NodeCollectionView.Items
                this.OneWayBind(ViewModel,
                    vm => vm.Nodes,
                    v => v.NodeCollectionView.ItemsSource)
                    .DisposeWith(disposables);

                // NodeCollectionView.SelectedItem
                this.Bind(ViewModel,
                    vm => vm.FocusedNode,
                    v => v.NodeCollectionView.SelectedItem)
                    .DisposeWith(disposables);

                // Info
                this.OneWayBind(ViewModel,
                    vm => vm.FocusedNodeText,
                    v => v.InfoLabel.Text)
                    .DisposeWith(disposables);

                // AddGroupButton
                this.BindCommand(ViewModel, 
                    vm => vm.AddNewGroupCommand, 
                    v => v.AddGroupButton)
                    .DisposeWith(disposables);

                // AddItemButton
                this.BindCommand(ViewModel,
                    vm => vm.AddNewItemCommand,
                    v => v.AddItemButton)
                    .DisposeWith(disposables);

                // RenameButton
                this.BindCommand(ViewModel, 
                    vm => vm.ChangeTitleCommand, 
                    v => v.RenameButton)
                    .DisposeWith(disposables);

                // DeletButton
                this.BindCommand(ViewModel,
                    vm => vm.DeleteNodeCommand,
                    v => v.DeleteButton)
                    .DisposeWith(disposables);

                // MoveDownButton
                this.BindCommand(ViewModel,
                    vm => vm.MoveNodeDownCommand,
                    v => v.MoveDownButton)
                    .DisposeWith(disposables);

                // MoveUpButton
                this.BindCommand(ViewModel,
                    vm => vm.MoveNodeUpCommand,
                    v => v.MoveUpButton)
                    .DisposeWith(disposables);

                // GoForwardButton
                this.BindCommand(ViewModel,
                    vm => vm.EnterGroupCommand,
                    v => v.GoForwardButton)
                    .DisposeWith(disposables);

                // GoBackButton
                this.BindCommand(ViewModel,
                    vm => vm.ExitGroupCommand,
                    v => v.GoBackButton)
                    .DisposeWith(disposables);
            });
        }

        //private void NodeCollectionView_SelectionChanged(object sender, Xamarin.Forms.SelectionChangedEventArgs e)
        //{
        //    ViewModel.FocusNode(NodeCollectionView.SelectedItem as NodeListItemViewModel);
        //}

        //private void NodeListView_ItemSelected(object sender, EventArgs args)
        //{
        //    ViewModel.FocusNode(NodeListView.SelectedItem as NodeListItemViewModel);
        //}

        //private void NodeListView_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        //{
        //    ViewModel.FocusNode(NodeListView.SelectedItem as NodeListItemViewModel);
        //}
    }
}
