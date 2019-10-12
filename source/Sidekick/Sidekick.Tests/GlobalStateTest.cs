using FluentAssertions;
using Sidekick.Actions;
using Xunit;

namespace Sidekick.Tests
{
    public class GlobalStateTest : TestBase
    {
        [Fact]
        public void GetNextComponentNumber_IncreasesComponentNumber()
        {
            var state = Store.GetState();
            var componentNumber = state.Global.ComponentNumber;
            
            Store.Dispatch(GlobalActionCreator.GetNextComponentNumber());
            state = Store.GetState();
            state.Global.ComponentNumber.Should().Be(componentNumber + 1);
        }

        [Fact]
        public void GetNextFolderNumber_IncreasesFolderNumber()
        {
            var state = Store.GetState();
            var folderNumber = state.Global.FolderNumber;

            Store.Dispatch(GlobalActionCreator.GetNextFolderNumber());
            state = Store.GetState();
            state.Global.FolderNumber.Should().Be(folderNumber + 1);
        }
    }
}
