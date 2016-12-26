using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hexdigits.DisplayModeMatrix.Strategies
{
    internal static class EvaluateStrategyFactory
    {
        public static IEvaluationStrategy Create(EvaluateBehavior behavior)
        {
            switch (behavior)
            {
                case EvaluateBehavior.Lazy:
                    return new LazyStrategy();
                default:
                    return new NormalStrategy();
            }
        }
    }
}
