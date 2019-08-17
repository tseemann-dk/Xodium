using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Xodium.Productivity.Content.Models;

namespace Sidekick.Features.Shopper.Models
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class ShoppingList : DocumentBase, IShoppingList
    {
        private readonly List<IComponent> components;

        public ShoppingList(string id, string name, IShoppingGroup root, IEnumerable<IComponent> components = null)
            : base(id, name, root)
        {
            this.components = components?.ToList() ?? new List<IComponent>();
        }

        public new IShoppingGroup Content => base.Content as IShoppingGroup;
        public IReadOnlyList<IComponent> Components => components;

        [ExcludeFromCodeCoverage]
        private string DebuggerDisplay => $"{Name}; {Content.Title}";

        public IShoppingList AddComponent(IComponent component)
        {
            var list = Components.ToList();
            list.Add(component);
            return WithComponents(list);
        }

        public IShoppingList RemoveComponent(IComponent component)
        {
            var list = Components.ToList();
            list.Remove(component);
            return WithComponents(list);
        }

        public IShoppingList WithContent(IShoppingGroup content) => new ShoppingList(Id, Name, content, Components);
        public IShoppingList WithComponents(IEnumerable<IComponent> components) => new ShoppingList(Id, Name, Content, components);

        public override IDocument WithContent(IContainer content) => WithContent(content as IShoppingGroup);
    }
}
