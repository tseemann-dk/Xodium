using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sidekick.Shopper.Models;

namespace Sidekick.Tests.TestDoubles
{
    class ShopFake : IShop
    {
        private const string shopId = "mocked";
        private readonly List<ComponentDescriptor> components = CreateComponents();

        public ShopIdentity Identity { get; } = new ShopIdentity(shopId);

        public Task<IReadOnlyList<IComponentDescriptor>> FindComponents(string searchText)
        {
            var matches = components
                .Where(x => x.Text.Contains(searchText))
                .ToList();

            return Task.FromResult<IReadOnlyList<IComponentDescriptor>>(matches);
        }

        private static List<ComponentDescriptor> CreateComponents()
        {
            return Enumerable.Range(1, 10)
                .Select(i => ComponentDescriptor.Create(shopId, $"C{i}", $"Component {i}", i * 10))
                .ToList();
        }
    }
}
