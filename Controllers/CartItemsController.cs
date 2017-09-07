using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using rcateShoppingApp1.Models;
using rcateShoppingApp1.Models.CodeFirst;
using Microsoft.AspNet.Identity;

namespace rcateShoppingApp1.Controllers
{
    
    public class CartItemsController : Universal
    {

        // GET: CartItems
        [Authorize]
        public ActionResult Index()
        {
                var user = db.Users.Find(User.Identity.GetUserId());
                return View(user.CartItems.ToList());
        }

        // GET: CartItems/Details/5
        public ActionResult Details(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CartItem cartItem = db.CartItems.Find(id);
            if (cartItem == null)
            {
                return HttpNotFound();
            }
            return View(cartItem);
        }

        // GET: CartItems/Create
        public ActionResult Create()
        {
            if (Request.IsAuthenticated)
            {
            return View();
            }
            return View();
        }

        // POST: CartItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create (int? itemId)
        {
            if (itemId != null)
            {
                var user = db.Users.Find(User.Identity.GetUserId());
                if (user == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                if (db.CartItems.Where(c => c.CustomerId == user.Id).Any(c => c.ItemId == itemId))
                {
                    var existingItem = db.CartItems.Where(c => c.CustomerId == user.Id).FirstOrDefault(c => c.ItemId == itemId);
                    existingItem.Count += 1;
                    db.SaveChanges();
                }
                else
                {
                    CartItem cartItem = new CartItem();
                    cartItem.ItemId = (int)itemId;
                    cartItem.CustomerId = user.Id;
                    cartItem.Count = 1;
                    cartItem.CreationDate = DateTime.Now;
                    cartItem.Price = cartItem.Price;
                    db.CartItems.Add(cartItem);
                    db.SaveChanges();
                }
                
                return RedirectToAction("Index");
            }

            return View();


        }



    // GET: CartItems/Edit/5
    //[Authorize(Roles = "Admin")]
    public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CartItem cartItem = db.CartItems.Find(id);
            if (cartItem == null)
            {
                return HttpNotFound();
            }
            return View(cartItem);
        }

        // POST: CartItems/Edit/5
        //[Authorize(Roles = "Admin")]
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ItemId,Count,CreationDate,CustomerId")] CartItem cartItem)
        {
            //db.Entry(cartItem).State = EntityState.Detached;
            if (ModelState.IsValid)
            {

                db.Entry(cartItem).State = EntityState.Modified;

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cartItem);
        }

        // GET: CartItems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CartItem cartItem = db.CartItems.Find(id);
            if (cartItem == null)
            {
                return HttpNotFound();
            }
            return View(cartItem);
        }

        // POST: CartItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CartItem cartItem = db.CartItems.Find(id);
            db.CartItems.Remove(cartItem);
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
