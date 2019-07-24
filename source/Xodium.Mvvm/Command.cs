using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Xodium.Mvvm
{
    public interface IAsyncCommand : ICommand
    {
        Task ExecuteAsync(object parameter);
        void Update();
    }

    public abstract class CommandBase : IAsyncCommand
    {
        public async void Execute(object parameter)
        {
            BeginExecute();
            try
            {
                await ExecuteAsync(parameter);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
            finally
            {
                EndExecute();
            }
        }

        public bool IsExecuting { get; private set; }

        private void BeginExecute()
        {
            if (IsExecuting) return;
            IsExecuting = true;
            OnCanExecuteChanged();
        }

        private void EndExecute()
        {
            if (!IsExecuting) return;
            IsExecuting = false;
            OnCanExecuteChanged();
        }

        public abstract Task ExecuteAsync(object parameter);
        protected abstract bool OnCanExecute(object parameter);

        public bool CanExecute(object parameter) => !IsExecuting && OnCanExecute(parameter);

        public event EventHandler CanExecuteChanged;

        private void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public void Update()
        {
            try
            {
                OnCanExecuteChanged();
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }
    }

    public class Command<T> : CommandBase
        where T : class
    {
        private readonly Func<T, Task> action;
        private readonly Func<T, bool> condition;

        public Command(Func<T, Task> action, Func<T, bool> condition)
        {
            this.action = action;
            this.condition = condition;
        }

        public Command(Func<T, Task> action)
            : this(action, null)
        {
        }

        public Command(Action<T> action, Func<T, bool> condition)
            : this(x => { action(x); return Task.FromResult(true); }, condition)
        {
        }

        public Command(Action<T> action)
            : this(action, null)
        {
        }

        public bool CheckCanExecute(T parameter)
        {
            return condition == null || condition(parameter);
        }

        protected override bool OnCanExecute(object parameter)
        {
            return CheckCanExecute(parameter as T);
        }

        public override Task ExecuteAsync(object parameter)
        {
            return ExecuteAsync(parameter as T);
        }

        public async Task ExecuteAsync(T parameter = null)
        {
            if (!CheckCanExecute(parameter))
                throw new InvalidOperationException("Cannot execute disabled command");

            await action(parameter);
        }

        public void Execute(T parameter)
        {
            Execute((object)parameter);
        }
    }

    public class Command : Command<object>
    {
        public Command(Func<Task> action)
            : base(async _ => await action())
        {
        }

        public Command(Func<Task> action, Func<bool> condition)
            : base(_ => action(), _ => condition == null || condition())
        {
        }

        public Command(Action action)
            : base(_ => action())
        {
        }

        public Command(Action action, Func<bool> condition)
            : base(_ => action(), _ => condition == null || condition())
        {
        }
    }
}
