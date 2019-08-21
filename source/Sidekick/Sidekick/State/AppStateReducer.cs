namespace Sidekick.State
{
    public static class AppStateReducer
    {
        public static AppState Execute(AppState state, object action)
        {
            foreach (var reducer in StoreConfiguration.Reducers)
            {
                state = reducer(state, action);
            }

            return state;
        }
    }
}
