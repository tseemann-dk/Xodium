using Sidekick.Actions.Global;
using Xodium.Flow;

namespace Sidekick.Actions
{
    public static class GlobalActionCreator
    {
        public static IAction GetNextComponentNumber() => new GetNextComponentNumberAction();
        public static IAction GetNextFolderNumber() => new GetNextFolderNumberAction();
    }
}
