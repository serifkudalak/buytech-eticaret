using Buytech.Eticaret.Core.Model;
using Buytech.Eticaret.Core.Model.Entity;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Buytech.Eticaret.UI.WEB.Controllers.Base;

namespace Buytech.Eticaret.UI.WEB.Controllers
{
    public class HomeController : BuytechControllerBase
    {
        BuytechDB db = new BuytechDB();
        // GET: Home
        public ActionResult Index()
        {
            ViewBag.IsLogin = this.IsLogin;

            return View();
        }

        [Route("aranan-urunler")]
        public ActionResult Search(string search)
        {
            var products = db.Products.Include(p => p.Category).ToList();

            if (!String.IsNullOrEmpty(search))
            {
                products = products.Where(p => p.Name.ToLower().Contains(search.ToLower())).ToList();
            }

            return View(products);
        }

        public PartialViewResult GetMenu()
        {
            var menus = db.Categories.ToList();

            return PartialView(menus);
        }

        public PartialViewResult UserBasketTotal()
        {
            int id = LoginUserID;

            var basket = db.Baskets.Where(p => p.UserID == id).ToList();

            return PartialView(basket);
        }

        public PartialViewResult OppurtunityProducts()
        {
            var dailyProducts = db.Products.Where(p => p.IsOpportunity == true).Include(p => p.Category).ToList();

            return PartialView(dailyProducts);
        }

        public PartialViewResult RandomProducts()
        {
            Random rnd = new Random();
            var products = db.Products.Include(p => p.Category).ToList();
            List<Product> randomProducts = new List<Product>();
            Product product = new Product();
            var numbers = Enumerable.Range(1, products.Count).OrderBy(p => Guid.NewGuid()).ToList();
            int number;

            if (numbers.Count != 0)
            {

                for (int i = 0; i < 20; i++)
                {
                    number = numbers[i];
                    product = db.Products.Where(p => p.ID == number).FirstOrDefault();

                    if (product != null && product.IsOpportunity != true)
                    {
                        randomProducts.Add(product);

                        if (randomProducts.Count >= 8)
                        {
                            break;
                        }
                    }
                }
            }


            return PartialView(randomProducts);
        }

        [Route("giris-yap")]
        public ActionResult Login()
        {
            return View();
        }

        [Route("giris-yap")]
        [HttpPost]
        public ActionResult Login(string Email, string Password)
        {
            var user = db.Users.Where(x => x.Email == Email && x.Password == Password && x.IsActive == true).ToList();

            if(user.Count == 1)
            {
                Session["LoginUserID"] = user.FirstOrDefault().ID;
                Session["LoginUser"] = user.FirstOrDefault();
                return Redirect("/");
            }
            else
            {
                ViewBag.Err = "Hatalı Parola Bilgisi Girdiniz!";
                return View();
            }
        }

        [Route("cikis-yap")]
        public ActionResult SignOut()
        {
            Session["LoginUser"] = null;
            Session["LoginUserID"] = null;

            return Redirect("/");
        }

        [Route("kayit-ol")]
        public ActionResult CreateUser()
        {
            return View();
        }

        [Route("kayit-ol")]
        [HttpPost]
        public ActionResult CreateUser(User entity)
        {
            try
            {
                Random rnd = new Random();

                entity.Photo = "1.jpg";
                entity.CreateDate = DateTime.Now;
                entity.IsActive = true;
                entity.TransferNumber = entity.NameSurname.ToLower()
                    .Replace("ş", "s")
                    .Replace("ğ","g")
                    .Replace("ö","o")
                    .Replace("ı","i")
                    .Replace("ü","u")
                    .Replace("ç","c")
                    .Split(' ')[0] + "x" + rnd.Next(10000000, 99999999).ToString();

                string folderName = @"C:\Users\elekse\Desktop\Bitirme\Buytech.Eticaret\Buytech.Eticaret.UI.WEB\Content\img\profile";
                string saveFolder = System.IO.Path.Combine(folderName, entity.GlobalNumber);
                System.IO.Directory.CreateDirectory(saveFolder);

                db.Users.Add(entity);
                db.SaveChanges();

                TempData["globalValue"] = entity.GlobalNumber;

                return Redirect("/fotograf-cek");
            }
            catch (Exception e)
            {
                return View();
            }
        }

