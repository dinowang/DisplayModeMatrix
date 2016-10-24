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
            var hierarchies = new[]
            {
                Hierarchy.Create("Device", false, new []
                {
                    new NamedCondition { Name = "Mobile", Expression = x => IsMobile(x), Weight = 3 },
                    new NamedCondition { Name = "Tablet", Expression = x => IsTablet(x), Weight = 3 }
                }),
                Hierarchy.Create("Theme", false, new []
                {
                    new NamedCondition { Name = "Dark", Expression = x => CurrentTheme(x) == "dark", Weight = 2 },
                }),
                Hierarchy.Create("Preview", false, new []
                {
                    new NamedCondition { Name = "Preview", Expression = x => IsPreview(x), Weight = 1 },
                }),
            };

            foreach (var permutation in hierarchies.Permutation().OrderByDescending(x => x.Weight).Distinct())
            {
                instance.Modes.Add(new DefaultDisplayMode(permutation.Name)
                {
                    ContextCondition = x => permutation.Expression.Compile().Invoke(x)
                });

                Debug.WriteLine($"{permutation.Name}, {permutation.Weight} => {permutation.Expression}");
            }
        }

        public static bool IsMobile(HttpContextBase x) => x.GetOverriddenBrowser().IsMobileDevice;

        public static bool IsTablet(HttpContextBase x) => Regex.IsMatch(x.GetOverriddenUserAgent(), "iPad|Tablet", RegexOptions.IgnoreCase);

        public static string CurrentTheme(HttpContextBase x) => x.Request.Cookies["Theme"]?.Value;

        public static bool IsPreview(HttpContextBase x) => x.Request.Cookies["Preview"] != null;
    }
}