using FluentAssertions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Xodium.Core.Tests.Models;
using Xodium.DataStructures;
using Xunit;

namespace Xodium.Core.Tests
{
    [ExcludeFromCodeCoverage]
    public partial class TreeManipulationTest
    {
        private readonly TreeBuilder<SampleNode> treeBuilder;

        public TreeManipulationTest()
        {
            treeBuilder = SampleNode.CreateTreeBuilder();
        }

        [Fact]
        public void CreateNode_WithNoContent_ShouldCreateEmptyNode()
        {
            var node = treeBuilder.CreateNode("A");

            node.Should().NotBeNull();
            node.Id.Should().Be("A");
            node.Nodes.Should().BeEmpty();
        }

        [Fact]
        public void CreateNode_WithAddedNode_ShouldCreateNodeWithOneChild()
        {
            var node = treeBuilder.CreateNode("A")
                .AddNode(treeBuilder.CreateNode("B"));

            node.Should().NotBeNull();
            node.Id.Should().Be("A");
            node.Nodes.Should().HaveCount(1);

            var child = node.GetSubTrees().First();
            child.Id.Should().Be("B");
        }

        [Fact]
        public void Clone_WhenNodeIsEmpty_ShouldCreateExactCopy()
        {
            var node = treeBuilder.CreateNode("A");
            var copy = node.Clone() as ITree;

            copy.Should().NotBeNull();
            copy.Should().NotBe(node);
            copy.Id.Should().Be(node.Id);
            copy.Nodes.Should().BeEmpty();
        }

        [Fact]
        public void Clone_WhenNodeHasChildren_ShouldCreateExactCopy()
        {
            var tree = treeBuilder.BuildTree("A", 1, 2);
            var copy = tree.Clone() as ITree;

            // Validate root
            copy.Should().NotBeNull();
            copy.Should().NotBe(tree);
            copy.Id.Should().Be(tree.Id);
            copy.Nodes.Count.Should().Be(2);

            var child1 = tree.Nodes[0];
            var child2 = tree.Nodes[1];
            var copyChild1 = copy.Nodes[0];
            var copyChild2 = copy.Nodes[1];

            // Validate children
            copyChild1.Should().Be(child1);
            copyChild2.Should().Be(child2);
        }

        [Fact]
        public void BuildTreeByConstruction_ShouldBuildExpectedTree()
        {
            var tree = treeBuilder.BuildTree("A", 3, 3);
            CheckTree(tree, "A", 3, 3);
        }

        [Fact]
        public void BuildTreeByEvolution_ShouldBuildExpectedTree()
        {
            var tree = treeBuilder.BuildTreeViaEvolution("A", 3, 3);
            CheckTree(tree, "A", 3, 3);
        }

        [Fact]
        public void AddNode_ShouldAddNode()
        {
            var nodeA = treeBuilder.CreateNode("A");
            var nodeB = treeBuilder.CreateNode("B");
            nodeA = nodeA.AddNode(nodeB);

            nodeA.Should().NotBeNull();
            nodeA.Id.Should().Be("A");
            nodeA.Nodes.Should().HaveCount(1);

            var child = nodeA.GetSubTrees().First();
            child.Id.Should().Be("B");
        }

