using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Xodium.Mvvm
{
    public class AsyncEvent<TEventArgs> 
        where TEventArgs : EventArgs
    {
        private readonly List<Func<object, TEventArgs, Task>> handlers = new List<Func<object, TEventArgs, Task>>();
        private readonly object locker = new object();

        public static AsyncEvent<TEventArgs> operator +(AsyncEvent<TEventArgs> e, Func<object, TEventArgs, Task> callback)
        {
            if (callback == null)
                throw new ArgumentNullException(nameof(callback));

            if (e == null) e = new AsyncEvent<TEventArgs>();

            lock (e.locker)
            {
                e.handlers.Add(callback);
            }

            return e;
        }

        public static AsyncEvent<TEventArgs> operator -(AsyncEvent<TEventArgs> e, Func<object, TEventArgs, Task> callback)
        {
            if (callback == null)
                throw new ArgumentNullException(nameof(callback));

            if (e == null) return null;

            lock (e.locker)
            {
                e.handlers.Remove(callback);
            }

            return e;
        }

        public async Task InvokeAsync(object sender, TEventArgs args, bool parallel = false)
        {
            List<Func<object, TEventArgs, Task>> funcs;

            lock (locker)
            {
                funcs = new List<Func<object, TEventArgs, Task>>(handlers);
            }

            if (parallel)
            {
                await Task.WhenAll(funcs.Select(f => f(sender, args)));
            }
            else
            {
                foreach (var callback in funcs)
                {
                    await callback(sender, args);
                }
            }
        }
    }

    public class AsyncEvent : AsyncEvent<EventArgs>
    {
    }
}
