using System.Collections.Generic;

namespace Xodium.Mvvm
{
    public class CommandCollection
    {
        private readonly List<IAsyncCommand> commandList = new List<IAsyncCommand>();

        public CommandCollection()
        {
        }

        public IReadOnlyList<IAsyncCommand> Commands => commandList;

        public IAsyncCommand AddCommand(IAsyncCommand command)
        {
            commandList.Add(command);
            return command;
        }

        public void RemoveCommand(IAsyncCommand command) => commandList.Remove(command);

        public void Update()
        {
            foreach (var command in commandList)
            {
                command.Update();
            }
        }
    }
}
