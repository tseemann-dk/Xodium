using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Xodium.Mvvm
{
    public class ViewModelBranch
    {
        private readonly Lazy<List<IViewModel>> childList = new Lazy<List<IViewModel>>(() => new List<IViewModel>());
        private readonly IViewModel owner;

        public ViewModelBranch(IViewModel owner, IParentViewModel parent)
        {
            this.owner = owner ?? throw new ArgumentNullException(nameof(owner));
            Parent = parent;
            parent?.AddChild(owner);
        }

        private List<IViewModel> ChildList => childList.Value;

        public IParentViewModel Parent { get; }
        public IReadOnlyList<IViewModel> Children => ChildList;

        public void AddChild(IViewModel child)
        {
            ChildList.Add(child);
        }

        #region Navigation

        public async Task NavigateTo()
        {
            await owner.OnNavigateTo(NavigationDirection.Forward);
            await ForEachChildAsync(async c => await c.NavigateTo());
        }

        public async Task NavigateBackTo()
        {
            await owner.OnNavigateTo(NavigationDirection.Backward);
            await ForEachChildAsync(async c => await c.NavigateBackTo());
        }

        public async Task NavigateFrom()
        {
            await owner.OnNavigateFrom(NavigationDirection.Forward);
            await ForEachChildAsync(async c => await c.NavigateFrom());
        }

        public async Task NavigateBackFrom()
        {
            await owner.OnNavigateFrom(NavigationDirection.Backward);
            await ForEachChildAsync(async c => await c.NavigateBackFrom());
        }

        #endregion

        #region Iterators

        public void ForEachChild(Action<IViewModel> action)
        {
            foreach (var child in Children.ToList())
            {
                action(child);
            }
        }

        public async Task ForEachChildAsync(Func<IViewModel, Task> action)
        {
            foreach (var child in Children.ToList())
            {
                await action(child);
            }
        }

        #endregion
    }
}
