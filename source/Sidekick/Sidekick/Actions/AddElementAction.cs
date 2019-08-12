using Sidekick.Models;
using Xodium.Redux;

namespace Sidekick.Actions
{
    public class AddElementAction : ReduxAction<AddElementAction.Properties>
    {
        public AddElementAction(IElement element)
            : base(typeof(AddElementAction).FullName, new Properties(element))
        {
        }

        public struct Properties
        {
            public Properties(IElement element)
            {
                Element = element;
            }

            public IElement Element;
        }
    }
}
