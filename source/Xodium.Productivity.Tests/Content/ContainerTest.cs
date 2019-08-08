using FluentAssertions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Xodium.Productivity.Content.Models;
using Xodium.Productivity.Content.Utilities;
using Xunit;

namespace Xodium.Productivity.Tests.Content
{
    [ExcludeFromCodeCoverage]
    public partial class ContainerTest
    {
        private readonly TreeBuilder<SampleContainer> treeBuilder;

        public ContainerTest()
        {
            treeBuilder = SampleContainer.CreateTreeBuilder();
        }

        [Fact]
        public void CreateContainer_WithNoContent_ShouldCreateEmptyContainer()
        {
            var container = treeBuilder.CreateContainer("A");

            container.Should().NotBeNull();
            container.Id.Should().Be("A");
            container.Nodes.Should().BeEmpty();
        }

        [Fact]
        public void CreateContainer_WithAddedContainer_ShouldCreateContainerAndSubContainer()
        {
            var container = treeBuilder.CreateContainer("A")
                .AddNode(treeBuilder.CreateContainer("B"));

            container.Should().NotBeNull();
            container.Id.Should().Be("A");
            container.Nodes.Should().HaveCount(1);

            var subContainer = container.GetContainers().First();
            subContainer.Id.Should().Be("B");
        }

        [Fact]
        public void Clone_ShouldCreateExactCopy()
        {
            var container = treeBuilder.CreateContainer("A");
            var copy = container.Clone(container.Nodes);

            copy.Should().NotBeNull();
            copy.Should().NotBe(container);
            copy.Id.Should().Be(container.Id);
            copy.Nodes.Should().BeEmpty();
        }

        [Fact]
        public void BuildTreeByConstruction_ShouldBuildExpectedTree()
        {
            var container = treeBuilder.BuildTree("A", 3, 3);
            CheckTree(container, "A", 3, 3);
        }

        [Fact]
        public void BuildTreeByEvolution_ShouldBuildExpectedTree()
        {
            var container = treeBuilder.BuildTreeViaEvolution("A", 3, 3);
            CheckTree(container, "A", 3, 3);
        }

        [Fact]
        public void AddNode_WhenNodeIsContainer_ShouldAddSubContainer()
        {
            var container = treeBuilder.CreateContainer("A");
            container = container.AddNode(treeBuilder.CreateContainer("B"));

            container.Should().NotBeNull();
            container.Id.Should().Be("A");
            container.Nodes.Should().HaveCount(1);

            var subContainer = container.GetContainers().First();
            subContainer.Id.Should().Be("B");
        }

