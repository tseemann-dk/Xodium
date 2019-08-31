using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Sidekick.Shopper.Actions;
using Sidekick.Shopper.Models;
using Sidekick.Shopper.State;
using Sidekick.Tests.TestDoubles;
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
        public async Task Search_WhenSearchTextIsValid_FindsComponents()
        {
            Store.Dispatch(ComponentLookupActionCreator.SetSearchText("Component 1"));
            await Store.DispatchAsync(ComponentLookupActionCreator.Search(shop));
            ComponentLookup.FoundComponents.Should().NotBeEmpty();
        }

        [Fact]
        public async Task Search_WhenSearchTextIsInvalid_ShowsErrorMessage()
        {
            Store.Dispatch(ComponentLookupActionCreator.SetSearchText("1"));
            await Store.DispatchAsync(ComponentLookupActionCreator.Search(shop));
            ComponentLookup.SearchError.Should().NotBeNull();
        }

        //[Fact]
        //public void PickComponent_WhenNoComponentIsSelected_Fails()
        //{
        //    Store.Dispatch(ComponentLookupActionCreator.PickComponent());
        //}

        [Fact]
        public async Task PickComponent_WhenComponentIsSelected_PicksComponent()
        {
            var group = ShoppingSession.GetCurrentGroup();
            var nodeCount = group.Nodes.Count;

            // Search 
            Store.Dispatch(ComponentLookupActionCreator.ShowLookup());
            Store.Dispatch(ComponentLookupActionCreator.SetSearchText("Component 5"));
            await Store.DispatchAsync(ComponentLookupActionCreator.Search(shop));

            // Select first match
            var matches = ComponentLookup.FoundComponents;
            matches.Should().NotBeNullOrEmpty();
            var component = matches.First();
            Store.Dispatch(ComponentLookupActionCreator.SelectComponent(component.Reference.ComponentNumber));

            // Pick component
            Store.Dispatch(ComponentLookupActionCreator.PickComponent());

            // Verify node was added
            group = ShoppingSession.GetCurrentGroup();
            group.Nodes.Count.Should().Be(nodeCount + 1);
            var node = group.Nodes.First();
            node.Should().BeOfType<ShoppingItem>();

            // Verify item has correct component
            var item = node as IShoppingItem;
            item.ComponentNumber.Should().Be(component.Reference.ComponentNumber);
        }
    }
}
