using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web;

namespace Hexdigits.DisplayModeMatrix
{
    public class FactorBuilder
    {
        private readonly int _weight;

        public List<Evidence> Evidences = new List<Evidence>();

        internal FactorBuilder(int weight)
        {
            _weight = weight;
        }

        public FactorBuilder Evidence(string name, Expression<Func<HttpContextBase, bool>> expression)
        {
            var evidence = new Evidence
            {
                Name = name,
                Expression = expression,
                Weight = _weight
            };

            Evidences.Add(evidence);

            return this;
        }
    }
}