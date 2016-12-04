using System;
using System.Web;

namespace Hexdigits.DisplayModeMatrix
{
    public class DisplayModeProfile
    {
        public string Name { get; internal set; }

        public Func<HttpContextBase, bool> ContextCondition { get; internal set; }
    }
}