using System;
using System.Linq.Expressions;
using System.Web;

namespace Hexdigits.DisplayModeMatrix.Strategies
{
    internal class LazyStrategy : IEvaluationStrategy
    {
        public Expression<Func<HttpContextBase, bool>> InitializePadding(Expression<Func<HttpContextBase, bool>> expression, ParameterExpression parameter)
        {
            var init = typeof(CalculationCaching).GetMethod("EnsureRequestContext");

            var body = Expression.AndAlso(
                            Expression.Call(init),
                            Expression.Invoke(expression, parameter));

            return Expression.Lambda<Func<HttpContextBase, bool>>(body, parameter);
        }

        public Expression Combine(Expression left, Expression right, ParameterExpression parameter)
        {
            CalculationCaching.BuildTemplate(left, parameter);
            CalculationCaching.BuildTemplate(right, parameter);

            var evaluatorMethod = typeof(CalculationCaching).GetMethod("Evaluate");

            var body = Expression.AndAlso(
                            Expression.Call(evaluatorMethod, Expression.Constant(left)),
                            Expression.Call(evaluatorMethod, Expression.Constant(right)));

            return body;
        }
    }
}