using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.XamForms;
using Sidekick.Features.Shopper.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sidekick.Features.Shopper.Views
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
                this.OneWayBind(ViewModel, vm => vm.SearchText, v => v.SearchTextEntry.Text)
                    .DisposeWith(disposable);

                Observable
                    .FromEventPattern<TextChangedEventArgs>(SearchTextEntry, nameof(Entry.TextChanged))
                    .Select(x => x.EventArgs.NewTextValue)
                    .Subscribe(x => ViewModel.ChangeSearchText(x), e => { });
            });
        }
    }
}
