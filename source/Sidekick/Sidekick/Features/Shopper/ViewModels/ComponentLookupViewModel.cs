using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using ReactiveUI;
using Sidekick.Extensions;
using Sidekick.Features.Shopper.Models;
using Xodium.Mvvm;
using Xodium.Mvvm.ReactiveUI;

namespace Sidekick.Features.Shopper.ViewModels
{
    public class ComponentLookupViewModel : ReactiveViewModelBase<IObservable<ComponentLookup>>
    {
        private IReadOnlyList<ComponentDescriptorViewModel> foundComponents;
        private IReadOnlyCollection<IComponentDescriptor> lastFoundComponents;
        private bool isSearching;
        private bool isVisible;
        private string searchText;

        public ComponentLookupViewModel(IObservable<ComponentLookup> model, IExecutionEnvironment executionEnvironment) 
            : base(model, executionEnvironment)
        {
            var canSearch = this
                .WhenAnyValue(x => x.SearchText)
                .Select(x => !string.IsNullOrEmpty(x));

            CancelCommand = ReactiveCommand.Create(() => Cancel());
            CommitCommand = ReactiveCommand.Create(() => Commit());
            SearchCommand = ReactiveCommand.Create(() => Search(), canSearch);

            Model.Subscribe(state => ApplyState(state));
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

        public string SearchText
        {
            get => searchText;
            set => this.RaiseAndSetIfChanged(ref searchText, value);
        }

        public ReactiveCommand<Unit, Unit> CancelCommand { get; }
        public ReactiveCommand<Unit, Unit> CommitCommand { get; }
        public ReactiveCommand<Unit, Task<Unit>> SearchCommand { get; }

        private void ApplyState(ComponentLookup state)
        {
            IsSearching = state.IsSearching;
            SearchText = state.SearchText;

            if (state.FoundComponents != lastFoundComponents)
            {
                FoundComponents = state.FoundComponents?
                    .Select(x => new ComponentDescriptorViewModel(x, ExecutionEnvironment))
                    .ToList();
                lastFoundComponents = state.FoundComponents;
            }

            SetVisible(state.IsVisible);
        }

        private async void SetVisible(bool value)
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

        private void Cancel()
        {
            this.DispatchAction(Actions.ComponentLookupActionCreator.Hide());
        }

        private void Commit()
        {
            this.DispatchAction(Actions.ComponentLookupActionCreator.Commit());
        }

        public void ChangeSearchText(string value)
        {
            if (value == SearchText) return;
            this.DispatchAction(Actions.ComponentLookupActionCreator.ChangeSearchText(value));
        }

        public async Task<Unit> Search()
        {
            await this.DispatchActionsAsync(Actions.ComponentLookupActionCreator.Search());
            return Unit.Default;
        }
    }
}
