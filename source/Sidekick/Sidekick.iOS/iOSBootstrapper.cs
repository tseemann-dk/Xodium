using System;
using Sidekick.Models;
using Xodium.Injection;
using Xodium.Mvvm;
using Xodium.Platform.iOS;

namespace Sidekick.iOS
{
    public class iOSBootstrapper : Bootstrapper
    {
        public iOSBootstrapper(StoreProvider<AppState> storeProvider = null) 
            : base(storeProvider)
        {
        }

        protected override IExecutionEnvironment GetExecutionEnvironment(Func<IDependencyResolver> resolver)
            => new iOSExecutionEnvironment(resolver);
    }
}