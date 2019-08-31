using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.XamForms;
using Sidekick.Shopper.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sidekick.Shopper.Views
{
    public class ComponentLookupViewBase : ReactiveContentView<ComponentLookupViewModel> { }

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ComponentLookupView : ComponentLookupViewBase
    {
        public ComponentLookupView()
        {
            InitializeComponent();

            this.WhenActivated(disposable =>
            {
                this.OneWayBind(ViewModel, vm => vm.FoundComponents, v => v.FoundComponentsListView.ItemsSource)
                    .DisposeWith(disposable);

                this.OneWayBind(ViewModel, vm => vm.SelectedComponent, v => v.FoundComponentsListView.SelectedItem)
                    .DisposeWith(disposable);

                this.OneWayBind(ViewModel, vm => vm.SearchText, v => v.SearchTextEntry.Text)
                    .DisposeWith(disposable);

                this.BindCommand(ViewModel, vm => vm.SearchCommand, v => v.SearchButton)
                    .DisposeWith(disposable);

                this.OneWayBind(ViewModel, vm => vm.ErrorMessage, v => v.ErrorMessageLabel.Text)
                    .DisposeWith(disposable);

                Observable
                    .FromEventPattern<TextChangedEventArgs>(SearchTextEntry, nameof(SearchBar.TextChanged))
                    .Select(x => x.EventArgs.NewTextValue)
                    .Subscribe(x => ViewModel.ChangeSearchText(x), e => { })
                    .DisposeWith(disposable);
            });
        }

        private void FoundComponentsListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ViewModel.SetSelectedComponent(e.SelectedItem as ComponentDescriptorViewModel);
        }
    }
}
