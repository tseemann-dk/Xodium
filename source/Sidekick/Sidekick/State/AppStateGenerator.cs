using System.Linq;
using Sidekick.Features.Shopper.Models;

namespace Sidekick.State
{
    public static class AppStateGenerator
    {
        public static AppState GenerateAppState()
        {
            var shoppingList = BuildSampleShoppingList();
            var content = shoppingList.Content;

            return new AppState(
                new GlobalState(3, 2),
                new ShoppingSession(
                    shoppingList,
                    content.Id,
                    content.Nodes.Last().Id
                )
            );
        }

        private static ShoppingList BuildSampleShoppingList()
        {
            var components = new[]
            {
                new Component("1", "Component 1", 10),
                new Component("2", "Component 2", 20)
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
