using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace DisplayModeMatrix
{
    public class DisplayModeMatrixBuilder
    {
        private List<Factor> _factors = new List<Factor>();

        public DisplayModeMatrixBuilder AddOptionalFactor(string name, Action<FactorBuilder> register)
        {
            return AddFactor(name, false, register);
        }

        public DisplayModeMatrixBuilder AddRequiredFactor(string name, Action<FactorBuilder> register)
        {
            return AddFactor(name, true, register);
        }

        public DisplayModeMatrixBuilder AddFactor(string name, bool required, Action<FactorBuilder> register)
        {
            var factorBuilder = new FactorBuilder(_factors.Count + 1);

            register(factorBuilder);

            var hierarchy = new Factor()
            {
                Key = name,
                Required = required,
                Values = factorBuilder.Evidences
            };

            _factors.Add(hierarchy);

            return this;
        }

        public IEnumerable<DisplayModeProfile> Build()
        {
            var weight = _factors.Count;

            foreach (var factor in _factors)
            {
                foreach (var value in factor.Values)
                {
                    value.Weight = weight;
                }
                weight--;
            }

            return _factors
                        .Permutation()
                        .Where(x => x != null)
                        .OrderByDescending(x => x.Weight)
                        .Distinct()
                        .Select(x => new DisplayModeProfile
                        {
                            Name = x.Name,
                            ContextCondition = x.Expression.Compile()
                        });
        }

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
}