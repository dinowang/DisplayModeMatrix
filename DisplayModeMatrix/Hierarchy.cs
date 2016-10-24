using System.Collections.Generic;
using System.Linq;

namespace DisplayModeMatrix
{
    public class Hierarchy
    {
        public string Key { get; set; }

        public bool Required { get; set; }

        public IEnumerable<NamedCondition> Values { get; set; }

        public NamedCondition this[string name] => Values.First(x => x.Name == name);

        public static Hierarchy Create(string key, bool required, IEnumerable<NamedCondition> values)
        {
            return new Hierarchy { Key = key, Required = required, Values = values };
        }
    }
}