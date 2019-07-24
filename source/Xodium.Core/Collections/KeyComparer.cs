using System;
using System.Collections.Generic;

namespace Xodium.Collections
{
    public class KeyComparer<T, TKey> : IEqualityComparer<T>
    {
        private readonly Func<T, TKey> keySelector;

        public KeyComparer(Func<T, TKey> keySelector)
        {
            this.keySelector = keySelector ?? throw new ArgumentNullException(nameof(keySelector));
        }

        public static KeyComparer<T, TKey> Create(Func<T, TKey> keySelector)
            => new KeyComparer<T, TKey>(keySelector);

        public bool Equals(T x, T y)
        {
            return keySelector(x).Equals(keySelector(y));
        }

        public int GetHashCode(T obj)
        {
            return keySelector(obj).GetHashCode();
        }
    }
}
