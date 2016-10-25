using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace DisplayModeMatrix
{
    public static class IEnumerableLayerExtensions
    {
        public static IEnumerable<NamedCondition> Permutation(this IEnumerable<Layer> source)
        {
            var layer = source.First();
            var nextLayers = source.Skip(1);

            if (nextLayers.Any())
            {
                foreach (var set in layer.Values)
                {
                    foreach (var childSet in nextLayers.Permutation())
                    {
                        if (childSet == null)
                        {
                            yield return set;
                        }
                        else
                        {
                            var parameter = Expression.Parameter(typeof(HttpContextBase), "x");
                            var body = Expression.AndAlso(
                                            Expression.Invoke(set.Expression, parameter), 
                                            Expression.Invoke(childSet.Expression, parameter));

                            yield return new NamedCondition
                            {
                                Name = $"{set.Name}-{childSet.Name}",
                                Expression = Expression.Lambda<Func<HttpContextBase, bool>>(body, parameter),
                                Weight = set.Weight + childSet.Weight + 0.1
                            };
                        }
                    }
                    if (layer.Required == false)
                    {
                        foreach (var x in nextLayers.Permutation())
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