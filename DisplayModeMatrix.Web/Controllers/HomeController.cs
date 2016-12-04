using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hexdigits.DisplayModeMatrix.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult ToogleTheme()
        {
            var cookie = Request.Cookies["Theme"];

            if (cookie == null)
            {
                cookie = new HttpCookie("Theme", "dark");
            }
            else
            {
                cookie.Expires = DateTime.Now.AddDays(-1);
            }
            Response.Cookies.Add(cookie);

            return RedirectToAction("Index", "Home");
        }

        public ActionResult TooglePreview()
        {
            var cookie = Request.Cookies["Preview"];

            if (cookie == null)
            {
                cookie = new HttpCookie("Preview", "1");
            }
            else
            {
                cookie.Expires = DateTime.Now.AddDays(-1);
            }
            Response.Cookies.Add(cookie);

            return RedirectToAction("Index", "Home");
        }
    }
}