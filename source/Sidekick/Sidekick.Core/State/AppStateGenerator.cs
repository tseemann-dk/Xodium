using System.Linq;
using Sidekick.Shopper.Models;
using Sidekick.Shopper.State;

namespace Sidekick.State
{
    public static class AppStateGenerator
    {
        public static AppState GenerateSampleState()
        {
            return new AppState(
                new GlobalState(2, 1),
                BuildShoppingSession(BuildSampleShoppingList())
            );
        }

        public static AppState GenerateDefaultState()
        {
            return new AppState(
                new GlobalState(),
                BuildShoppingSession(BuildEmptyShoppingList())
            );
        }

        private static ShoppingSession BuildShoppingSession(ShoppingList shoppingList)
        {
            var content = shoppingList.Content;

            return new ShoppingSession(
                shoppingList,
                content.Id,
                content.Nodes.FirstOrDefault()?.Id
            );
        }

        private static ShoppingList BuildEmptyShoppingList()
        {
            return new ShoppingList("list-1", "L1",
                new ShoppingGroup("group-1", "G1", "Group 1", 1)
            );
        }

        private static ShoppingList BuildSampleShoppingList()
        {
            var components = new[]
            {
                new Component(ShopIdentity.Internal, "1", "Component 1", 10),
                new Component(ShopIdentity.Internal, "2", "Component 2", 20)
            };

            return new ShoppingList("list-1", "L1",
                new ShoppingGroup("group-1", "G1", "Group 1", 1, new[]
                {
                    new ShoppingItem(components[0], 1),
                    new ShoppingItem(components[1], 1)
                }),
                components
            );
        }
    }
}
