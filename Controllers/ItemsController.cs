﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using rcateShoppingApp1.Models;
using rcateShoppingApp1.Models.CodeFirst;
using System.IO;

namespace rcateShoppingApp1.Controllers
{
    public class ItemsController : Universal
    {

        // GET: Items
        public ActionResult Index()
        {
            return View(db.Items.ToList());
        }

        public ActionResult SearchResult(string searchitem)
        {
            return View(db.Items.Where(i => i.Name.Contains(searchitem) || i.Description.Contains(searchitem)).ToList());
        }

        // GET: Items/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // GET: Items/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize (Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CreationDate,UpdatedDate,Name,Price,MediaURL,Description")] Item item, HttpPostedFileBase image)
        {
            if (image != null && image.ContentLength > 0)
            {
                var ext = Path.GetExtension(image.FileName).ToLower();
                if (ext != ".png" && ext != ".jpg" && ext != ".jpeg" && ext != ".gif" && ext != ".bmp")
                {
                    ModelState.AddModelError("Image", "Invalid Format.");
                }
            }
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    var filePath = "/Uploads/"; //relative server path             
                    var absPath = Server.MapPath("~" + filePath);
                    // path on physicaldrive on server                          
                    item.MediaURL = filePath + image.FileName;
                    // media url for relative path                       
                    image.SaveAs(Path.Combine(absPath, image.FileName));
                    //save image
                }
                item.CreationDate = System.DateTime.Now;
                db.Items.Add(item);
                db.SaveChanges();

                return RedirectToAction("Index");

            }
            

            return View(item);
        }

        // GET: Items/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(Id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CreationDate,UpdatedDate,Name,Price,MediaURL,Description")] Item item, string mediaURL,HttpPostedFile image)
        {
            if (image != null && image.ContentLength > 0)
            {
                var ext = Path.GetExtension(image.FileName).ToLower();
                if (ext != ".png" && ext != ".jpg" && ext != ".jpeg" && ext != ".gif" && ext != ".bmp")
                {
                    ModelState.AddModelError("Image", "Invalid Format.");
                }
            }

            if (ModelState.IsValid)
            {
                db.Entry(item).State = EntityState.Modified;
                if (image != null)
                {
                    var filePath = "/assets/images/"; //url path             
                    var absPath = Server.MapPath("~" + filePath); // path on physicaldrive on server
                    item.MediaURL = filePath + image.FileName; // media url for relative path                       
                    image.SaveAs(Path.Combine(absPath, image.FileName)); //save image
                }
                else
                {
                    item.MediaURL = mediaURL;
                }

                
                item.UpdatedDate = System.DateTime.Now;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(item);
        }

        // GET: Items/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(Id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: Items/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Item item = db.Items.Find(id);
            var absPath = Server.MapPath("~" + item.MediaURL);
            System.IO.File.Delete(absPath);
            db.Items.Remove(item);
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
