using Buytech.Eticaret.Core.Model;
using Buytech.Eticaret.Core.Model.Entity;
using Buytech.Eticaret.UI.WEB.Controllers.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;

namespace Buytech.Eticaret.UI.WEB.Controllers
{
    public class BasketController : BuytechControllerBase
    {
        BuytechDB db = new BuytechDB();
        UserBasketAllDetail userBasketAllDetail = new UserBasketAllDetail();

        // GET: Product

        [Route("sepetim")]
        public ActionResult Index()
        {
            userBasketAllDetail.Baskets = db.Baskets.Include(p => p.Product).Include(p => p.Product.Category).Where(p => p.UserID == LoginUserID).ToList();
            userBasketAllDetail.Users = db.Users.Where(p => p.ID == LoginUserID).FirstOrDefault();

            ViewBag.Count = userBasketAllDetail.Baskets.Count();

            return View(userBasketAllDetail);
        }

        [HttpPost]
        [Route("sepetim")]
        public ActionResult BasketDelete(int productID)
        {
            var basketProduct = db.Baskets.Where(p => p.ID == productID).FirstOrDefault();
            var product = db.Products.Where(p => p.ID == basketProduct.ProductID).FirstOrDefault();

            int quantity = basketProduct.Quantity;

            db.Baskets.Remove(basketProduct);

            product.Stock += quantity;

            db.SaveChanges();

            return Json(true);
        }

        [Route("bakiye-ile-odeme/b/s/siparis-olustur")]
        public ActionResult BalanceOrder()
        {
            userBasketAllDetail.Baskets = db.Baskets.Include(p => p.Product).Include(p => p.Product.Category).Where(p => p.UserID == LoginUserID).ToList();
            userBasketAllDetail.Users = db.Users.Where(p => p.ID == LoginUserID).FirstOrDefault();

            return View(userBasketAllDetail);
        }
        
        [HttpPost]
        [Route("bakiye-ile-odeme/b/s/siparis-olustur")]
        public ActionResult BalanceOrder(decimal totalPrice)
        {
            var basket = db.Baskets.Include(p => p.Product).Where(p => p.UserID == LoginUserID).ToList();
            Order order = new Order();
            order.CreateDate = DateTime.Now;
            order.StatusID = 1;
            order.TotalProductPrice = basket.Sum(p => p.Product.Price);
            order.TotalTaxPrice = basket.Sum(p => p.Product.Tax);
            order.TotalDiscount = basket.Sum(p => p.Product.Discount);
            order.TotalPrice = totalPrice;
            order.UserID = LoginUserID;
            order.OrderProducts = new List<OrderProduct>();

            foreach (var item in basket)
            {
                order.OrderProducts.Add(new OrderProduct
                {
                    CreateDate = DateTime.Now,
                    ProductID = item.ProductID,
                    Quantity = item.Quantity
                });
            }

            db.Orders.Add(order);
            db.SaveChanges();

            return Json(true);
        }

        [Route("kredi-karti-ile-odeme/c/s/siparis-olustur")]
        public ActionResult CreditCartOrder()
        {
            userBasketAllDetail.Baskets = db.Baskets.Include(p => p.Product).Include(p => p.Product.Category).Where(p => p.UserID == LoginUserID).ToList();
            userBasketAllDetail.Users = db.Users.Where(p => p.ID == LoginUserID).FirstOrDefault();

            return View(userBasketAllDetail);
        }

        [HttpPost]
        [Route("kredi-karti-ile-odeme/c/s/siparis-olustur")]
        public ActionResult CreditCartOrder(decimal totalPrice)
        {
            var basket = db.Baskets.Include(p => p.Product).Where(p => p.UserID == LoginUserID).ToList();
            Order order = new Order();
            order.CreateDate = DateTime.Now;
            order.StatusID = 1;
            order.TotalProductPrice = basket.Sum(p => p.Product.Price);
            order.TotalTaxPrice = basket.Sum(p => p.Product.Tax);
            order.TotalDiscount = basket.Sum(p => p.Product.Discount);
            order.TotalPrice = totalPrice;
            order.UserID = LoginUserID;
            order.OrderProducts = new List<OrderProduct>();

            foreach (var item in basket)
            {
                order.OrderProducts.Add(new OrderProduct
                {
                    CreateDate = DateTime.Now,
                    ProductID = item.ProductID,
                    Quantity = item.Quantity
                });
            }

            db.Orders.Add(order);
            db.SaveChanges();

            return Json(true);
        }

        [Route("bakiye-ile-odeme/b/s/siparis-olustur/odeme-tamamla")]
        public ActionResult BalancePayment()
        {
            userBasketAllDetail.Baskets = db.Baskets.Include(p => p.Product).Include(p => p.Product.Category).Where(p => p.UserID == LoginUserID).ToList();
            userBasketAllDetail.Users = db.Users.Where(p => p.ID == LoginUserID).FirstOrDefault();

            return View(userBasketAllDetail);
        }

        [HttpPost]
        [Route("bakiye-ile-odeme/b/s/siparis-olustur/odeme-tamamla")]
        public ActionResult BalancePayment(decimal totalPrice)
        {
            var order = db.Orders.Where(p => p.UserID == LoginUserID).OrderByDescending(p => p.ID).FirstOrDefault();
            var user = db.Users.Where(p => p.ID == LoginUserID).FirstOrDefault();
            var baskets = db.Baskets.Where(p => p.UserID == LoginUserID).ToList();

            db.OrderPayments.Add(new OrderPayment
            {
                OrderID = order.ID,
                OrderType = (_OrderType)2,
                Price = totalPrice,
                Bank = "",
                CreateDate = DateTime.Now
            });

            user.Balance -= totalPrice;

            foreach (var item in baskets)
            {
                var basket = db.Baskets.Where(p => p.ID == item.ID).FirstOrDefault();

                db.Baskets.Remove(basket);
            }

            db.SaveChanges();
           
            return Json(true);
        }

        [Route("siparislerim")]
        public ActionResult MyOrders()
        {
            var order = db.Orders.Where(p => p.UserID == LoginUserID).Include(p => p.OrderPayments).Include(p => p.Status).Include(p => p.User).ToList();
            var orderPayments = db.OrderPayments.ToList();
            int resultValue = 0;

            if(orderPayments.Count != 0)
            {
                foreach (var item in orderPayments)
                {
                    foreach (var item2 in order)
                    {
                        if(item.OrderID == item2.ID)
                        {
                            resultValue++;
                        }
                    }
                }
            }

            ViewBag.Result = resultValue;

            return View(order);
        }
    }
}