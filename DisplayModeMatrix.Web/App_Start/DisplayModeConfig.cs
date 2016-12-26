using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.WebPages;

namespace Hexdigits.DisplayModeMatrix.Web
{
    public class DisplayModeConfig
    {
        public static void Register(DisplayModeProvider instance)
        {
            //instance.Modes.Add(new DefaultDisplayMode("Mobile-Dark-Preview")
            //{
            //    ContextCondition = x => IsMobile(x) && CurrentTheme(x) == "dark" && IsPreview(x)
            //});

            //instance.Modes.Add(new DefaultDisplayMode("Tablet-Dark-Preview")
            //{
            //    ContextCondition = x => IsTablet(x) && CurrentTheme(x) == "dark" && IsPreview(x)
            //});

            //instance.Modes.Add(new DefaultDisplayMode("Mobile-Dark")
            //{
            //    ContextCondition = x => IsMobile(x) && CurrentTheme(x) == "dark"
            //});

            //instance.Modes.Add(new DefaultDisplayMode("Tablet-Dark")
            //{
            //    ContextCondition = x => IsTablet(x) && CurrentTheme(x) == "dark"
            //});

            //instance.Modes.Add(new DefaultDisplayMode("Dark-Preview")
            //{
            //    ContextCondition = x => CurrentTheme(x) == "dark" && IsPreview(x)
            //});

            //instance.Modes.Add(new DefaultDisplayMode("Mobile-Preview")
            //{
            //    ContextCondition = x => IsMobile(x) && IsPreview(x)
            //});

            //instance.Modes.Add(new DefaultDisplayMode("Tablet-Preview")
            //{
            //    ContextCondition = x => IsTablet(x) && IsPreview(x)
            //});

            //instance.Modes.Add(new DefaultDisplayMode("Mobile")
            //{
            //    ContextCondition = x => IsMobile(x)
            //});

            //instance.Modes.Add(new DefaultDisplayMode("Tablet")
            //{
            //    ContextCondition = x => IsTablet(x)
            //});

            //instance.Modes.Add(new DefaultDisplayMode("Dark")
            //{
            //    ContextCondition = x => CurrentTheme(x) == "dark"
            //});

            //instance.Modes.Add(new DefaultDisplayMode("Preview")
            //{
            //    ContextCondition = x => IsPreview(x)
            //});


            var builder = new DisplayModeMatrixBuilder();

            var matrix = builder
                            //.Precondition(x => false)
                            .SetEvaluateBehavior(EvaluateBehavior.Lazy)
                            .AddOptionalFactor("Device", l => l
                                     .Evidence("Tablet", x => IsTablet(x))
                                     .Evidence("Mobile", x => IsMobile(x)))
                            .AddOptionalFactor("Theme", l => l
                                     .Evidence("Dark", x => CurrentTheme(x) == "dark")
                                     .Evidence("Light", x => CurrentTheme(x) == "light"))
                            .AddOptionalFactor("Preview", l => l
                                     .Evidence("Preview", x => IsPreview(x)))
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
            => Regex.IsMatch(x.GetOverriddenUserAgent() ?? "", "iPad|Tablet", RegexOptions.IgnoreCase);

        public static string CurrentTheme(HttpContextBase x) 
            => x.Request.Cookies.AllKeys.Contains("Theme") ? x.Request.Cookies["Theme"].Value : string.Empty;

        public static bool IsPreview(HttpContextBase x) 
            => x.Request.Cookies.AllKeys.Contains("Preview");
    }
}