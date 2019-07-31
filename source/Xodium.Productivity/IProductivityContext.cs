using Xodium.Productivity.Common;
using Xodium.Productivity.Scheduling;

namespace Xodium.Productivity
{
    public interface IProductivityContext
    {
        IUserManager UserManager { get; }

        IScheduleManager GetScheduleManager();
        IScheduleManager GetScheduleManagerForUser(IUser user);
    }
}
