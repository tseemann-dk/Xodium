using System;
using System.Linq;
using Xodium.Productivity.Common;
using MSGraph = Microsoft.Graph;

namespace Xodium.Productivity.Microsoft365.Common
{
    public class User : IUser
    {
        private readonly MSGraph.User instance;

        public User(MSGraph.User instance)
        {
            this.instance = instance ?? throw new ArgumentNullException(nameof(instance));
        }

        public string Id => instance.Id;
        public string UserName => instance.UserPrincipalName;
        public string DisplayName => instance.DisplayName;
        public string FirstName => instance.GivenName;
        public string LastName => instance.Surname;
        public string Email => instance.Mail;
        public string MobilePhone => instance.MobilePhone;
        public string BusinessPhone => instance.BusinessPhones?.FirstOrDefault();
    }
}
