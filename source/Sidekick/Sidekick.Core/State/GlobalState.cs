namespace Sidekick.State
{
    public class GlobalState
    {
        public GlobalState()
            : this(0, 0)
        {
        }

        public GlobalState(int componentNumber, int folderNumber)
        {
            ComponentNumber = componentNumber;
            FolderNumber = folderNumber;
        }

        public int ComponentNumber { get; }
        public int FolderNumber { get; }

        public GlobalState WithNextComponentNumber() => new GlobalState(ComponentNumber + 1, FolderNumber);
        public GlobalState WithNextFolderNumber() => new GlobalState(ComponentNumber, FolderNumber + 1);
    }
}
