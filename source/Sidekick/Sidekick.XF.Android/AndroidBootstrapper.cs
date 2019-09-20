using System;
using Android.Content;
using Sidekick.State;
using Xodium.Injection;
using Xodium.Mvvm;
using Xodium.Platform.Android;

namespace Sidekick.XF.Droid
{
    public class AndroidBootstrapper : Bootstrapper
    {
        private readonly Context context;

        public AndroidBootstrapper(Context context, StoreProvider<AppState> storeProvider = null) 
            : base(storeProvider)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        protected override IExecutionEnvironment CreateExecutionEnvironment(Func<IDependencyResolver> resolver)
            => new AndroidExecutionEnvironment(resolver, context);
    }
}