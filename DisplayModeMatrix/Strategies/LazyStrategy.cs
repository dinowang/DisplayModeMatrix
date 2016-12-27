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

        public Expression<Func<HttpContextBase, bool>> WarpExpression(Expression<Func<HttpContextBase, bool>> expression, ParameterExpression parameter)
        {
            CalculationCaching.BuildTemplate(expression, parameter);

            var evaluatorMethod = typeof(CalculationCaching).GetMethod("Evaluate");

            var body = Expression.Lambda<Func<HttpContextBase, bool>>(Expression.Call(evaluatorMethod, Expression.Constant(expression)), parameter);

            return body;
        }
    }
}