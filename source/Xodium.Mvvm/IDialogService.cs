using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Xodium.Mvvm
{
    public class UserAction
    {
        public UserAction(string name, ICommand command = null, object argument = null)
        {
            Name = name;
            Command = command;
            Argument = argument;
        }

        public UserAction(string name, Action action)
        {
            Name = name;
            Action = action;
        }

        public string Name { get; private set; }
        public ICommand Command { get; }
        public Action Action { get; }
        public object Argument { get; }

        public void Execute()
        {
            if (Action != null)
            {
                Action();
            }
            else
            {
                Command?.Execute(Argument);
            }
        }
    }

    public interface IDialogService
    {
        Task<bool> DisplayAlert(string title, string message, string accept = null, string cancel = null);
        Task<UserAction> DisplayDialog(string title, object viewModel, UserAction primaryAction = null, UserAction secondaryAction = null, CancellationToken cancellationToken = default(CancellationToken));
        Task DisplayException(string title, Exception exception);
        Task<string> DisplayPrompt(string title, string message, string value, string accept = null, string cancel = null);
        Task<UserAction> SelectAction(string title, string cancel, IEnumerable<UserAction> actions);
    }
}
