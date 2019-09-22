using System;

namespace Xodium.Exceptions
{
    public class ServiceNotFoundException : Exception
    {
        public ServiceNotFoundException(Type service) 
            : base("Service not found: " + service.FullName)
        {
        }

        public Type Service { get; }
    }
}
