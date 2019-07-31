using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Xodium.Productivity.Common
{
    public interface IPage<T> : IEnumerable<T>
    {
        IReadOnlyList<T> Items { get; }
        Task<IPage<T>> NextPage { get; }
    }

    public static class PageExtensions
    {
        public static async Task ForEachPage<T>(this IPage<T> page, Func<IPage<T>, CancellationToken, Task> func, CancellationToken cancellationToken = default(CancellationToken))
        {
            for (var p = page; p != null; p = p.NextPage == null ? null : await p.NextPage)
            {
                if (cancellationToken.IsCancellationRequested) break;
                await func(p, cancellationToken);
            }
        }

        public static async Task ForEach<T>(this IPage<T> page, Func<IPage<T>, T, CancellationToken, Task> func, CancellationToken cancellationToken = default(CancellationToken))
        {
            await page.ForEachPage(async (p, ct) =>
            {
                foreach (var item in p)
                {
                    await func(p, item, ct);
                }
            }, cancellationToken);
        }
    }

    public class Page<T> : IPage<T>
    {
        public Page(IEnumerable<T> items, Task<IPage<T>> nextPage)
        {
            Items = items?.ToList() ?? new List<T>();
            NextPage = nextPage;
        }

        public IReadOnlyList<T> Items { get; }
        public Task<IPage<T>> NextPage { get; }

        public static Page<T> Empty() => new Page<T>(Enumerable.Empty<T>(), Task.FromResult<IPage<T>>(null));

        public IEnumerator<T> GetEnumerator() => Items.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => Items.GetEnumerator();
    }
}
