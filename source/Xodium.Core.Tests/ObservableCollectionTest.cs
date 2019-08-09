using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xodium.Collections;
using Xunit;

namespace Xodium.Core.Tests
{
    public class ObservableCollectionTest
    {
        private readonly ObservableCollection<ItemViewModel> items;

        public ObservableCollectionTest()
        {
            items = new ObservableCollection<ItemViewModel>(
                BuildItems(1, 5).Select(ItemViewModel.FromItem));
        }

        [Fact]
        public void MorphTo_WhenItemsAreMissing_AddsItems()
        {
            var goal = BuildItems(1, 8).ToList();
            MorphItemsTo(goal);
            AssertItemsEquals(goal);
        }

        [Fact]
        public void MorphTo_WhenItemsAreInExcess_RemovesItems()
        {
            var goal = BuildItems(1, 3).ToList();
            MorphItemsTo(goal);
            AssertItemsEquals(goal);
        }

        [Fact]
        public void MorphTo_WhenItemsAreOutOfSequence_ReordersItems()
        {
            var goal = BuildItems(1, 5).ToList();

            // Swap item 0 and 1
            var item = goal[0];
            goal[0] = goal[1];
            goal[1] = item;

            MorphItemsTo(goal);
            AssertItemsEquals(goal);
        }

        [Fact]
        public void MorphTo_WhenItemsAreReversed_ReversesItems()
        {
            var goal = BuildItems(1, 5).Reverse().ToList();
            MorphItemsTo(goal);
            AssertItemsEquals(goal);
        }

        private IEnumerable<Item> BuildItems(int start, int count)
        {
            return Enumerable
                .Range(start, count)
                .Select(x => new Item($"{x}", $"Item {x}"));
        }

        private void MorphItemsTo(IReadOnlyCollection<Item> other)
        {
            items.MorphTo(
                other,
                (x, y) => x.Item.Id == y.Id,
                (x, y) => ReferenceEquals(x, y),
                ItemViewModel.FromItem);
        }

        private void AssertItemsEquals(IReadOnlyCollection<Item> other)
        {
            AssertSequencesAreEqual(items.Select(x => x.Item), other);
        }

        private void AssertSequencesAreEqual(IEnumerable<Item> sequence1, IEnumerable<Item> sequence2)
        {
            var list1 = sequence1.ToList();
            var list2 = sequence2.ToList();

            list1.Count.Should().Be(list2.Count);

            for (var i = 0; i < list1.Count; i++)
            {
                var item1 = list1[i];
                var item2 = list2[i];

                if (item1 != item2)
                {
                    item1.Should().NotBeNull();
                    item2.Should().NotBeNull();
                }

                item1.Id.Should().Be(item2.Id, $"items at index {i} should have same Id");
            }
        }

        class Item
        {
            public Item(string id, string text)
            {
                Id = id;
                Text = text;
            }

            public string Id { get; }
            public string Text { get; }

            public override string ToString() => $"{Id}: {Text}";
        }

        class ItemViewModel
        {
            public ItemViewModel(Item item)
            {
                Item = item;
            }

            public static ItemViewModel FromItem(Item item) => new ItemViewModel(item);

            public Item Item { get; }

            public override string ToString() => Item.ToString();
        }
    }
}
