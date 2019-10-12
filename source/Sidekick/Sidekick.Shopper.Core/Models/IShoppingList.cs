using System.Collections.Generic;
using Xodium.DataStructures;

namespace Sidekick.Shopper.Models
{
    public interface IShoppingList : IDocument
    {
        new IShoppingFolder Content { get; }
        IReadOnlyList<IComponent> Components { get; }

        IShoppingList AddComponent(IComponent component);
        IShoppingList RemoveComponent(IComponent component);
        IShoppingList WithContent(IShoppingFolder content);
        IShoppingList WithComponents(IReadOnlyList<IComponent> components);
    }
}
