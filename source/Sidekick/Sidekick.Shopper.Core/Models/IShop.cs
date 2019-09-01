using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sidekick.Shopper.Models
{
    public interface IShop
    {
        ShopIdentity ShopIdentity { get; }

        Task<IReadOnlyList<IComponentDescriptor>> FindComponents(string searchText);
    }
}
