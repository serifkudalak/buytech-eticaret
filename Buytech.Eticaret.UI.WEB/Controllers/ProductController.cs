using Buytech.Eticaret.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Buytech.Eticaret.Core.Model.Entity;
using System.Data.Entity;
using Buytech.Eticaret.UI.WEB.Controllers.Base;

namespace Buytech.Eticaret.UI.WEB.Controllers
{
    public class ProductController : BuytechControllerBase
    {
        ProductAllDetails productAllDetails = new ProductAllDetails();
        BuytechDB db = new BuytechDB();

        // GET: Product
        [Route("{categoryName}/{modelName}/{id}")]
        public ActionResult Detail(string categoryName, string modelName, int id)
        {
            productAllDetails.Products = db.Products.Where(p => p.ID == id).Include(p => p.Category).FirstOrDefault();
            productAllDetails.ProductImages = db.ProductImages.Where(p => p.ProductID == id).ToList();
            productAllDetails.ProductComments = db.ProductComments.Where(p => p.ProductID == id).Include(p => p.User).ToList();
            productAllDetails.ProductDetails = db.ProductDetails.Where(p => p.ProductID == id).FirstOrDefault();

            var products = db.Products.Include(p => p.Category).Where(p => p.Category.Name.Replace(" ", "-").ToLower() == categoryName && p.ID != id).ToList();
            List<Product> randomProducts = new List<Product>();
            Product product = new Product();
            var numbers = Enumerable.Range(0, products.Count).OrderBy(p => Guid.NewGuid()).ToList();
            int number;
            int[] productIds = new int[numbers.Count];

            for (int i = 0; i < productIds.Length; i++)
            {
                productIds[i] = products[i].ID;
            }

            for (int i = 0; i < numbers.Count; i++)
            {
                number = productIds[numbers[i]];
                product = db.Products.Where(p => p.ID == number).FirstOrDefault();

                if (product != null)
                {
                    randomProducts.Add(product);

                    if (randomProducts.Count >= 3)
                    {
                        break;
                    }
                }
            }

            productAllDetails.AllProducts = randomProducts;


            return View(productAllDetails);
        }

        [HttpPost]
        [Route("{categoryName}/{modelName}/{id}")]
        public ActionResult AddBasket(string categoryName, string modelName, int id, int quantity)
        {
            db.Baskets.Add(new Basket
            {
                UserID = LoginUserID,
                ProductID = id,
                Quantity = quantity,
                CreateDate = DateTime.Now
            });

            var product = db.Products.Where(p => p.ID == id).FirstOrDefault();
            product.Stock -= quantity;

            db.SaveChanges();

            return Json(true);
        }

        [Route("{categoryName}/{modelName}/{categoryNameFirstLetter}/p/{id}")]
        public ActionResult UserProductComment(string categoryName, string modelName, string categoryNameFirstLetter, int id)
        {
            var product = db.Products.Include(p => p.Category).Where(p => p.ID == id).FirstOrDefault();

            return View(product);
        }

        [HttpPost]
        [Route("{categoryName}/{modelName}/{categoryNameFirstLetter}/p/{id}")]
        public ActionResult ProductComment(string categoryName, string modelName, int id, ProductComment productComment)
        {
            db.ProductComments.Add(new ProductComment
            {
                ProductID = id,
                UserID = LoginUserID,
                Rating = productComment.Rating,
                Comment = productComment.Comment,
                Date = DateTime.Now
            });

            db.SaveChanges();

            return Json(true);
        }
    }
}