        [Fact]
        public void AddNodeAt_WhenTargetIsNotPartOfTree_ShouldFail()
        {
            var tree = treeBuilder.BuildTree("A", 1, 2);
            var nodeB = treeBuilder.CreateNode("B");
            var nodeC = treeBuilder.CreateNode("C");

            Action action = () => tree = tree.AddNodeAt(nodeB, nodeC);
            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void InsertNode_ShouldInsertNode()
        {
            var nodeA = treeBuilder.CreateNode("A");
            nodeA = nodeA.AddNode(treeBuilder.CreateNode("C"));
            nodeA = nodeA.InsertNode(0, treeBuilder.CreateNode("B"));

            nodeA.Should().NotBeNull();
            nodeA.Id.Should().Be("A");
            nodeA.Nodes.Should().HaveCount(2);

            var nodeB = nodeA.GetSubTrees().First();
            var nodeC = nodeA.GetSubTrees().Last();
            nodeB.Id.Should().Be("B");
            nodeC.Id.Should().Be("C");
        }

        [Fact]
        public void InsertNodeAt_ShouldInsertNode()
        {
            var tree = treeBuilder.BuildTree("A", 3, 3);

            tree = tree.InsertNodeAt(
                tree.GetSubTrees().First(),
                2, treeBuilder.CreateNode("B")
            );

            tree.Should().NotBeNull();
            tree.Nodes.Should().HaveCount(3);
            tree.GetSubTrees().First().Nodes.Should().HaveCount(4);

            var nodeB = tree.GetSubTrees().First().Nodes.Skip(2).First();
            nodeB.Id.Should().Be("B");
        }

        [Fact]
        public void InsertNodeAt_WhenTargetIsNotPartOfTree_ShouldFail()
        {
            var tree = treeBuilder.BuildTree("A", 1, 2);
            var target = treeBuilder.CreateNode("B");
            var node = treeBuilder.CreateNode("C");

            Action action = () => tree = tree.InsertNodeAt(target, 0, node);
            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void RemoveNode_WhenNodeExists_ShouldRemoveNode()
        {
            var tree = treeBuilder.BuildTree("A", 1, 3);

            tree.Nodes.Should().HaveCount(3);
            tree.Nodes.First().Id.Should().Be("A.1");

            tree = tree.RemoveNode(tree.Nodes.First());

            tree.Nodes.Should().HaveCount(2);
            tree.Nodes.First().Id.Should().Be("A.2");
        }

        [Fact]
        public void RemoveChildNodes_WhenNodesExists_ShouldRemoveNodes()
        {
            var tree = treeBuilder.BuildTree("A", 1, 3);

            tree.Nodes.Should().HaveCount(3);
            tree.Nodes.First().Id.Should().Be("A.1");

            tree = tree.RemoveChildNodes(tree.Nodes.Take(2));

            tree.Nodes.Should().HaveCount(1);
            tree.Nodes.First().Id.Should().Be("A.3");
        }

        [Fact]
        public void ReplaceNode_WhenOldNodeExists_ShouldReplaceNode()
        {
            var tree = treeBuilder.BuildTree("A", 1, 3);

            tree.Nodes.Should().HaveCount(3);
            tree.Nodes.First().Id.Should().Be("A.1");

            tree = tree.ReplaceNode(tree.Nodes.First(), treeBuilder.CreateNode("B"));

            tree.Nodes.Should().HaveCount(3);
            tree.Nodes.First().Id.Should().Be("B");
        }

        [Fact]
        public void ReplaceNodeAt_WhenOldNodeExists_ShouldReplaceNodeAndRetainRestOfTree()
        {
            // Creates a 3x3 tree and replaces a sub-branch, then validates that the new tree 
            // contains the replaced branch as well as the actual and unchanged branches of the
            // original tree

            var tree = treeBuilder.BuildTree("A", 3, 3);

            CheckTree(tree, "A", 3, 3);

            var nodedA1 = tree.GetSubTrees().First();
            var nodeA2 = tree.GetSubTrees().Skip(1).First();
            var nodeA3 = tree.GetSubTrees().Skip(2).First();

            var nodeA2_1 = TreeExtensions.GetSubTrees(nodeA2).First();
            var nodeA2_2 = nodeA2.GetSubTrees().Skip(1).First();
            var nodeA2_3 = nodeA2.GetSubTrees().Skip(2).First();

            var nodeA2_2_replaced = treeBuilder.BuildTree("A.2.2", 1, 3);
            tree = tree.ReplaceNodeAt(nodeA2, nodeA2_2, nodeA2_2_replaced);

            CheckTree(tree, "A", 3, 3);

            var nodeA1_new = tree.GetSubTrees().First();
            var nodeA2_new = tree.GetSubTrees().Skip(1).First();
            var nodeA3_new = tree.GetSubTrees().Skip(2).First();

            var nodeA2_1_new = TreeExtensions.GetSubTrees(nodeA2_new).First();
            var nodeA2_2_new = nodeA2_new.GetSubTrees().Skip(1).First();
            var nodeA2_3_new = nodeA2_new.GetSubTrees().Skip(2).First();

            nodeA1_new.Should().Be(nodedA1, "because it was unchanged");
            nodeA2_new.Should().NotBe(nodeA2, "because content was modified");
            nodeA3_new.Should().Be(nodeA3, "because it was unchanged");

            nodeA2_1_new.Should().Be(nodeA2_1, "because it was unchanged");
            nodeA2_2_new.Should().NotBe(nodeA2_2, "because content was modified");
            nodeA2_3_new.Should().Be(nodeA2_3, "because it was unchanged");

            nodeA2_2_new.Should().Be(nodeA2_2_replaced, "because it was added to the tree");
        }

        [Fact]
        public void ReplaceNode_WhenOldNodeIsNotFound_ShouldFail()
        {
            var nodeA = treeBuilder.CreateNode("A");
            var nodeB = treeBuilder.CreateNode("B");
            var nodeC = treeBuilder.CreateNode("C");

            Action action = () => nodeA = nodeA.ReplaceNode(nodeB, nodeC);
            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void ReplaceNodeAt_WhenOldNodeIsNotFound_ShouldFail()
        {
            var nodeA = treeBuilder.BuildTree("A", 3, 3);
            var nodeA1 = nodeA.GetSubTrees().First();
            var nodeB = treeBuilder.CreateNode("B");
            var nodeC = treeBuilder.CreateNode("C");

            Action action = () => nodeA = nodeA.ReplaceNodeAt(nodeA1, nodeB, nodeC);
            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void ReplaceNodeAt_WhenTargetIsNotPartOfTree_ShouldFail()
        {
            var nodeA = treeBuilder.CreateNode("A");
            var nodeB = treeBuilder.CreateNode("B");
            var nodeC = treeBuilder.CreateNode("C");
            var nodeD = treeBuilder.CreateNode("D");

            Action action = () => nodeA = nodeA.ReplaceNodeAt(nodeB, nodeC, nodeD);
            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void GetAncestors_OfSubNode_ShouldReturnAncestors()
        {
            var tree = treeBuilder.BuildTree("A", 3, 3);
            var node = tree.FindNode(x => x.Id == "A.2.3.3");
            var ancestors = node.GetAncestors(tree).ToList();
            var path = string.Join("/", ancestors.Select(x => x.Id));

            Assert.Equal("A/A.2/A.2.3", path);
        }

        [Fact]
        public void GetPath_OfSubNode_ShouldReturnPath()
        {
            var root = treeBuilder.BuildTree("A", 3, 3);
            var child = root.GetSubTrees().Last();
            var grandChild = child.GetSubTrees().Last();
            var greatGrandChild = grandChild.GetSubTrees().Last();

            var expected = string.Join('/', new[] { root, child, grandChild }.Select(x => x.Id));
            var actual = greatGrandChild.GetPath(root);

            Assert.Equal(expected, actual);
        }

        private void CheckTree(ITree tree, string id, int depth, int width)
        {
            tree.Should().NotBeNull();
            tree.Id.Should().Be(id);
            tree.Nodes.Should().HaveCount(depth > 0 ? width : 0);

            int x = 1;
            foreach (var subTree in tree.GetSubTrees())
            {
                CheckTree(subTree, treeBuilder.ProvideId(id, x++), depth - 1, width);
            }
        }
    }
}
