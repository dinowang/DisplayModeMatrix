using System;
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
                            .AddOptionalFactor("Device", l => l.Evidence("Mobile", x => IsMobile(x)).Evidence("Tablet", x => IsTablet(x)))
                            .AddOptionalFactor("Theme", l => l.Evidence("Dark", x => CurrentTheme(x) == "dark"))
                            .AddOptionalFactor("Preview", l => l.Evidence("Preview", x => IsPreview(x)))
                            .Build();
            
            instance.Modes.Clear();

            foreach (var profile in matrix)
            {
                instance.Modes.Add(new DefaultDisplayMode(profile.Name)
                {
                    ContextCondition = profile.ContextCondition
                });
            }
            
            instance.Modes.Add(new DefaultDisplayMode(""));
        }

        public static bool IsMobile(HttpContextBase x) 
            => x.GetOverriddenBrowser().IsMobileDevice;

        public static bool IsTablet(HttpContextBase x) 
            => Regex.IsMatch(x.GetOverriddenUserAgent(), "iPad|Tablet", RegexOptions.IgnoreCase);

        public static string CurrentTheme(HttpContextBase x) 
            => x.Request.Cookies.AllKeys.Contains("Theme") ? x.Request.Cookies["Theme"].Value : string.Empty;

        public static bool IsPreview(HttpContextBase x) 
            => x.Request.Cookies.AllKeys.Contains("Preview");
    }
}