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

            foreach (var item in removableItems)
            {
                self.Remove(item);
            }

            var index = 0;

            foreach (var item in other)
            {
                if (index >= self.Count)
                {
                    self.Add(transform(item));
                }
                else
                {
                    var existing = self[index];

                    if (!areEqual(existing, item))
                    {
                        self.Insert(index, transform(item));
                    }
                    else if (!areSame(existing, item))
                    {
                        self[index] = transform(item);
                    }
                }

                index++;
            }
        }
    }
}
