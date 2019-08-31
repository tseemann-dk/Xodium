using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sidekick.Shopper.Models
{
    public interface IShop
    {
        ShopIdentity Identity { get; }

        Task<IReadOnlyList<IComponentDescriptor>> FindComponents(string searchText);
    }
}
