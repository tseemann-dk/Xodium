using System.Collections.Generic;

namespace Xodium.Data.Schemas
{
    public interface ISchema
    {
        IReadOnlyList<IField> Fields { get; }
        int IndexOfField(string name);
    }
}
