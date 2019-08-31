using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sidekick.Shopper.Models;

namespace Sidekick.Shopper.eBay
{
    public class eBayShop : IShop
    {
        private readonly List<ComponentDescriptor> components;

        public eBayShop()
        {
            components = CreateComponents();
        }

        public ShopIdentity Identity { get; } = new ShopIdentity("eBay");

        public async Task<IReadOnlyList<IComponentDescriptor>> FindComponents(string searchText)
        {
            var matches = components
                .Where(x => x.Text.Contains(searchText))
                .ToList();

            await Task.Delay(2000);
            return matches;
        }

        private List<ComponentDescriptor> CreateComponents()
        {
            return Enumerable
                .Range(1, 100)
                .Select(x => ComponentDescriptor.Create(Identity, $"EB-{x}", $"eBay Component {x}", x + .99))
                .ToList();
        }
    }
}
