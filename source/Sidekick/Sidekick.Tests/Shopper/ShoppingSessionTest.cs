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
        public void EnterFolder_WhenFolderIsFocused_EntersFolder()
        {
            var state = Store.GetState();
            var startFolder = state.ShoppingSession.GetCurrentFolder();
            
            // Add a new folder and verify that it is focused
            var newFolder = new ShoppingFolder("F2", "Folder 2", 1);
            Store.Dispatch(ShoppingListActionCreator.AddFolder(startFolder.Id, newFolder));
            state = Store.GetState();
            state.ShoppingSession.FocusedNodeId.Should().Be(newFolder.Id);

            // Enter the new folder and verify that it was entered
            Store.Dispatch(ShoppingSessionActionCreator.EnterFolder(newFolder.Id));
            state = Store.GetState();
            state.ShoppingSession.GetCurrentFolder().Id.Should().Be(newFolder.Id);
        }

        [Fact]
        public void EnterFolder_WhenItemIsFocused_Fails()
        {
            var state = Store.GetState();
            var startFolder = state.ShoppingSession.GetCurrentFolder();

            // Add a new item and verify that it is focused
            var newItem = new ShoppingItem(null, 0);
            Store.Dispatch(ShoppingListActionCreator.AddItem(startFolder.Id, newItem));
            state = Store.GetState();
            state.ShoppingSession.FocusedNodeId.Should().Be(newItem.Id);

            // Attempt to enter the new item and verify failure
            Store.Invoking(x => x.Dispatch(ShoppingSessionActionCreator.EnterFolder(newItem.Id)))
                .Should().Throw<InvalidCastException>();
        }
        
        [Fact]
        public void ExitFolder_WhenInsideChildFolder_GoesToParentFolderAndFocusesChildFolder()
        {
            var state = Store.GetState();
            var startFolder = state.ShoppingSession.GetCurrentFolder();

            // Add a new folder
            var newFolder = new ShoppingFolder("F2", "Folder 2", 1);
            Store.Dispatch(ShoppingListActionCreator.AddFolder(startFolder.Id, newFolder));

            // Enter the new folder
            Store.Dispatch(ShoppingSessionActionCreator.EnterFolder(newFolder.Id));
            state = Store.GetState();
            var currentFolder = state.ShoppingSession.GetCurrentFolder();
            currentFolder.Id.Should().Be(newFolder.Id);

            // Exit folder and verify that we are back at parent
            Store.Dispatch(ShoppingSessionActionCreator.ExitFolder());
            state = Store.GetState();
            currentFolder = state.ShoppingSession.GetCurrentFolder();
            currentFolder.Id.Should().Be(startFolder.Id);

            // Verify that the folder is focused
            state.ShoppingSession.FocusedNodeId.Should().Be(newFolder.Id);
        }

        [Fact]
        public void ExitFolder_WhenAtRoot_StaysPut()
        {
            var state = Store.GetState();
            var startFolder = state.ShoppingSession.GetCurrentFolder();
            
            // Attempt to exit folder
            Store.Dispatch(ShoppingSessionActionCreator.ExitFolder());

            // Verify that we have not moved
            state = Store.GetState();
            state.ShoppingSession.GetCurrentFolder().Should().Be(startFolder);
        }
    }
}
