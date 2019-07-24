using System;
using System.Collections.Generic;
using System.Threading.Tasks;
//using XLabs.Platform.Services;

namespace Xodium.Platform.Xamarin.Services
{
    /*
    public class CommunicationService : ICommunicationService
    {
        private readonly IPhoneService phoneService;
        private static readonly Task<bool> CompletedTask = Task.FromResult(true);

        public CommunicationService(IPhoneService phoneService)
        {
            this.phoneService = phoneService;
        }

        public Task<bool> StartPhoneCall(string phoneNumber, string displayName)
        {
            phoneNumber = phoneNumber.Replace(" ", "");
            phoneService.DialNumber(phoneNumber);
            return CompletedTask;
        }

        public Task<bool> StartVideoCall(string phoneNumber, string displayName)
        {
            return Task.FromResult(false);
        }

        public Task<bool> SendTextMessage(string phoneNumber, string displayName, string message)
        {
            phoneService.SendSMS(phoneNumber, message ?? "");
            return CompletedTask;
        }

        public Task<bool> SendMail(ICollection<string> to, ICollection<string> cc, ICollection<string> bcc, string subject, string body)
        {
            Device.OpenUri(new Uri($"mailto:{string.Join(";", to)}"));
            return CompletedTask;
        }
    }
    */
}
