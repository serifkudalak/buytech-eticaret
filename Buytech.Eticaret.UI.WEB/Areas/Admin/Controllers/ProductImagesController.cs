﻿using System;
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
    public class ProductImagesController : AdminControllerBase
    {
        private BuytechDB db = new BuytechDB();

        // GET: Admin/ProductImages
        public ActionResult Index()
        {
            var productImages = db.ProductImages.Include(p => p.Product).Include(p => p.Product.Category).ToList();
            return View(productImages);
        }

        // GET: Admin/ProductImages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductImage productImage = db.ProductImages.Include(p => p.Product).Where(i => i.ID == id).FirstOrDefault();
            if (productImage == null)
            {
                return HttpNotFound();
            }
            return View(productImage);
        }

        // GET: Admin/ProductImages/Create
        public ActionResult Create()
        {
            ViewBag.ProductID = new SelectList(db.Products, "ID", "Name");
            return View();
        }

        // POST: Admin/ProductImages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ProductID,Image1,Image2,Image3,Image4,Image5,CreateDate,UpdateDate")] ProductImage productImage)
        {
            if (ModelState.IsValid)
            {
                db.ProductImages.Add(productImage);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProductID = new SelectList(db.Products, "ID", "Name", productImage.ProductID);
            return View(productImage);
        }

        // GET: Admin/ProductImages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductImage productImage = db.ProductImages.Find(id);
            if (productImage == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductID = new SelectList(db.Products, "ID", "Name", productImage.ProductID);
            return View(productImage);
        }

        // POST: Admin/ProductImages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ProductID,Image1,Image2,Image3,Image4,Image5,CreateDate,UpdateDate")] ProductImage productImage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productImage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductID = new SelectList(db.Products, "ID", "Name", productImage.ProductID);
            return View(productImage);
        }

        // GET: Admin/ProductImages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductImage productImage = db.ProductImages.Include(p => p.Product).Where(p => p.ID == id).FirstOrDefault();
            if (productImage == null)
            {
                return HttpNotFound();
            }
            return View(productImage);
        }

        // POST: Admin/ProductImages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductImage productImage = db.ProductImages.Find(id);
            db.ProductImages.Remove(productImage);
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
