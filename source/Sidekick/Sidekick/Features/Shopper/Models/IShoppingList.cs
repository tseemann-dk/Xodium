using System.Collections.Generic;
using Xodium.Productivity.Content.Models;

namespace Sidekick.Features.Shopper.Models
{
    public interface IShoppingList : IDocument
    {
        new IShoppingGroup Content { get; }
        IReadOnlyList<IComponent> Components { get; }

        IShoppingList AddComponent(IComponent component);
        IShoppingList RemoveComponent(IComponent component);
        IShoppingList WithContent(IShoppingGroup content);
        IShoppingList WithComponents(IReadOnlyList<IComponent> components);
    }

    public static class ShoppingListExtensions
    {
        public static ShoppingList WithTitle(this IShoppingList self, string text)
            => new ShoppingList(self.Id, self.Name, self.Content.WithTitle(text), self.Components);
    }
}
