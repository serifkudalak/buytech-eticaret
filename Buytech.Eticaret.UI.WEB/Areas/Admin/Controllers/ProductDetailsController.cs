using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Buytech.Eticaret.Core.Model;
using Buytech.Eticaret.Core.Model.Entity;

namespace Buytech.Eticaret.UI.WEB.Areas.Admin.Controllers
{
    public class ProductDetailsController : AdminControllerBase
    {
        private BuytechDB db = new BuytechDB();

        // GET: Admin/ProductDetails
        public ActionResult Index()
        {
            var productDetails = db.ProductDetails.Include(p => p.Product);
            return View(productDetails.ToList());
        }

        // GET: Admin/ProductDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductDetail productDetail = db.ProductDetails.Include(p => p.Product).Where(p => p.ID == id).FirstOrDefault();
            if (productDetail == null)
            {
                return HttpNotFound();
            }
            return View(productDetail);
        }

        // GET: Admin/ProductDetails/Create
        public ActionResult Create()
        {
            ViewBag.ProductID = new SelectList(db.Products, "ID", "Name");
            return View();
        }

        // POST: Admin/ProductDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ProductID,DetailTitle1,DetailTitle2,DetailTitle3,DetailTitle4,DetailTitle5,DetailTitle6,DetailTitle7,DetailTitle8,DetailTitle9,DetailTitle10,DetailTitle11,DetailTitle12,DetailTitle13,DetailTitle14,DetailTitle15,DetailTitle16,Detail1,Detail2,Detail3,Detail4,Detail5,Detail6,Detail7,Detail8,Detail9,Detail10,Detail11,Detail12,Detail13,Detail14,Detail15,Detail16,CreateDate,UpdateDate")] ProductDetail productDetail)
        {
            if (ModelState.IsValid)
            {
                db.ProductDetails.Add(productDetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProductID = new SelectList(db.Products, "ID", "Name", productDetail.ProductID);
            return View(productDetail);
        }

        // GET: Admin/ProductDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductDetail productDetail = db.ProductDetails.Find(id);
            if (productDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductID = new SelectList(db.Products, "ID", "Name", productDetail.ProductID);
            return View(productDetail);
        }

        // POST: Admin/ProductDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ProductID,DetailTitle1,DetailTitle2,DetailTitle3,DetailTitle4,DetailTitle5,DetailTitle6,DetailTitle7,DetailTitle8,DetailTitle9,DetailTitle10,DetailTitle11,DetailTitle12,DetailTitle13,DetailTitle14,DetailTitle15,DetailTitle16,Detail1,Detail2,Detail3,Detail4,Detail5,Detail6,Detail7,Detail8,Detail9,Detail10,Detail11,Detail12,Detail13,Detail14,Detail15,Detail16,CreateDate,UpdateDate")] ProductDetail productDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductID = new SelectList(db.Products, "ID", "Name", productDetail.ProductID);
            return View(productDetail);
        }

        // GET: Admin/ProductDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductDetail productDetail = db.ProductDetails.Include(p => p.Product).Where(p => p.ID == id).FirstOrDefault();
            if (productDetail == null)
            {
                return HttpNotFound();
            }
            return View(productDetail);
        }

        // POST: Admin/ProductDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductDetail productDetail = db.ProductDetails.Find(id);
            db.ProductDetails.Remove(productDetail);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
