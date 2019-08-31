namespace Sidekick.State
{
    public class GlobalState
    {
        public GlobalState()
            : this(0, 0)
        {
        }

        public GlobalState(int componentNumber, int groupNumber)
        {
            ComponentNumber = componentNumber;
            GroupNumber = groupNumber;
        }

        public int ComponentNumber { get; }
        public int GroupNumber { get; }

        public GlobalState WithNextComponentNumber() => new GlobalState(ComponentNumber + 1, GroupNumber);
        public GlobalState WithNextGroupNumber() => new GlobalState(ComponentNumber, GroupNumber + 1);
    }
}
