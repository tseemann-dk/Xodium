using Microsoft.Graph;
using Xodium.Productivity.Common;
using Xodium.Productivity.Microsoft365.Common;
using Xodium.Productivity.Outlook.Scheduling;
using Xodium.Productivity.Scheduling;

namespace Xodium.Productivity.Microsoft365
{
    public class MicrosoftGraphProductivityContext : IProductivityContext
    {
        private readonly IGraphServiceClient graphClient;
        private readonly MicrosoftGraphOptions options;

        public MicrosoftGraphProductivityContext(IGraphServiceClient graphClient, MicrosoftGraphOptions options = null)
        {
            this.graphClient = graphClient ?? throw new System.ArgumentNullException(nameof(graphClient));
            this.options = options ?? MicrosoftGraphOptions.Empty;
            UserManager = new UserManager(graphClient);
        }

        public IUserManager UserManager { get; }

        public IScheduleManager GetScheduleManager()
        {
            return new ScheduleManager(graphClient.Me, options.AppointmentCustomPropertyNames);
        }

        public IScheduleManager GetScheduleManagerForUser(IUser user)
        {
            return new ScheduleManager(graphClient.Users[user.Id], options.AppointmentCustomPropertyNames);
        }
    }
}
