using Buytech.Eticaret.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;
using Buytech.Eticaret.Core.Model.Entity;
using Buytech.Eticaret.UI.WEB.Controllers.Base;

namespace Buytech.Eticaret.UI.WEB.Controllers
{
    public class CategoryController : BuytechControllerBase
    {
        BuytechDB db = new BuytechDB();

        // GET: Category
        [Route("{categoryName}/{categoryID}")]
        public ActionResult Index(string categoryName, int categoryID, List<string> brand, string minPrice, string maxPrice)
        {
            var products = db.Products.Include(p => p.Category).Where(p => p.CategoryID == categoryID).ToList();
            List<Product> productList = new List<Product>();
            List<Product> tempraryList = products;
            Product product = new Product();

            ViewBag.Title = products.FirstOrDefault().Category.Name;

            ViewBag.Brand = products.Select(p => p.Brand).Distinct();

            if (brand != null && !String.IsNullOrEmpty(minPrice) && !String.IsNullOrEmpty(maxPrice))
            {
                for (int i = 0; i < brand.Count; i++)
                {
                    for (int j = 0; j < brand[i].Length; j++)
                    {
                        foreach (var item in tempraryList.Where(p => p.Brand == brand[i]).ToList())
                        {
                            if (item.Price >= Convert.ToDecimal(minPrice) && item.Price <= Convert.ToDecimal(maxPrice))
                            {
                                productList.Add(item);
                            }
                        }
                    }
                }

                productList = productList.Distinct().ToList();
            }
            else if (brand != null && !String.IsNullOrEmpty(minPrice))
            {
                for (int i = 0; i < brand.Count; i++)
                {
                    for (int j = 0; j < brand[i].Length; j++)
                    {
                        foreach (var item in tempraryList.Where(p => p.Brand == brand[i]).ToList())
                        {
                            if (item.Price >= Convert.ToDecimal(minPrice))
                            {
                                productList.Add(item);
                            }
                        }
                    }
                }

                productList = productList.Distinct().ToList();
            }
            else if (brand != null && !String.IsNullOrEmpty(maxPrice))
            {
                for (int i = 0; i < brand.Count; i++)
                {
                    for (int j = 0; j < brand[i].Length; j++)
                    {
                        foreach (var item in tempraryList.Where(p => p.Brand == brand[i]).ToList())
                        {
                            if (item.Price <= Convert.ToDecimal(maxPrice))
                            {
                                productList.Add(item);
                            }
                        }
                    }
                }

                productList = productList.Distinct().ToList();
            }
            else if (!String.IsNullOrEmpty(minPrice) && !String.IsNullOrEmpty(maxPrice))
            {
                productList = products.Where(p => p.Price >= Convert.ToDecimal(minPrice) && p.Price <= Convert.ToDecimal(maxPrice)).ToList();
            }
            else if (brand != null)
            {
                for (int i = 0; i < brand.Count; i++)
                {
                    for (int j = 0; j < brand[i].Length; j++)
                    {
                        foreach (var item in tempraryList.Where(p => p.Brand == brand[i]).ToList())
                        {
                            productList.Add(item);
                        }
                    }
                }

                productList = productList.Distinct().ToList();
            }
            else if (!String.IsNullOrEmpty(minPrice))
            {
                productList = products.Where(p => p.Price >= Convert.ToDecimal(minPrice)).ToList();
            }
            else if (!String.IsNullOrEmpty(maxPrice))
            {
                productList = products.Where(p => p.Price <= Convert.ToDecimal(maxPrice)).ToList();
            }
            else
            {
                productList = products;
            }

            return View(productList);
        }
    }
}