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
    public class BranchTest
    {
        private readonly TreeBuilder<SampleContainer> treeBuilder;

        public BranchTest()
        {
            treeBuilder = new TreeBuilder<SampleContainer>((id, nodes) => new SampleContainer(id, nodes));
        }

        [Fact]
        public void FindNode_WhenMatchingNodeExists_ShouldReturnNode()
        {
            var root = treeBuilder.BuildTree("A", 3, 3);
            var folder = root.FindNode(x => x.Id == "A.1.3");

            folder.Should().NotBeNull();
            folder.Id.Should().Be("A.1.3");
        }

        [Fact]
        public void FindBranchOf_WhenNodeExists_ShouldReturnBranch()
        {
            var root = treeBuilder.BuildTree("A", 3, 3);
            var folder = root.GetContainers().First().GetContainers().First();
            var branch = root.FindBranchOf(folder);

            branch.Should().NotBeNull();
            branch.Id.Should().Be("A.1");
        }

        [Fact]
        public void FindBranchOf_WhenNodeIsNull_ShouldFail()
        {
            var folder = treeBuilder.CreateContainer("A");
            Action action = () => folder.FindBranchOf(null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void FindNode_WhenPredicateIsNull_ShouldFail()
        {
            var folder = treeBuilder.CreateContainer("A");
            Action action = () => folder.FindNode(null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void GetIndexOfNode_WhenNodeExists_ShouldReturnIndex()
        {
            var folder = treeBuilder.BuildTree("A", 3, 2);
            var node = folder.Nodes[1];
            var index = folder.GetIndexOfNode(node);

            index.Should().Be(1);
        }

        [Fact]
        public void GetIndexOfNode_WhenNodeIsMissing_ShouldReturnNegative()
        {
            var folder = treeBuilder.BuildTree("A", 3, 2);
            var node = folder.GetContainers().First().Nodes.First();
            var index = folder.GetIndexOfNode(node);

            index.Should().Be(-1);
        }

        [Fact]
        public void GetNextNode_WhenNodeIsNotLast_ShouldReturnNextSibling()
        {
            var folder = treeBuilder.BuildTree("A", 3, 2);
            var node = folder.Nodes.First();
            var next = folder.GetNextNode(node);

            next.Should().NotBeNull();
        }

        [Fact]
        public void GetNextNode_WhenNodeIsLast_ShouldReturnNull()
        {
            var folder = treeBuilder.BuildTree("A", 3, 2);
            var node = folder.Nodes.Last();
            var next = folder.GetNextNode(node);

            next.Should().BeNull();
        }

        [Fact]
        public void GetPreviousNode_WhenNodeIsNotFirst_ShouldReturnNextSibling()
        {
            var folder = treeBuilder.BuildTree("A", 3, 2);
            var node = folder.Nodes.Last();
            var previous = folder.GetPreviousNode(node);

            previous.Should().NotBeNull();
        }

        [Fact]
        public void GetPreviousNode_WhenNodeIsFirst_ShouldReturnNull()
        {
            var folder = treeBuilder.BuildTree("A", 3, 2);
            var node = folder.Nodes.First();
            var previous = folder.GetPreviousNode(node);

            previous.Should().BeNull();
        }
    }
}
