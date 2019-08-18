namespace Sidekick.State
{
    public struct GlobalState
    {
        public GlobalState(int nextComponentNumber, int nextGroupNumber)
        {
            NextComponentNumber = nextComponentNumber;
            NextGroupNumber = nextGroupNumber;
        }

        public int NextComponentNumber;
        public int NextGroupNumber;

        public GlobalState WithNextComponentNumber() => new GlobalState(NextComponentNumber + 1, NextGroupNumber);
        public GlobalState WithNextGroupNumber() => new GlobalState(NextComponentNumber, NextGroupNumber + 1);
    }
}
