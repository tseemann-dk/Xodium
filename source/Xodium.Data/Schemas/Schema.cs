using System.Collections.Generic;
using System.Linq;

namespace Xodium.Data.Schemas
{
    public class Schema : ISchema
    {
        private readonly IReadOnlyList<IField> fields;

        public Schema(IEnumerable<IField> fields)
        {
            this.fields = fields?.ToList() ?? new List<IField>();
        }

        public IReadOnlyList<IField> Fields => fields;

        public int IndexOfField(string name)
        {
            var index = 0;

            foreach (var field in fields)
            {
                if (field.Name == name)
                {
                    return index;
                }

                index++;
            }

            return -1;
        }
    }
}
