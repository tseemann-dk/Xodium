using System.Collections.Generic;
using System.Threading.Tasks;
using Xodium.Services;

namespace Xodium.Platform.Uwp.Services
{
    public class CommunicationService : ICommunicationService
    {
        public Task<bool> StartPhoneCall(string phoneNumber, string displayName)
        {
            if (!global::Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.ApplicationModel.Calls.PhoneCallManager"))
                return Task.FromResult(false);

            global::Windows.ApplicationModel.Calls.PhoneCallManager.ShowPhoneCallUI(phoneNumber, displayName);
            return Task.FromResult(true);
        }

        public Task<bool> StartVideoCall(string phoneNumber, string displayName)
        {
            return Task.FromResult(false);
        }

        public Task<bool> SendTextMessage(string phoneNumber, string displayName, string message)
        {
            return Task.FromResult(false);
        }

        public Task<bool> SendMail(ICollection<string> to, ICollection<string> cc = null, ICollection<string> bcc = null, string subject = null, string body = null)
        {
            return Task.FromResult(false);
        }
    }
}
