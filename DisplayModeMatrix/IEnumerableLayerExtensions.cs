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
            var level = source.First();
            var next = source.Skip(1);

            if (next.Any())
            {
                foreach (var t in level.Values)
                {
                    foreach (var x in next.Permutation())
                    {
                        if (x == null)
                        {
                            yield return t;
                        }
                        else
                        {
                            var parameter = Expression.Parameter(typeof(HttpContextBase), "x");
                            var body = Expression.AndAlso(
                                            Expression.Invoke(t.Expression, parameter), 
                                            Expression.Invoke(x.Expression, parameter));

                            yield return new NamedCondition
                            {
                                Name = $"{t.Name}-{x.Name}",
                                Expression = Expression.Lambda<Func<HttpContextBase, bool>>(body, parameter),
                                Weight = t.Weight + x.Weight + 0.1
                            };
                        }
                    }
                    if (level.Required == false)
                    {
                        foreach (var x in next.Permutation())
                        {
                            yield return x;
                        }
                    }
                }
            }
            else
            {
                foreach (var x in level.Values)
                {
                    yield return x;
                }

                if (level.Required == false)
                {
                    yield return null;
                }
            }
        }
    }
}