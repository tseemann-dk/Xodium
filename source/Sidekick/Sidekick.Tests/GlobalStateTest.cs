using FluentAssertions;
using Sidekick.Actions;
using Sidekick.Reducers;
using Sidekick.State;
using Xodium.Flow;
using Xodium.Redux;
using Xunit;

namespace Sidekick.Tests
{
    public class GlobalStateTest
    {
        private readonly IStore<AppState> store;

        public GlobalStateTest()
        {
            store = new ReduxStore<AppState>(
                r => new Redux.Store<AppState>(r, AppStateGenerator.GenerateDefaultState()), 
                AppStateReducer.Reduce
            );
        }

        [Fact]
        public void GetNextComponentNumber_IncreasesComponentNumber()
        {
            var state = store.GetState();
            var componentNumber = state.Global.ComponentNumber;
            
            store.Dispatch(GlobalActionCreator.GetNextComponentNumber());
            state = store.GetState();
            state.Global.ComponentNumber.Should().Be(componentNumber + 1);
        }

        [Fact]
        public void GetNextGroupNumber_IncreasesGroupNumber()
        {
            var state = store.GetState();
            var groupNumber = state.Global.GroupNumber;

            store.Dispatch(GlobalActionCreator.GetNextGroupNumber());
            state = store.GetState();
            state.Global.GroupNumber.Should().Be(groupNumber + 1);
        }
    }
}
