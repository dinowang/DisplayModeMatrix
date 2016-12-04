using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web;

namespace Hexdigits.DisplayModeMatrix
{
    public class Evidence : IEquatable<Evidence>
    {
        public string Name { get; set; }

        public Expression<Func<HttpContextBase, bool>> Expression { get; set; }

        internal double Weight { get; set; }

        public override string ToString() => $"{Name}, {Weight}: {Expression}";

        public bool Equals(Evidence another) => Name == another.Name;

        public override int GetHashCode() => Name.GetHashCode();
    }
}