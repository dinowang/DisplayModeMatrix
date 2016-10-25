using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web;

namespace DisplayModeMatrix
{
    public class NamedCondition : IEqualityComparer<NamedCondition>
    {
        public string Name { get; set; }

        public Expression<Func<HttpContextBase, bool>> Expression { get; set; }

        internal double Weight { get; set; }

        public override string ToString() => $"{Name}, {Weight}: {Expression}";

        public bool Equals(NamedCondition a, NamedCondition b) => a.Name.Equals(b.Name);

        public int GetHashCode(NamedCondition c) => c.Name.GetHashCode();
    }
}