using System;
using System.Collections.Generic;
using System.Linq;

namespace Hexdigits.DisplayModeMatrix
{
    public partial class DisplayModeMatrixBuilder
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
    }
}