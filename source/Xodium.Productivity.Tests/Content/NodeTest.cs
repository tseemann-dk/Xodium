using FluentAssertions;
using System.Linq;
using Xodium.Productivity.Content.Models;
using Xodium.Productivity.Content.Utilities;
using Xunit;

namespace Xodium.Productivity.Tests.Content
{
    public class NodeTest
    {
        private TreeBuilder<SampleContainer> treeBuilder;

        public NodeTest()
        {
            treeBuilder = SampleContainer.CreateTreeBuilder();
        }

        [Fact]
        public void GetParent_WhenNodeIsFirstChild_ReturnsParent()
        {
            var container = treeBuilder.BuildTree("A", 1, 3);
            var child = container.Nodes.First();
            var parent = child.GetParent(container);

            parent.Should().Be(container);
        }

        [Fact]
        public void GetParent_WhenNodeIsLastChild_ReturnsParent()
        {
            var container = treeBuilder.BuildTree("A", 1, 3);
            var child = container.Nodes.Last();
            var parent = child.GetParent(container);

            parent.Should().Be(container);
        }

        [Fact]
        public void GetParent_WhenNodeIsFirstGrandChild_ReturnsParent()
        {
            var container = treeBuilder.BuildTree("A", 2, 3);
            var child = container.GetContainers().First();
            var grandChild = child.Nodes.First();
            var parent = grandChild.GetParent(container);

            parent.Should().Be(child);
        }
    }
}
