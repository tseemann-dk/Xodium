using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using ReactiveUI;
using Sidekick.Extensions;
using Sidekick.Shopper.Models;
using Sidekick.Shopper.State;
using Xodium.Mvvm;
using Xodium.Mvvm.ReactiveUI;

namespace Sidekick.Shopper.ViewModels
{
    public class ComponentLookupViewModel : ReactiveViewModelBase<IObservable<ComponentLookup>>
    {
        private IReadOnlyList<ComponentDescriptorViewModel> foundComponents;
        private IReadOnlyCollection<IComponentDescriptor> lastFoundComponents;
        private ComponentDescriptorViewModel selectedComponent;
        private bool isSearching;
        private bool isVisible;
        private string errorMessage;
        private string searchText;

        public ComponentLookupViewModel(IObservable<ComponentLookup> model, IExecutionEnvironment executionEnvironment) 
            : base(model, executionEnvironment)
        {
            var canSearch = this.WhenAnyValue(x => x.SearchText)
                .Select(x => !string.IsNullOrEmpty(x));

            this.WhenAnyValue(x => x.SearchText)
                .Subscribe(_ => ChangeSearchText(SearchText));

            this.WhenAnyValue(x => x.SelectedComponent)
                .Subscribe(_ => SetSelectedComponent(SelectedComponent));

            CancelCommand = ReactiveCommand.Create(() => Cancel());
            SubmitCommand = ReactiveCommand.Create(() => Submit());
            SearchCommand = ReactiveCommand.Create(() => Search(), canSearch);

            Model
                .SelectMany(ApplyState)
                .SubscribeOn(RxApp.MainThreadScheduler)
                .Subscribe();
        }

        public string ErrorMessage
        {
            get => errorMessage;
            set => this.RaiseAndSetIfChanged(ref errorMessage, value);
        }

        public IReadOnlyList<ComponentDescriptorViewModel> FoundComponents
        {
            get => foundComponents;
            set => this.RaiseAndSetIfChanged(ref foundComponents, value);
        }

        public bool IsSearching
        {
            get => isSearching;
            set => this.RaiseAndSetIfChanged(ref isSearching, value);
        }

        public bool IsVisible
        {
            get => isVisible;
            set => this.RaiseAndSetIfChanged(ref isVisible, value);
        }

        public string SearchText
        {
            get => searchText;
            set => this.RaiseAndSetIfChanged(ref searchText, value);
        }

        public ComponentDescriptorViewModel SelectedComponent
        {
            get => selectedComponent;
            set => this.RaiseAndSetIfChanged(ref selectedComponent, value);
        }

        public ReactiveCommand<Unit, Unit> CancelCommand { get; }
        public ReactiveCommand<Unit, Unit> SubmitCommand { get; }
        public ReactiveCommand<Unit, Task<Unit>> SearchCommand { get; }

        private async Task<Unit> ApplyState(ComponentLookup state)
        {
            IsSearching = state.IsSearching;
            SearchText = state.SearchText;
            ErrorMessage = state.SearchError;

            if (state.FoundComponents != lastFoundComponents)
            {
                FoundComponents = state.FoundComponents?
                    .Select(x => new ComponentDescriptorViewModel(x, ExecutionEnvironment))
                    .ToList();
                lastFoundComponents = state.FoundComponents;
            }

            SelectedComponent = FoundComponents?.FirstOrDefault(x => x.ComponentNumber == state.SelectedComponentNumber);
            IsVisible = state.IsVisible; //await SetVisible(state.IsVisible);
            return Unit.Default;
        }

        /*
        private async Task SetVisible(bool value)
        {
            if (value == isVisible) return;

            try
            {
                if (value)
                {
                    isVisible = true;
                    await ExecutionEnvironment.NavigationService.OpenPopup(this);
                }
                else
                {
                    await ExecutionEnvironment.NavigationService.GoBack();
                    isVisible = false;
                }
            }
            catch (Exception exception)
            {
                await this.HandleException(exception);
            }
        }
        */

        private void Cancel()
        {
            this.DispatchAction(Actions.ComponentLookupActionCreator.HideLookup());
        }

        private void Submit()
        {
            this.DispatchAction(Actions.ComponentLookupActionCreator.PickComponent());
        }

        public void ChangeSearchText(string value)
        {
            //if (value == SearchText) return;
            this.DispatchAction(Actions.ComponentLookupActionCreator.SetSearchText(value));
        }

        public async Task<Unit> Search()
        {
            //await this.DispatchActionsAsync(Actions.ComponentLookupActionCreator.Search());

            var shop = ExecutionEnvironment.GetService<IShop>();
            await this.DispatchActionAsync(Actions.ComponentLookupActionCreator.Search(shop));
            return Unit.Default;
        }

        public void SetSelectedComponent(ComponentDescriptorViewModel value)
        {
            //if (value == SelectedComponent) return;
            this.DispatchAction(Actions.ComponentLookupActionCreator.SelectComponent(value?.ComponentNumber));
        }
    }
}
