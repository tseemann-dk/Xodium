using FluentAssertions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Xodium.Core.Tests.Models;
using Xodium.Core.Tests.Utilities;
using Xodium.DataStructures;
using Xunit;

namespace Xodium.Core.Tests
{
    [ExcludeFromCodeCoverage]
    public class TreeInspectionTest
    {
        private readonly TreeBuilder<SampleNode> treeBuilder;

        public TreeInspectionTest()
        {
            treeBuilder = SampleNode.CreateTreeBuilder();
        }

        [Fact]
        public void GetParent_WhenNodeIsFirstChild_ReturnsParent()
        {
            var tree = treeBuilder.BuildTree("A", 1, 3);
            var child = tree.Nodes.First();
            var parent = child.GetParent(tree);

            parent.Should().Be(tree);
        }

        [Fact]
        public void GetParent_WhenNodeIsLastChild_ReturnsParent()
        {
            var tree = treeBuilder.BuildTree("A", 1, 3);
            var child = tree.Nodes.Last();
            var parent = child.GetParent(tree);

            parent.Should().Be(tree);
        }

        [Fact]
        public void GetParent_WhenNodeIsFirstGrandChild_ReturnsParent()
        {
            var tree = treeBuilder.BuildTree("A", 2, 3);
            var child = tree.GetSubTrees().First();
            var grandChild = child.Nodes.First();
            var parentOfGrandChild = grandChild.GetParent(tree);

            parentOfGrandChild.Should().Be(child);
        }

        [Fact]
        public void FindNode_WhenMatchingNodeExists_ShouldReturnNode()
        {
            var tree = treeBuilder.BuildTree("A", 3, 3);
            var node = tree.FindNode(x => x.Id == "A.1.3");

            node.Should().NotBeNull();
            node.Id.Should().Be("A.1.3");
        }

        [Fact]
        public void FindParentOf_WhenNodeExists_ShouldReturnParent()
        {
            var tree = treeBuilder.BuildTree("A", 3, 3);
            var node = tree.GetSubTrees().First().GetSubTrees().First();
            var parent = tree.FindParentOf(node);

            parent.Should().NotBeNull();
            parent.Id.Should().Be("A.1");
        }

        [Fact]
        public void FindParentOf_WhenNodeIsNull_ShouldFail()
        {
            var node = treeBuilder.CreateNode("A");
            Action action = () => node.FindParentOf(null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void FindNode_WhenPredicateIsNull_ShouldFail()
        {
            var node = treeBuilder.CreateNode("A");
            Action action = () => node.FindNode(null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void GetIndexOfNode_WhenNodeExists_ShouldReturnIndex()
        {
            var tree = treeBuilder.BuildTree("A", 3, 2);
            var node = tree.Nodes[1];
            var index = tree.GetIndexOfNode(node);

            index.Should().Be(1);
        }

        [Fact]
        public void GetIndexOfNode_WhenNodeIsMissing_ShouldReturnNegative()
        {
            var tree = treeBuilder.BuildTree("A", 3, 2);
            var node = tree.GetSubTrees().First().Nodes.First();
            var index = tree.GetIndexOfNode(node);

            index.Should().Be(-1);
        }

        [Fact]
        public void GetNextSibling_WhenNodeIsNotLast_ShouldReturnNextSibling()
        {
            var tree = treeBuilder.BuildTree("A", 3, 2);
            var node = tree.Nodes.First();
            var next = tree.GetNextSibling(node);

            next.Should().NotBeNull();
        }

        [Fact]
        public void GetNextSibling_WhenNodeIsLast_ShouldReturnNull()
        {
            var tree = treeBuilder.BuildTree("A", 3, 2);
            var node = tree.Nodes.Last();
            var next = tree.GetNextSibling(node);

            next.Should().BeNull();
        }

        [Fact]
        public void GetPreviousSibling_WhenNodeIsNotFirst_ShouldReturnNextSibling()
        {
            var tree = treeBuilder.BuildTree("A", 3, 2);
            var node = tree.Nodes.Last();
            var previous = tree.GetPreviousSibling(node);

            previous.Should().NotBeNull();
        }

        [Fact]
        public void GetPreviousSibling_WhenNodeIsFirst_ShouldReturnNull()
        {
            var tree = treeBuilder.BuildTree("A", 3, 2);
            var node = tree.Nodes.First();
            var previous = tree.GetPreviousSibling(node);

            previous.Should().BeNull();
        }
    }
}
