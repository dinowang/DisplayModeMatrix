using System;
using System.Linq.Expressions;
using System.Web;

namespace Hexdigits.DisplayModeMatrix.Strategies
{
    internal interface IEvaluationStrategy
    {
        Expression<Func<HttpContextBase, bool>> InitializePadding(Expression<Func<HttpContextBase, bool>> expression, ParameterExpression parameter);

        Expression<Func<HttpContextBase, bool>> WarpExpression(Expression<Func<HttpContextBase, bool>> expression, ParameterExpression parameter);
    }
}