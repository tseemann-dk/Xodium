using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Sidekick.Shopper.Actions;
using Sidekick.Shopper.Models;
using Sidekick.Shopper.State;
using Sidekick.Tests.TestDoubles;
using Xodium.Productivity.Content.Models;
using Xunit;

namespace Sidekick.Tests
{
    public class ComponentLookupTest : TestBase
    {
        private readonly IShop shop = new ShopFake();

        protected ShoppingSession ShoppingSession => Store.GetState().ShoppingSession;
        protected ShoppingList ShoppingList => ShoppingSession.ShoppingList;
        protected ComponentLookup ComponentLookup => ShoppingSession.ComponentLookup;

        [Fact]
        public void ShowLookup_ShowsLookup()
        {
            ComponentLookup.IsVisible.Should().BeFalse();
            Store.Dispatch(ComponentLookupActionCreator.ShowLookup());
            ComponentLookup.IsVisible.Should().BeTrue();
        }

        [Fact]
        public void HideLookup_HidesLookup()
        {
            Store.Dispatch(ComponentLookupActionCreator.ShowLookup());
            ComponentLookup.IsVisible.Should().BeTrue();
            Store.Dispatch(ComponentLookupActionCreator.HideLookup());
            ComponentLookup.IsVisible.Should().BeFalse();
        }

        [Fact]
        public void SetSearchText_ChangesSearchText()
        {
            ComponentLookup.SearchText.Should().BeNull();
            Store.Dispatch(ComponentLookupActionCreator.SetSearchText("123"));
            ComponentLookup.SearchText.Should().Be("123");
        }

        [Fact]
        public async Task Search_WhenSearchTextIsNull_ShowsErrorMessage()
        {
            Store.Dispatch(ComponentLookupActionCreator.SetSearchText(null));
            await Store.DispatchAsync(ComponentLookupActionCreator.Search(shop));
            ComponentLookup.SearchError.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Search_WhenSearchTextIsEmpty_ShowsErrorMessage()
        {
            Store.Dispatch(ComponentLookupActionCreator.SetSearchText(""));
            await Store.DispatchAsync(ComponentLookupActionCreator.Search(shop));
            ComponentLookup.SearchError.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Search_WhenSearchTextIsTooShort_ShowsErrorMessage()
        {
            Store.Dispatch(ComponentLookupActionCreator.SetSearchText("x"));
            await Store.DispatchAsync(ComponentLookupActionCreator.Search(shop));
            ComponentLookup.SearchError.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Search_WhenSearchTextMatchesOneComponent_FindsComponent()
        {
            Store.Dispatch(ComponentLookupActionCreator.SetSearchText("Component 2"));
            await Store.DispatchAsync(ComponentLookupActionCreator.Search(shop));
            ComponentLookup.FoundComponents.Should().HaveCount(1);
        }

        [Fact]
        public async Task Search_WhenSearchTextMatchesMultipleComponent_FindsComponents()
        {
            Store.Dispatch(ComponentLookupActionCreator.SetSearchText("Component 1"));
            await Store.DispatchAsync(ComponentLookupActionCreator.Search(shop));
            ComponentLookup.FoundComponents.Should().HaveCount(2);
            ComponentLookup.FoundComponents.Select(x => x.Text).Should().BeEquivalentTo("Component 1", "Component 10");
        }

        [Fact]
        public void PickComponent_WhenNoComponentIsSelected_KeepsLookupOpen()
        {
            Store.Dispatch(ComponentLookupActionCreator.ShowLookup());
            Store.Dispatch(ComponentLookupActionCreator.PickComponent());
            ComponentLookup.IsVisible.Should().BeTrue();
        }

        [Fact]
        public async Task PickComponent_WhenComponentIsSelected_PicksComponentAndClosesLookup()
        {
            var folder = ShoppingSession.GetCurrentFolder();
            var nodeCount = folder.Nodes.Count;
            var focusedNode = folder.Nodes.First(x => x.Id == ShoppingSession.FocusedNodeId);

            // Show lookup
            Store.Dispatch(ComponentLookupActionCreator.ShowLookup());
            ComponentLookup.IsVisible.Should().BeTrue();

            // Perform search
            Store.Dispatch(ComponentLookupActionCreator.SetSearchText("Component 5"));
            await Store.DispatchAsync(ComponentLookupActionCreator.Search(shop));

            // Select first match
            var matches = ComponentLookup.FoundComponents;
            matches.Should().NotBeNullOrEmpty();
            var component = matches.First();
            Store.Dispatch(ComponentLookupActionCreator.SelectComponent(component.Reference.ComponentNumber));

            // Pick component
            Store.Dispatch(ComponentLookupActionCreator.PickComponent());

            // Verify that new item was added after focused node
            folder = ShoppingSession.GetCurrentFolder();
            folder.Nodes.Count.Should().Be(nodeCount + 1);
            var node = folder.GetNextSibling(focusedNode);
            node.Should().BeOfType<ShoppingItem>();

            // Verify that item has correct component
            var item = node as IShoppingItem;
            item.ComponentNumber.Should().Be(component.Reference.ComponentNumber);

            // Verify lookup is hidden
            ComponentLookup.IsVisible.Should().BeFalse();
        }
    }
}
