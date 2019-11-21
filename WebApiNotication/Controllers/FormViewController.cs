using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApiProxy.Controllers
{
    public class FormViewController : Controller
    {
        // GET: FormView
        public ActionResult Index(string qs, string uri)
        {
            ViewBag.url = (uri + "?qx=" +  HttpUtility.UrlEncode(qs));
            return View();
        }
    }
}