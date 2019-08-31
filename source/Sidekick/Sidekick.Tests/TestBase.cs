using Sidekick.Reducers;
using Sidekick.State;
using Xodium.Flow;
using Xodium.Redux;

namespace Sidekick.Tests
{
    public abstract class TestBase
    {
        public TestBase()
        {
            Store = new ReduxStore<AppState>(
                r => new Redux.Store<AppState>(r, AppStateGenerator.GenerateDefaultState()), 
                AppStateReducer.Reduce
            );
        }
        
        protected IStore<AppState> Store { get; }
    }
}