        [Fact]
        public void AddNodeAt_WhenRootIsEmpty_ShouldFail()
        {
            var containerA = treeBuilder.CreateContainer("A");
            var containerB = treeBuilder.CreateContainer("B");
            var containerC = treeBuilder.CreateContainer("C");

            Action action = () => containerA = containerA.AddNodeAt(containerB, containerC);
            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void AddNodeAt_WhenTargetIsNotFound_ShouldFail()
        {
            var containerA = treeBuilder.BuildTree("A", 1, 2);
            var containerB = treeBuilder.CreateContainer("B");
            var containerC = treeBuilder.CreateContainer("C");

            Action action = () => containerA = containerA.AddNodeAt(containerB, containerC);
            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void InsertNode_WhenNodeIsContainer_ShouldInsertSubContainer()
        {
            var container = treeBuilder.CreateContainer("A");
            container = container.AddNode(treeBuilder.CreateContainer("C"));
            container = container.InsertNode(0, treeBuilder.CreateContainer("B"));

            container.Should().NotBeNull();
            container.Id.Should().Be("A");
            container.Nodes.Should().HaveCount(2);

            var containerB = container.GetContainers().First();
            var containerC = container.GetContainers().Last();
            containerB.Id.Should().Be("B");
            containerC.Id.Should().Be("C");
        }

        [Fact]
        public void InsertNodeAt_WhenNodeIsContainer_ShouldInsertSubContainer()
        {
            var container = treeBuilder.BuildTree("A", 3, 3);

            container = container.InsertNodeAt(
                container.GetContainers().First(),
                2, treeBuilder.CreateContainer("B")
            );

            container.Should().NotBeNull();
            container.Nodes.Should().HaveCount(3);
            container.GetContainers().First().Nodes.Should().HaveCount(4);

            var containerB = container.GetContainers().First().Nodes.Skip(2).First();
            containerB.Id.Should().Be("B");
        }

        [Fact]
        public void InsertNodeAt_WhenRootIsEmpty_ShouldFail()
        {
            var containerA = treeBuilder.CreateContainer("A");
            var containerB = treeBuilder.CreateContainer("B");
            var containerC = treeBuilder.CreateContainer("B");

            Action action = () => containerA = containerA.InsertNodeAt(containerB, 2, containerC);
            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void InsertNodeAt_WhenTargetIsNotFound_ShouldFail()
        {
            var containerA = treeBuilder.BuildTree("A", 1, 2);
            var containerB = treeBuilder.CreateContainer("B");
            var containerC = treeBuilder.CreateContainer("C");

            Action action = () => containerA = containerA.InsertNodeAt(containerB, 0, containerC);
            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void RemoveNode_WhenNodeExists_ShouldRemoveNode()
        {
            var root = treeBuilder.BuildTree("A", 1, 3);

            root.Nodes.Should().HaveCount(3);
            root.Nodes.First().Id.Should().Be("A.1");

            root = root.RemoveNode(root.Nodes.First());

            root.Nodes.Should().HaveCount(2);
            root.Nodes.First().Id.Should().Be("A.2");
        }

        [Fact]
        public void RemoveChildNodes_WhenNodesExists_ShouldRemoveNodes()
        {
            var root = treeBuilder.BuildTree("A", 1, 3);

            root.Nodes.Should().HaveCount(3);
            root.Nodes.First().Id.Should().Be("A.1");

            root = root.RemoveChildNodes(root.Nodes.Take(2));

            root.Nodes.Should().HaveCount(1);
            root.Nodes.First().Id.Should().Be("A.3");
        }

        [Fact]
        public void ReplaceNode_WhenOldNodeExists_ShouldReplaceNode()
        {
            var root = treeBuilder.BuildTree("A", 1, 3);

            root.Nodes.Should().HaveCount(3);
            root.Nodes.First().Id.Should().Be("A.1");

            root = root.ReplaceNode(root.Nodes.First(), treeBuilder.CreateContainer("B"));

            root.Nodes.Should().HaveCount(3);
            root.Nodes.First().Id.Should().Be("B");
        }

        [Fact]
        public void ReplaceNodeAt_WhenOldNodeExists_ShouldReplaceNodeAndRetainRestOfTree()
        {
            // Creates a 3x3 tree and replaces a sub-branch, then validates that the new tree 
            // contains the replaced branch as well as the actual unchanged branches from the
            // original tree

            var root = treeBuilder.BuildTree("A", 3, 3);

            CheckTree(root, "A", 3, 3);

            var containerA1 = root.GetContainers().First();
            var containerA2 = root.GetContainers().Skip(1).First();
            var containerA3 = root.GetContainers().Skip(2).First();

            var containerA2_1 = containerA2.GetContainers().First();
            var containerA2_2 = containerA2.GetContainers().Skip(1).First();
            var containerA2_3 = containerA2.GetContainers().Skip(2).First();

            var containerA2_2_replaced = treeBuilder.BuildTree("A.2.2", 1, 3);
            root = root.ReplaceNodeAt(containerA2, containerA2_2, containerA2_2_replaced);

            CheckTree(root, "A", 3, 3);

            var containerA1_new = root.GetContainers().First();
            var containerA2_new = root.GetContainers().Skip(1).First();
            var containerA3_new = root.GetContainers().Skip(2).First();

            var containerA2_1_new = containerA2_new.GetContainers().First();
            var containerA2_2_new = containerA2_new.GetContainers().Skip(1).First();
            var containerA2_3_new = containerA2_new.GetContainers().Skip(2).First();

            containerA1_new.Should().Be(containerA1, "because it was unchanged");
            containerA2_new.Should().NotBe(containerA2, "because content was modified");
            containerA3_new.Should().Be(containerA3, "because it was unchanged");

            containerA2_1_new.Should().Be(containerA2_1, "because it was unchanged");
            containerA2_2_new.Should().NotBe(containerA2_2, "because content was modified");
            containerA2_3_new.Should().Be(containerA2_3, "because it was unchanged");

            containerA2_2_new.Should().Be(containerA2_2_replaced, "because it was added to the tree");
        }

        [Fact]
        public void ReplaceNode_WhenOldNodeIsNotFound_ShouldFail()
        {
            var containerA = treeBuilder.CreateContainer("A");
            var containerB = treeBuilder.CreateContainer("B");
            var containerC = treeBuilder.CreateContainer("C");

            Action action = () => containerA = containerA.ReplaceNode(containerB, containerC);
            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void ReplaceNodeAt_WhenOldNodeIsNotFound_ShouldFail()
        {
            var containerA = treeBuilder.BuildTree("A", 3, 3);
            var containerA1 = containerA.GetContainers().First();
            var containerB = treeBuilder.CreateContainer("B");
            var containerC = treeBuilder.CreateContainer("C");

            Action action = () => containerA = containerA.ReplaceNodeAt(containerA1, containerB, containerC);
            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void ReplaceNodeAt_WhenRootIsEmpty_ShouldFail()
        {
            var containerA = treeBuilder.CreateContainer("A");
            var containerB = treeBuilder.CreateContainer("B");
            var containerC = treeBuilder.CreateContainer("C");
            var containerD = treeBuilder.CreateContainer("D");

            Action action = () => containerA = containerA.ReplaceNodeAt(containerB, containerC, containerD);
            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void GetParents_OfSubNode_ShouldReturnParents()
        {
            var tree = treeBuilder.BuildTree("A", 3, 3);
            var node = tree.FindNode(x => x.Id == "A.2.3.3");
            var containers = node.GetParents(tree).ToList();
            var path = string.Join("/", containers.Select(x => x.Id));

            Assert.Equal("A/A.2/A.2.3", path);
        }

        [Fact]
        public void GetPath_OfSubNode_ShouldReturnPath()
        {
            var f0 = treeBuilder.BuildTree("A", 3, 3);
            var f1 = f0.GetContainers().Last();
            var f2 = f1.GetContainers().Last();
            var f3 = f2.GetContainers().Last();

            var expected = string.Join('/', new[] { f0, f1, f2 }.Select(x => x.Id));
            var actual = f3.GetPath(f0);

            Assert.Equal(expected, actual);
        }

        private void CheckTree(IContainer container, string id, int depth, int width)
        {
            container.Should().NotBeNull();
            container.Id.Should().Be(id);
            container.Nodes.Should().HaveCount(depth > 0 ? width : 0);

            int x = 1;
            foreach (var subContainer in container.GetContainers())
            {
                CheckTree(subContainer, treeBuilder.GetContainerId(id, x++), depth - 1, width);
            }
        }
    }
}
