using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web;

namespace DisplayModeMatrix
{
    public class Evidence : IEqualityComparer<Evidence>
    {
        public string Name { get; set; }

        public Expression<Func<HttpContextBase, bool>> Expression { get; set; }

        internal double Weight { get; set; }

        public override string ToString() => $"{Name}, {Weight}: {Expression}";

        public bool Equals(Evidence a, Evidence b) => a.Name.Equals(b.Name);

        public int GetHashCode(Evidence c) => c.Name.GetHashCode();
    }
}