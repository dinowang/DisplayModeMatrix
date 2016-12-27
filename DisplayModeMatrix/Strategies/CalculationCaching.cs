using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;

namespace Hexdigits.DisplayModeMatrix.Strategies
{
    internal class CalculationCaching
    {
        public static bool EnsureRequestContext()
        {
            if (CacheData == null)
            {
                CacheData = new Dictionary<Expression, Lazy<bool>>();
            }

            var dict = CacheData;
            var httpContextBase = new HttpContextWrapper(HttpContext.Current);

            foreach (var template in Templates)
            {
                dict[template.Key] = new Lazy<bool>(() => template.Value(httpContextBase));
            }

            return true;
        }

        public static Dictionary<Expression, Func<HttpContextBase, bool>> Templates = new Dictionary<Expression, Func<HttpContextBase, bool>>();

        public static void BuildTemplate(Expression expr, ParameterExpression parameter)
        {
            if (!Templates.ContainsKey(expr))
            {
                var httpContextCurrent = Expression.Parameter(typeof(HttpContextBase), "httpContextBase");
                var body = Expression.Invoke(expr, httpContextCurrent);
                var valueFactory = Expression.Lambda<Func<HttpContextBase, bool>>(body, httpContextCurrent).Compile();

                Templates[expr] = valueFactory;
            }
        }


        public static Dictionary<Expression, Lazy<bool>> CacheData
        {
            get
            {
                return HttpContext.Current.Items[typeof(CalculationCaching)] as Dictionary<Expression, Lazy<bool>>;
            }
            private set
            {
                HttpContext.Current.Items[typeof(CalculationCaching)] = value;
            }
        }

        public static bool Evaluate(Expression<Func<HttpContextBase, bool>> expr) => CacheData[expr].Value;
    }
}