        [Route("fotograf-cek")]
        public ActionResult Capture()
        {
            return View();
        }

        [HttpPost]
        [Route("fotograf-yakala")]
        public ContentResult SaveCapture(string data)
        {
            string fileName = TempData["globalValue"].ToString();

            //Convert Base64 Encoded string to Byte Array.
            byte[] imageBytes = Convert.FromBase64String(data.Split(',')[1]);

            //Save the Byte Array as Image File.
            string filePath = Server.MapPath(string.Format("~/Content/img/profile/{0}/1.jpg", fileName));
            System.IO.File.WriteAllBytes(filePath, imageBytes);

            return Content("/");
        }

        [HttpPost]
        [Route("klasor-isimleri")]
        public JsonResult FolderName()
        {
            var globalNumberList = db.Users.ToList();
            List<string> globalNumbers = new List<string>();

            foreach (var item in globalNumberList)
            {
                globalNumbers.Add(item.GlobalNumber);
            }

            return Json(globalNumbers, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Route("email-bilgileri")]
        public JsonResult EmailDetail(string globalNumber)
        {
            var globalNumberList = db.Users.Where(x => x.GlobalNumber == globalNumber).ToList();
            var globalNumberDetail = globalNumberList.FirstOrDefault().Email;

            return Json(globalNumberDetail, JsonRequestBehavior.AllowGet);
        }

        [Route("kullanici/profilim/bilgiler/{id}")]
        public ActionResult MyProfile(int id)
        {
            var user = db.Users.Where(p => p.ID == id).FirstOrDefault();

            return View(user);
        }

        [HttpPost]
        [Route("kullanici/profilim/bilgiler/{id}")]
        public ActionResult UpdateCustomer(int id, User entity)
        {

            User updatedUser = db.Users.Where(p => p.ID == entity.ID).FirstOrDefault();

            if (updatedUser != null)
            {
                updatedUser.NameSurname = entity.NameSurname;
                updatedUser.Email = entity.Email;
                updatedUser.Town = entity.Town;
                updatedUser.City = entity.City;
                updatedUser.Address = entity.Address;
                updatedUser.PostalCode = entity.PostalCode;
                updatedUser.Phone = entity.Phone;
                updatedUser.Password = entity.Password;

                db.SaveChanges();
                return Json(true);
            }

            return Json(false);
        }

        [Route("para-transferi/a/{transferNumber}/{id}")]
        public ActionResult MoneyTransfer(string transferNumber, int id)
        {
            var user = db.Users.Where(p => p.ID == id).FirstOrDefault();

            return View(user);
        }

        [HttpPost]
        [Route("para-transferi/a/{transferNumber}/{id}")]
        public ActionResult Transfer(string transferNumber, int id, string userTransferNumber, decimal price)
        {
            var increaseUser = db.Users.Where(p => p.TransferNumber == userTransferNumber).FirstOrDefault();
            var decreaseUser = db.Users.Where(p => p.ID == id).FirstOrDefault();

            if(increaseUser != null)
            {
                increaseUser.Balance += price;
                decreaseUser.Balance -= price;

                db.SaveChanges();

                return Json(true);
            }

            return Json(false);
        }

        [Route("profilim/bakiye-yukle/d/{transferNumber}/{id}")]
        public ActionResult LoadMoney(string transferNumber, int id)
        {
            var user = db.Users.Where(p => p.ID == id).FirstOrDefault();

            return View(user);
        }

        [Route("hakkimizda")]
        public ActionResult About()
        {
            return View();
        }

        [Route("gizlilik-politikasi")]
        public ActionResult PrivacyPolicy()
        {
            return View();
        }

        [Route("para-transferi-hakkinda")]
        public ActionResult MoneyTransferAbout()
        {
            return View();
        }

        [Route("iletisim")]
        public ActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        [Route("iletisim")]
        public ActionResult Contact(Contact contact)
        {
            db.Contacts.Add(new Contact
            {
                NameSurname = contact.NameSurname,
                Email = contact.Email,
                Phone = contact.Phone,
                Subject = contact.Subject,
                Message = contact.Message,
                CreateDate = DateTime.Now
            });

            db.SaveChanges();

            return Json(true);
        }
    }
}