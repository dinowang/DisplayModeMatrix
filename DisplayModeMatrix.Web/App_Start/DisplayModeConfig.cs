using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.WebPages;

namespace DisplayModeMatrix.Web
{
    public class DisplayModeConfig
    {
        public static void Register(DisplayModeProvider instance)
        {
            var builder = new DisplayModeMatrixBuilder();

            var matrix = builder
                            .AddOptionalLayer("Device", l => l.Suffix("Mobile", x => IsMobile(x)).Suffix("Tablet", x => IsTablet(x)))
                            .AddOptionalLayer("Theme", l => l.Suffix("Dark", x => CurrentTheme(x) == "dark"))
                            .AddOptionalLayer("Preview", l => l.Suffix("Preview", x => IsPreview(x)))
                            .Build();

            foreach (var profile in matrix)
            {
                instance.Modes.Add(new DefaultDisplayMode(profile.Name)
                {
                    ContextCondition = x => profile.ContextCondition(x)
                });
            }
        }

        public static bool IsMobile(HttpContextBase x) => x.GetOverriddenBrowser().IsMobileDevice;

        public static bool IsTablet(HttpContextBase x) => Regex.IsMatch(x.GetOverriddenUserAgent(), "iPad|Tablet", RegexOptions.IgnoreCase);

        public static string CurrentTheme(HttpContextBase x) => x.Request.Cookies["Theme"]?.Value;

        public static bool IsPreview(HttpContextBase x) => x.Request.Cookies["Preview"] != null;
    }
}