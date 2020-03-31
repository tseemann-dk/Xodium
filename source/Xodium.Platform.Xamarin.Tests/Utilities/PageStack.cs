using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xodium.Platform.Xamarin.Tests.Utilities
{
    public class PageStack<T>
        where T : Page
    {
        private readonly List<T> pages = new List<T>();

        public IReadOnlyList<T> Pages => pages;

        public Task<T> Pop()
        {
            if (pages.Count == 0)
                throw new InvalidOperationException("No pages");

            var page = pages.Last();
            pages.Remove(page);
            return Task.FromResult(page);
        }

        public Task Push(T page)
        {
            if (page == null)
                throw new ArgumentNullException(nameof(page));

            pages.Add(page);
            return Task.CompletedTask;
        }

        public Task InsertBefore(T page, T before)
        {
            if (!pages.Contains(before))
                throw new InvalidOperationException("Page not found");

            var index = pages.IndexOf(before);
            pages.Insert(index, page);
            return Task.CompletedTask;
        }

        public Task RemovePage(T page)
        {
            if (!pages.Contains(page))
                throw new InvalidOperationException("Page not found");

            pages.Remove(page);
            return Task.CompletedTask;
        }
    }
}
