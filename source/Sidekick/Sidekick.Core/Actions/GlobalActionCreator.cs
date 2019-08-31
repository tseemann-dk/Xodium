using Sidekick.Actions.Global;
using Xodium.Flow;

namespace Sidekick.Actions
{
    public static class GlobalActionCreator
    {
        public static IAction GetNextComponentNumber() => new GetNextComponentNumberAction();
        public static IAction GetNextGroupNumber() => new GetNextGroupNumberAction();
    }
}
