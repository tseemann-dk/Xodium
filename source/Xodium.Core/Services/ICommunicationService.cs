using System.Collections.Generic;
using System.Threading.Tasks;

namespace Xodium.Services
{
    public interface ICommunicationService
    {
        Task<bool> StartPhoneCall(string phoneNumber, string displayName);
        Task<bool> StartVideoCall(string phoneNumber, string displayName);
        Task<bool> SendTextMessage(string phoneNumber, string displayName, string message);
        Task<bool> SendMail(ICollection<string> to, ICollection<string> cc = null, ICollection<string> bcc = null, string subject = null, string body = null);
    }
}
