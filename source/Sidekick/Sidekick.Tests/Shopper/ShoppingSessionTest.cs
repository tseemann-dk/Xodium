using System;
using FluentAssertions;
using Sidekick.Shopper.Actions;
using Sidekick.Shopper.Models;
using Xunit;

namespace Sidekick.Tests
{
    public class ShoppingSessionTest : TestBase
    {
        [Fact]
        public void EnterGroup_WhenGroupIsFocused_EntersGroup()
        {
            var state = Store.GetState();
            var startGroup = state.ShoppingSession.GetCurrentGroup();
            
            // Add a new group and verify that it is focused
            var newGroup = new ShoppingGroup("G2", "Group 2", 1);
            Store.Dispatch(ShoppingListActionCreator.AddGroup(startGroup.Id, newGroup));
            state = Store.GetState();
            state.ShoppingSession.FocusedNodeId.Should().Be(newGroup.Id);

            // Enter the new group and verify that it was entered
            Store.Dispatch(ShoppingSessionActionCreator.EnterGroup(newGroup.Id));
            state = Store.GetState();
            state.ShoppingSession.GetCurrentGroup().Id.Should().Be(newGroup.Id);
        }

        [Fact]
        public void EnterGroup_WhenItemIsFocused_Fails()
        {
            var state = Store.GetState();
            var startGroup = state.ShoppingSession.GetCurrentGroup();

            // Add a new item and verify that it is focused
            var newItem = new ShoppingItem(null, 0);
            Store.Dispatch(ShoppingListActionCreator.AddItem(startGroup.Id, newItem));
            state = Store.GetState();
            state.ShoppingSession.FocusedNodeId.Should().Be(newItem.Id);

            // Attempt to enter the new item and verify failure
            Store.Invoking(x => x.Dispatch(ShoppingSessionActionCreator.EnterGroup(newItem.Id)))
                .Should().Throw<InvalidCastException>();
        }
        
        [Fact]
        public void ExitGroup_WhenInsideChildGroup_GoesToParentGroupAndFocusesChildGroup()
        {
            var state = Store.GetState();
            var startGroup = state.ShoppingSession.GetCurrentGroup();

            // Add a new group
            var newGroup = new ShoppingGroup("G2", "Group 2", 1);
            Store.Dispatch(ShoppingListActionCreator.AddGroup(startGroup.Id, newGroup));

            // Enter the new group
            Store.Dispatch(ShoppingSessionActionCreator.EnterGroup(newGroup.Id));
            state = Store.GetState();
            var currentGroup = state.ShoppingSession.GetCurrentGroup();
            currentGroup.Id.Should().Be(newGroup.Id);

            // Exit group and verify that we are back at parent
            Store.Dispatch(ShoppingSessionActionCreator.ExitGroup());
            state = Store.GetState();
            currentGroup = state.ShoppingSession.GetCurrentGroup();
            currentGroup.Id.Should().Be(startGroup.Id);

            // Verify that the group is focused
            state.ShoppingSession.FocusedNodeId.Should().Be(newGroup.Id);
        }

        [Fact]
        public void ExitGroup_WhenAtRoot_StaysPut()
        {
            var state = Store.GetState();
            var startGroup = state.ShoppingSession.GetCurrentGroup();
            
            // Attempt to exit group
            Store.Dispatch(ShoppingSessionActionCreator.ExitGroup());

            // Verify that we have not moved
            state = Store.GetState();
            state.ShoppingSession.GetCurrentGroup().Should().Be(startGroup);
        }
    }
}
