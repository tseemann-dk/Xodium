using System;

namespace Xodium.Mvvm
{
    public interface IViewRegistryProvider
    {
        IViewRegistry ViewRegistry { get; }
    }

    public class ViewRegistryProvider : IViewRegistryProvider
    {
        private readonly Func<IViewRegistry> getViewRegistry;

        public ViewRegistryProvider(Func<IViewRegistry> getViewRegistry)
        {
            this.getViewRegistry = getViewRegistry ?? throw new ArgumentNullException(nameof(getViewRegistry));
        }

        public IViewRegistry ViewRegistry => getViewRegistry();
    }
}
