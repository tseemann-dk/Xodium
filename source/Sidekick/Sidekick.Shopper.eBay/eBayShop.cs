using System.Collections.Generic;
using System.Threading.Tasks;
using Sidekick.Shopper.Models;

namespace Sidekick.Shopper.eBay
{
    public class eBayShop : IShop
    {
        public ShopIdentity Identity { get; } = new ShopIdentity("eBay");

        public Task<IReadOnlyList<IComponentDescriptor>> FindComponents(string searchText)
        {
            var components = new List<IComponentDescriptor> 
            {
                new ComponentDescriptor(new ComponentReference(Identity, "EB-0001"), "eBay Component 1", 1.99),
                new ComponentDescriptor(new ComponentReference(Identity, "EB-0002"), "eBay Component 2", 2.99),
                new ComponentDescriptor(new ComponentReference(Identity, "EB-0003"), "eBay Component 3", 3.99)
            };

            return Task.FromResult<IReadOnlyList<IComponentDescriptor>>(components);
        }
    }
}
