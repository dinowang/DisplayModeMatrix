using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using Hexdigits.DisplayModeMatrix.Strategies;

namespace Hexdigits.DisplayModeMatrix
{
    internal static class IEnumerableFactorExtensions
    {
        internal static IEnumerable<Evidence> Permutation(this IEnumerable<Factor> source, IEvaluationStrategy strategy)
        {
            var layer = source.First();
            var nextLayers = source.Skip(1);

            if (nextLayers.Any())
            {
                foreach (var set in layer.Values)
                {
                    foreach (var childSet in nextLayers.Permutation(strategy))
                    {
                        if (childSet == null)
                        {
                            yield return set;
                        }
                        else
                        {
                            var parameter = Expression.Parameter(typeof(HttpContextBase), "x");
                            var body = strategy.Combine(set.Expression, childSet.Expression, parameter);

                            yield return new Evidence
                            {
                                Name = $"{set.Name}-{childSet.Name}",
                                Expression = Expression.Lambda<Func<HttpContextBase, bool>>(body, parameter),
                                Weight = set.Weight + childSet.Weight + 0.1
                            };
                        }
                    }
                    if (layer.Required == false)
                    {
                        foreach (var x in nextLayers.Permutation(strategy))
                        {
                            yield return x;
                        }
                    }
                }
            }
            else
            {
                foreach (var set in layer.Values)
                {
                    yield return set;
                }

                if (layer.Required == false)
                {
                    yield return null;
                }
            }
        }
    }
}