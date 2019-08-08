using System;
using System.Reactive.Disposables;
using ReactiveUI;
using ReactiveUI.XamForms;
using Sidekick.ViewModels;
using Xamarin.Forms.Xaml;

namespace Sidekick.Views
{
    public abstract class FolderViewBase : ReactiveContentView<FolderViewModel> { }

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FolderView : FolderViewBase
    {
        public FolderView(FolderViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;

            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel, 
                    vm => vm.Title, 
                    v => v.TitleLabel.Text)
                    .DisposeWith(disposables);

                this.OneWayBind(ViewModel, 
                    vm => vm.Nodes, 
                    v => v.NodeListView.ItemsSource)
                    .DisposeWith(disposables);

                this.OneWayBind(ViewModel,
                    vm => vm.FocusedNode,
                    v => v.NodeListView.SelectedItem)
                    .DisposeWith(disposables);

                this.BindCommand(ViewModel, 
                    vm => vm.AddNewFolderCommand, 
                    v => v.AddFolderButton)
                    .DisposeWith(disposables);

                this.BindCommand(ViewModel,
                    vm => vm.AddNewShortcutCommand,
                    v => v.AddShortcutButton)
                    .DisposeWith(disposables);

                this.BindCommand(ViewModel, 
                    vm => vm.ChangeTitleCommand, 
                    v => v.ChangeTitleButton)
                    .DisposeWith(disposables);

                this.BindCommand(ViewModel,
                    vm => vm.DeleteNodeCommand,
                    v => v.DeleteButton)
                    .DisposeWith(disposables);

                this.BindCommand(ViewModel,
                    vm => vm.EnterFolderCommand,
                    v => v.EnterFolderButton)
                    .DisposeWith(disposables);

                this.BindCommand(ViewModel,
                    vm => vm.ExitFolderCommand,
                    v => v.ExitFolderButton)
                    .DisposeWith(disposables);
            });
        }

        private void NodeListView_ItemSelected(object sender, EventArgs args)
        {
            ViewModel.FocusNode(NodeListView.SelectedItem as NodeListItemViewModel);
        }

        /*
        private void NodeCollectionView_SelectionChanged(object sender, EventArgs args)
        {
            ViewModel.SelectNode(NodeCollectionView.SelectedItem as NodeListItemViewModel);
        }
        */
    }
}
