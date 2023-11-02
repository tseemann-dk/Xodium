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
        private readonly MSGraph.GraphServiceClient graphClient;

        public UserManager(MSGraph.GraphServiceClient graphClient)
        {
            this.graphClient = graphClient ?? throw new ArgumentNullException(nameof(graphClient));
        }

        private IUser Map(MSGraph.Models.User value) => new User(value);

        public async Task<IUser> GetUser(string id, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Map(await graphClient.Users[id].GetAsync());
        }

        public async Task<IReadOnlyList<IUser>> GetUsers(CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = new List<IUser>();
            var request = graphClient.Users;

            while (request != null)
            {
                var page = await request.GetAsync(_ => { }, cancellationToken);
                result.AddRange(page.Value.Select(Map));
                request = null; // page.NextPageRequest;
            };

            return result;
        }
    }
}
