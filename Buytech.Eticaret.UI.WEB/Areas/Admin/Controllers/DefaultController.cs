using Buytech.Eticaret.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Buytech.Eticaret.UI.WEB.Areas.Admin.Controllers
{
    public class DefaultController : AdminControllerBase
    {
        BuytechDB db = new BuytechDB();
        // GET: Admin/Default
        public ActionResult Index()
        {
            ViewBag.User = db.Users.Count();
            ViewBag.Product = db.Products.Count();
            ViewBag.Order = db.OrderPayments.Count();
            ViewBag.Contact = db.Contacts.Count();

            return View();
        }

        [Route("admin-cikis-yap")]
        public ActionResult SignOut()
        {
            Session["AdminLoginUser"] = null;
            return RedirectToAction("Index", "Admin");
        }
    }
}