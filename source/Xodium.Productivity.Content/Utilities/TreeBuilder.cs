using System;
using System.Collections.Generic;
using System.Linq;
using Xodium.Productivity.Content.Models;

namespace Xodium.Productivity.Content.Utilities
{
    public delegate T ContainerCreator<T>(string id, IEnumerable<INode> nodes) where T : IContainer;
    public delegate string IdentityProvider(string parentId, int index);
    public delegate IEnumerable<INode> NodesProvider();

    public class TreeBuilder<TContainer>
        where TContainer : class, IContainer
    {
        private readonly ContainerCreator<TContainer> containerCreator;
        private readonly IdentityProvider identityProvider;

        public TreeBuilder(ContainerCreator<TContainer> containerCreator, IdentityProvider identityProvider = null)
        {
            this.containerCreator = containerCreator ?? throw new ArgumentNullException(nameof(containerCreator));
            this.identityProvider = identityProvider ?? ((id, index) => $"{id}.{index}");
        }

        public TContainer CreateContainer(string id, IEnumerable<INode> nodes = null) => containerCreator(id, nodes);

        public TContainer BuildTree(string id, int depth, int width, NodesProvider getLeaves = null)
        { 
            return CreateContainer(id, depth > 0
                ? Enumerable.Range(1, width).Select(x => BuildTree($"{id}.{x}", depth - 1, width, getLeaves))
                : getLeaves?.Invoke());
        }

        public TContainer BuildTreeViaEvolution(string id, int depth, int width, Func<IEnumerable<INode>> getLeaves = null)
        {
            IContainer container = CreateContainer(id);

            if (depth > 0)
            {
                foreach (var x in Enumerable.Range(1, width))
                {
                    container = container.AddNode(
                        BuildTreeViaEvolution(
                            GetContainerId(id, x), depth - 1, width, getLeaves));
                }
            }
            else
            {
                var leaves = getLeaves?.Invoke();

                if (leaves != null)
                {
                    container = container.AddNodes(leaves);
                }
            }

            return container as TContainer;
        }

        public string GetContainerId(string id, int index) => identityProvider(id, index);
    }
}
