using System;
using Sidekick.State;
using Xodium.Injection;
using Xodium.Mvvm;
using Xodium.Platform.Uwp;

namespace Sidekick.XF.UWP
{
    public class UwpBootstrapper : Bootstrapper
    {
        public UwpBootstrapper(StoreProvider<AppState> storeProvider)
            : base(storeProvider)
        {
        }

        protected override IExecutionEnvironment CreateExecutionEnvironment(Func<IDependencyResolver> resolver)
            => new UwpExecutionEnvironment(resolver);
    }
}
