using Redux;
using Sidekick.Models;

namespace Sidekick.Reducers
{
    public class AppStateReducer
    {
        public static AppState Execute(AppState state, object action)
        {
            return new AppState
            {
                Global = GlobalStateReducer.Execute(state.Global, action),
                CurrentArchive = ArchiveStateReducer.Execute(state.CurrentArchive, action)
            };
        }
    }
}
