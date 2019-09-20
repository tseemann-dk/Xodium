using System;
using Sidekick.State;
using Xodium.Injection;
using Xodium.Mvvm;
using Xodium.Platform.iOS;

namespace Sidekick.XF.iOS
{
    public class iOSBootstrapper : Bootstrapper
    {
        public iOSBootstrapper(StoreProvider<AppState> storeProvider = null) 
            : base(storeProvider)
        {
        }

        protected override IExecutionEnvironment CreateExecutionEnvironment(Func<IDependencyResolver> resolver)
            => new iOSExecutionEnvironment(resolver);
    }
}