using System;
using System.Web;

namespace DisplayModeMatrix
{
    public class DisplayModeProfile
    {
        public string Name { get; internal set; }

        public Func<HttpContextBase, bool> ContextCondition { get; internal set; }
    }
}