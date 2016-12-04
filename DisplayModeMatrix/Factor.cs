using System.Collections.Generic;
using System.Linq;

namespace Hexdigits.DisplayModeMatrix
{
    public class Factor
    {
        public string Key { get; set; }

        public bool Required { get; set; }

        public IEnumerable<Evidence> Values { get; set; }

        public Evidence this[string name] => Values.First(x => x.Name == name);

        public static Factor Create(string key, bool required, IEnumerable<Evidence> values)
        {
            return new Factor { Key = key, Required = required, Values = values };
        }
    }
}