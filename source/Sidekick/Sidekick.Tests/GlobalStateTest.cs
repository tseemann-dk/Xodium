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
        public void GetNextGroupNumber_IncreasesGroupNumber()
        {
            var state = Store.GetState();
            var groupNumber = state.Global.GroupNumber;

            Store.Dispatch(GlobalActionCreator.GetNextGroupNumber());
            state = Store.GetState();
            state.Global.GroupNumber.Should().Be(groupNumber + 1);
        }
    }
}
