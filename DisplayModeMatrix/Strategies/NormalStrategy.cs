﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web;

namespace Hexdigits.DisplayModeMatrix.Strategies
{
    internal class NormalStrategy : IEvaluationStrategy
    {
        public Expression<Func<HttpContextBase, bool>> InitializePadding(Expression<Func<HttpContextBase, bool>> expression, ParameterExpression parameter)
        {
            return expression;
        }

        public Expression<Func<HttpContextBase, bool>> WarpExpression(Expression<Func<HttpContextBase, bool>> expression, ParameterExpression parameter)
        {
            return expression;
        }
    }
}
