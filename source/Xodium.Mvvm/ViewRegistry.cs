using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Xodium.Mvvm
{
    public class ViewRegistry : IViewRegistry
    {
        private readonly Dictionary<Type, Type> viewTypes = new Dictionary<Type, Type>();
        private readonly Dictionary<Type, Func<object, object>> factories = new Dictionary<Type, Func<object, object>>();
        private readonly Func<Type, object> typeResolver;

        public ViewRegistry()
        {
        }

        public ViewRegistry(Func<Type, object> typeResolver)
        {
            this.typeResolver = typeResolver ?? throw new ArgumentNullException(nameof(typeResolver));
        }

        public object GetViewFor<TViewModel>()
        {
            return GetViewFor(typeof(TViewModel));
        }

        public object GetViewFor(Type viewModelType)
        {
            return GetViewFor(ResolveType(viewModelType));
        }

        public object GetViewFor(object viewModel)
        {
            if (viewModel == null) return null;

            var viewModelType = viewModel.GetType();

            if (factories.TryGetValue(viewModelType, out var viewProvider))
            {
                return viewProvider(viewModel);
            }

            var viewType = GetViewTypeFor(viewModel.GetType());

            return CreateView(viewType, viewModel);
        }

        public Type GetViewTypeFor(Type viewModelType)
        {
            for (var vmt = viewModelType; vmt != null; vmt = vmt.GetTypeInfo().BaseType)
            {
                if (viewTypes.TryGetValue(vmt, out Type viewType))
                {
                    return viewType;
                }
            }

            throw new ViewRegistryException($"No view registered for view model type: {viewModelType.FullName}");
        }

        private object CreateView(Type viewType, object viewModel)
        {
            var constructors = viewType.GetTypeInfo().DeclaredConstructors.ToArray();

            if (constructors.Any(x => x.GetParameters().Length == 1))
            {
                return Activator.CreateInstance(viewType, viewModel);
            }

            if (constructors.Any(x => x.GetParameters().Length == 0))
            {
                return Activator.CreateInstance(viewType);
            }

            throw new ArgumentException("Constructor taking 0 or 1 argument not found on type " + viewType.FullName, nameof(viewType));
        }

        public void RegisterViewType<TView, TViewModel>()
        {
            RegisterViewType(typeof(TView), typeof(TViewModel));
        }

        public void RegisterViewType(Type viewType, Type viewModelType)
        {
            viewTypes[viewModelType] = viewType;
        }

        public void RegisterViewFactory<TViewModel>(Func<TViewModel, object> factory)
        {
            RegisterViewFactory(typeof(TViewModel), vm => factory((TViewModel)vm));
        }

        public void RegisterViewFactory(Type viewModelType, Func<object, object> factory)
        {
            factories[viewModelType] = vm => factory(vm);
        }

        private object ResolveType(Type type)
        {
            return typeResolver != null ? typeResolver(type) : Activator.CreateInstance(type);
        }
    }

    public class ViewRegistryException : Exception
    {
        public ViewRegistryException(string message)
            : base(message)
        {
        }
    }
}
