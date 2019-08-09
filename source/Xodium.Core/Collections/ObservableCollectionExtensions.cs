using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Xodium.Collections
{
    public static class ObservableCollectionExtensions
    {
        public static void MorphTo<T1, T2>(
            this ObservableCollection<T1> self,
            IReadOnlyCollection<T2> other,
            Func<T1, T2, bool> areEqual,
            Func<T1, T2, bool> areSame,
            Func<T2, T1> transform)
        {
            var removableItems = self
                .Where(x => !other.Any(y => areEqual(x, y)))
                .ToArray();

            foreach (var removableItem in removableItems)
            {
                self.Remove(removableItem);
            }

            var index = 0;

            foreach (var otherItem in other)
            {
                if (index >= self.Count)
                {
                    self.Add(transform(otherItem));
                }
                else
                {
                    var existingItem = self[index];

                    if (!areEqual(existingItem, otherItem))
                    {
                        var equalItem = self.FirstOrDefault(x => areEqual(x, otherItem));

                        if (equalItem != null)
                        {
                            var indexOfEqualItem = self.IndexOf(equalItem);
                            var newItem = areSame(equalItem, otherItem) ? equalItem : transform(otherItem);

                            self[index] = newItem;
                            self[indexOfEqualItem] = existingItem;
                        }
                        else
                        {
                            self.Insert(index, transform(otherItem));
                        }
                    }
                    else if (!areSame(existingItem, otherItem))
                    {
                        self[index] = transform(otherItem);
                    }
                }

                index++;
            }
        }
    }
}
