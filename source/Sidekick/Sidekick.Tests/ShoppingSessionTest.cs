using System;
using FluentAssertions;
using Redux;
using Sidekick.Features.Shopper.Actions.ShoppingList;
using Sidekick.Features.Shopper.Actions.ShoppingSession;
using Sidekick.Features.Shopper.Models;
using Sidekick.State;
using Xunit;

namespace Sidekick.Tests
{
    public class ShoppingSessionTest
    {
        private readonly Store<AppState> store;

        public ShoppingSessionTest()
        {
            store = new Store<AppState>(AppStateReducer.Execute, AppStateGenerator.GenerateAppState());
        }

        [Fact]
        public void EnterGroup_WhenGroupIsFocused_EntersGroup()
        {
            var state = store.GetState();
            var startGroup = state.ShoppingSession.GetCurrentGroup();
            
            // Add a new group and verify that it is focused
            var newGroup = new ShoppingGroup("G2", "Group 2", 1);
            store.Dispatch(new AddGroupAction(startGroup.Id, newGroup));
            state = store.GetState();
            state.ShoppingSession.FocusedNodeId.Should().Be(newGroup.Id);

            // Enter the new group and verify that it was entered
            store.Dispatch(new EnterGroupAction(newGroup.Id));
            state = store.GetState();
            state.ShoppingSession.GetCurrentGroup().Id.Should().Be(newGroup.Id);
        }

        /*
        [Fact]
        public void EnterGroup_WhenItemIsFocused_StaysPut()
        {
            var state = store.GetState();
            var startGroup = state.ShoppingSession.GetCurrentGroup();

            // Add a new item and verify that it is focused
            var newItem = new ShoppingItem(null, 0);
            store.Dispatch(new AddItemAction(startGroup.Id, newItem));
            state = store.GetState();
            state.ShoppingSession.FocusedNodeId.Should().Be(newItem.Id);

            // Attempt to enter the new item and verify that it was not possible
            store.Dispatch(new EnterGroupAction(newItem.Id)); // TODO: throw
            state = store.GetState();
            state.ShoppingSession.GetCurrentGroup().Id.Should().Be(startGroup.Id);
        }
        */

        [Fact]
        public void ExitGroup_WhenInsideChildGroup_GoesToParentGroupAndFocusesChildGroup()
        {
            var state = store.GetState();
            var startGroup = state.ShoppingSession.GetCurrentGroup();

            // Add a new group
            var newGroup = new ShoppingGroup("G2", "Group 2", 1);
            store.Dispatch(new AddGroupAction(startGroup.Id, newGroup));

            // Enter the new group
            store.Dispatch(new EnterGroupAction(newGroup.Id));
            state = store.GetState();
            var currentGroup = state.ShoppingSession.GetCurrentGroup();
            currentGroup.Id.Should().Be(newGroup.Id);

            // Exit group and verify that we are back at parent
            store.Dispatch(new ExitGroupAction());
            state = store.GetState();
            currentGroup = state.ShoppingSession.GetCurrentGroup();
            currentGroup.Id.Should().Be(startGroup.Id);

            // Verify that the group is focused
            state.ShoppingSession.FocusedNodeId.Should().Be(newGroup.Id);
        }

        [Fact]
        public void ExitGroup_WhenAtRoot_StaysPut()
        {
            var state = store.GetState();
            var startGroup = state.ShoppingSession.GetCurrentGroup();
            
            // Attempt to exit group
            store.Dispatch(new ExitGroupAction());

            // Verify that we have not moved
            state = store.GetState();
            state.ShoppingSession.GetCurrentGroup().Should().Be(startGroup);
        }
    }
}
