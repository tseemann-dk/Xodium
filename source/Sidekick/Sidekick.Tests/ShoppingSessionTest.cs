using System;
using FluentAssertions;
using Sidekick.Reducers;
using Sidekick.Shopper.Actions;
using Sidekick.Shopper.Models;
using Sidekick.State;
using Xodium.Flow;
using Xodium.Redux;
using Xunit;

namespace Sidekick.Tests
{
    public class ShoppingSessionTest
    {
        private readonly IStore<AppState> store;

        public ShoppingSessionTest()
        {
            store = new ReduxStore<AppState>(
                r => new Redux.Store<AppState>(r, AppStateGenerator.GenerateDefaultState()), 
                AppStateReducer.Reduce
            );
        }

        [Fact]
        public void EnterGroup_WhenGroupIsFocused_EntersGroup()
        {
            var state = store.GetState();
            var startGroup = state.ShoppingSession.GetCurrentGroup();
            
            // Add a new group and verify that it is focused
            var newGroup = new ShoppingGroup("G2", "Group 2", 1);
            store.Dispatch(ShoppingListActionCreator.AddGroup(startGroup.Id, newGroup));
            state = store.GetState();
            state.ShoppingSession.FocusedNodeId.Should().Be(newGroup.Id);

            // Enter the new group and verify that it was entered
            store.Dispatch(ShoppingSessionActionCreator.EnterGroup(newGroup.Id));
            state = store.GetState();
            state.ShoppingSession.GetCurrentGroup().Id.Should().Be(newGroup.Id);
        }

        [Fact]
        public void EnterGroup_WhenItemIsFocused_Fails()
        {
            var state = store.GetState();
            var startGroup = state.ShoppingSession.GetCurrentGroup();

            // Add a new item and verify that it is focused
            var newItem = new ShoppingItem(null, 0);
            store.Dispatch(ShoppingListActionCreator.AddItem(startGroup.Id, newItem));
            state = store.GetState();
            state.ShoppingSession.FocusedNodeId.Should().Be(newItem.Id);

            // Attempt to enter the new item and verify failure
            store.Invoking(x => x.Dispatch(ShoppingSessionActionCreator.EnterGroup(newItem.Id)))
                .Should().Throw<InvalidCastException>();
        }
        
        [Fact]
        public void ExitGroup_WhenInsideChildGroup_GoesToParentGroupAndFocusesChildGroup()
        {
            var state = store.GetState();
            var startGroup = state.ShoppingSession.GetCurrentGroup();

            // Add a new group
            var newGroup = new ShoppingGroup("G2", "Group 2", 1);
            store.Dispatch(ShoppingListActionCreator.AddGroup(startGroup.Id, newGroup));

            // Enter the new group
            store.Dispatch(ShoppingSessionActionCreator.EnterGroup(newGroup.Id));
            state = store.GetState();
            var currentGroup = state.ShoppingSession.GetCurrentGroup();
            currentGroup.Id.Should().Be(newGroup.Id);

            // Exit group and verify that we are back at parent
            store.Dispatch(ShoppingSessionActionCreator.ExitGroup());
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
            store.Dispatch(ShoppingSessionActionCreator.ExitGroup());

            // Verify that we have not moved
            state = store.GetState();
            state.ShoppingSession.GetCurrentGroup().Should().Be(startGroup);
        }
    }
}
