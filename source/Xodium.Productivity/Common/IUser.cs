namespace Xodium.Productivity.Common
{
    public interface IUser
    {
        string Id { get; }
        string UserName { get; }
        string DisplayName { get; }
        string FirstName { get; }
        string LastName { get; }
        string Email { get; }
        string MobilePhone { get; }
        string BusinessPhone { get; }
    }
}
