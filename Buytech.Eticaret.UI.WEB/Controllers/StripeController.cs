using Buytech.Eticaret.Core.Model;
using Buytech.Eticaret.Core.Model.Entity;
using Buytech.Eticaret.UI.WEB.Controllers.Base;
using Stripe;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Buytech.Eticaret.UI.WEB.Controllers
{
    public class StripeController : BuytechControllerBase
    {
        BuytechDB db = new BuytechDB();

        // GET: Stripe
        [Route("siparis-odeme-tamamla/m/b/c/{balance}")]
        public ActionResult Index(decimal balance)
        {
            TempData["balance"] = balance;

            // Set your secret key. Remember to switch to your live secret key in production.
            // See your keys here: https://dashboard.stripe.com/apikeys
            StripeConfiguration.ApiKey = "sk_test_51Kae7mGFvunKEL77X7MN1HnHxzkrVmICLYAIQwb5hojSxOdpwL4WUvKm2cbA3R4WtqwSpoF5lCLRmKtnddZJ0P0900UfwvSnW1";

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
                {
                "card",
                },
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        Name = "Sipariş Ödeme Tamamla",
                        Amount = (long)Convert.ToDouble(balance.ToString() + "00"),
                        Quantity = 1,
                        Currency = "try"
                    },
                },

                SuccessUrl = "https://localhost:44399/siparis-islem-basarili",
                CancelUrl = "https://localhost:44399/siparis-odeme-tamamla/m/b/c/" + ((long)balance).ToString(),

                PaymentIntentData = new SessionPaymentIntentDataOptions
                {
                    Metadata = new Dictionary<string, string>
                    {
                        { "Order_ID", "1234567" },
                        { "Description", "Sipariş Ödeme işlemi" }
                    }
                }
            };

            var service = new SessionService();
            var session = service.Create(options);

            return View(session);
        }

        [Route("bakiye-odeme-tamamla/m/b/c/{balance}")]
        public ActionResult StripeBalancePayment(decimal balance)
        {
            TempData["balance"] = balance;

            // Set your secret key. Remember to switch to your live secret key in production.
            // See your keys here: https://dashboard.stripe.com/apikeys
            StripeConfiguration.ApiKey = "sk_test_51Kae7mGFvunKEL77X7MN1HnHxzkrVmICLYAIQwb5hojSxOdpwL4WUvKm2cbA3R4WtqwSpoF5lCLRmKtnddZJ0P0900UfwvSnW1";

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
                {
                "card",
                },
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        Name = "Bakiye Ödeme Tamamla",
                        Amount = (long)Convert.ToDouble(balance.ToString() + "00"),
                        Quantity = 1,
                        Currency = "try"
                    },
                },

                SuccessUrl = "https://localhost:44399/bakiye-islem-basarili",
                CancelUrl = "https://localhost:44399/bakiye-odeme-tamamla/m/b/c/" + ((long)balance).ToString(),

                PaymentIntentData = new SessionPaymentIntentDataOptions
                {
                    Metadata = new Dictionary<string, string>
                    {
                        { "Order_ID", "1234568" },
                        { "Description", "Bakiye Ödeme işlemi" }
                    }
                }
            };

            var service = new SessionService();
            var session = service.Create(options);

            return View(session);
        }

        [Route("siparis-islem-basarili")]
        public ActionResult SuccessPage()
        {
            var order = db.Orders.Where(p => p.UserID == LoginUserID).OrderByDescending(p => p.ID).FirstOrDefault();
            var user = db.Users.Where(p => p.ID == LoginUserID).FirstOrDefault();
            var baskets = db.Baskets.Where(p => p.UserID == LoginUserID).ToList();
            var totalPrice = Convert.ToDecimal(TempData["balance"]);

            db.OrderPayments.Add(new OrderPayment
            {
                OrderID = order.ID,
                OrderType = (_OrderType)1,
                Price = totalPrice,
                Bank = "Test",
                CreateDate = DateTime.Now
            });

            foreach (var item in baskets)
            {
                var basket = db.Baskets.Where(p => p.ID == item.ID).FirstOrDefault();

                db.Baskets.Remove(basket);
            }

            db.SaveChanges();

            return View();
        }

        [Route("bakiye-islem-basarili")]
        public ActionResult BalanceSuccessPage()
        {
            var user = db.Users.Where(p => p.ID == LoginUserID).FirstOrDefault();
            var value = Convert.ToDecimal(TempData["balance"]);
            user.Balance += value;

            db.SaveChanges();

            return View();
        }
    }
}