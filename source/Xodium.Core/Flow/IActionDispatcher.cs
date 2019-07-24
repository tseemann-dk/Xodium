namespace Xodium.Flow
{
    public interface IActionDispatcher
    {
        void Dispatch(IAction action);
    }
}
