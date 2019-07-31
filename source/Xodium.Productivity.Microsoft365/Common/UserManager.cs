using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xodium.Productivity.Common;
using MSGraph = Microsoft.Graph;

namespace Xodium.Productivity.Microsoft365.Common
{
    public class UserManager : IUserManager
    {
        private readonly MSGraph.IGraphServiceClient graphClient;

        public UserManager(MSGraph.IGraphServiceClient graphClient)
        {
            this.graphClient = graphClient ?? throw new ArgumentNullException(nameof(graphClient));
        }

        private IUser Map(MSGraph.User value) => new User(value);

        public async Task<IUser> GetUser(string id, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Map(await graphClient.Users[id].Request().GetAsync());
        }

        public async Task<IReadOnlyList<IUser>> GetUsers(CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = new List<IUser>();
            var request = graphClient.Users.Request();

            while (request != null)
            {
                var page = await request.GetAsync(cancellationToken);
                result.AddRange(page.Select(Map));
                request = page.NextPageRequest;
            };

            return result;
        }
    }
}
