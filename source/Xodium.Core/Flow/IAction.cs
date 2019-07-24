namespace Xodium.Flow
{
    public interface IAction
    {
        string ActionType { get; }
        object Payload { get; }
    }
}
