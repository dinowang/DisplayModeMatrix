using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace DisplayModeMatrix
{
    public class DisplayModeMatrixBuilder
    {
        private List<Layer> _layers = new List<Layer>();

        public DisplayModeMatrixBuilder AddOptionalLayer(string name, Action<LayerBuilder> register)
        {
            return AddLayer(name, false, register);
        }

        public DisplayModeMatrixBuilder AddRequiredLayer(string name, Action<LayerBuilder> register)
        {
            return AddLayer(name, true, register);
        }

        public DisplayModeMatrixBuilder AddLayer(string name, bool required, Action<LayerBuilder> register)
        {
            var layerBuilder = new LayerBuilder(_layers.Count + 1);

            register(layerBuilder);

            var hierarchy = new Layer()
            {
                Key = name,
                Required = required,
                Values = layerBuilder.NamedConditions
            };

            _layers.Add(hierarchy);

            return this;
        }

        public IEnumerable<DisplayModeProfile> Build()
        {
            var weight = _layers.Count;

            foreach (var layer in _layers)
            {
                foreach (var value in layer.Values)
                {
                    value.Weight = weight;
                }
                weight--;
            }

            return _layers
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

        public class LayerBuilder
        {
            private readonly int _weight;

            public List<NamedCondition> NamedConditions = new List<NamedCondition>();

            internal LayerBuilder(int weight)
            {
                _weight = weight;
            }

            public LayerBuilder Suffix(string name, Expression<Func<HttpContextBase, bool>> expression)
            {
                var namedExpression = new NamedCondition
                {
                    Name = name,
                    Expression = expression,
                    Weight = _weight
                };

                NamedConditions.Add(namedExpression);

                return this;
            }
        }
    }
}