using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Xodium.Productivity.Common
{
    public interface IUserManager
    {
        Task<IUser> GetUser(string id, CancellationToken cancellationToken = default(CancellationToken));
        Task<IReadOnlyList<IUser>> GetUsers(CancellationToken cancellationToken = default(CancellationToken));
    }
}
