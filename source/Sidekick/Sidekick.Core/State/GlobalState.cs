namespace Sidekick.State
{
    public class GlobalState
    {
        public GlobalState()
            : this(1, 1)
        {
        }

        public GlobalState(int nextComponentNumber, int nextGroupNumber)
        {
            NextComponentNumber = nextComponentNumber;
            NextGroupNumber = nextGroupNumber;
        }

        public int NextComponentNumber { get; }
        public int NextGroupNumber { get; }

        public GlobalState WithNextComponentNumber() => new GlobalState(NextComponentNumber + 1, NextGroupNumber);
        public GlobalState WithNextGroupNumber() => new GlobalState(NextComponentNumber, NextGroupNumber + 1);
    }
}
