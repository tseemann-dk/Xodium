namespace Xodium.Flow
{
    public abstract class ActionBase<T> : IAction
    {
        public ActionBase(string actionType, T payload = default)
        {
            ActionType = actionType;
            Payload = payload;
        }

        public string ActionType { get; set; }
        public T Payload { get; set; }

        object IAction.Payload => Payload;
    }

    public abstract class ActionBase : ActionBase<object>
    {
        public ActionBase(string actionType) 
            : base(actionType)
        {
        }
    }
}
