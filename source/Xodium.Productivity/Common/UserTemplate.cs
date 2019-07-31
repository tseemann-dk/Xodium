namespace Xodium.Productivity.Common
{
    public class UserTemplate : IUser
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string MobilePhone { get; set; }
        public string BusinessPhone { get; set; }
    }
}
