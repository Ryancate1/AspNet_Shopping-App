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
    [Authorize]
    public class OrdersController : Universal
    {

        // GET: Orders
        public ActionResult Index()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            return View(user.Orders.Where(o => o.Completed == true).ToList());
        }

        // GET: Orders/Details/5
        public ActionResult Details(int? id, bool? justCompleted)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Order.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.JustComplated = false;
            if (justCompleted != null && justCompleted == true)
            {
                ViewBag.JustCompleted = true;
            }
            return View(order);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            var user = db.Users.Find(User.Identity.GetUserId());

            foreach (var order in user.Orders.Where(o => o.Completed == false).ToList())
            {
                db.Order.Remove(order);
                db.SaveChanges();
            }
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Address,City,State,ZipCode,Phone,Total,OrderDate,CustomerId,OrderDetails,CardNumber,CVC,ExpMonth,ExpYear")] Order order)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.Find(User.Identity.GetUserId());
                order.CustomerId = user.Id;
                order.OrderDate = DateTime.Now;
                order.Total = ViewBag.CartTotal;
                order.Completed = false;

                db.Order.Add(order);
                db.SaveChanges();
                return RedirectToAction("Finalize", new { id = order.Id });
            }

            return View(order);
        }
        public ActionResult Finalize(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Order.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }

            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Finalize(int id)
        {
            Order order = db.Order.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            order.Completed = true;
            order.OrderDate = System.DateTime.Now;
            db.SaveChanges();
            var user = db.Users.Find(User.Identity.GetUserId());

            foreach (var cartItem in user.CartItems.ToList())
            {
                OrderItem orderItem = new OrderItem();
                orderItem.ItemId = cartItem.ItemId;
                orderItem.OrderId = id;
                orderItem.Quantity = cartItem.Count;
                orderItem.UnitPrice = cartItem.Item.Price;
                db.OrderItem.Add(orderItem);
                db.CartItems.Remove(cartItem);
                db.SaveChanges();
            }

            return RedirectToAction("Details", new { id = id, justCompleted = true });
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Order.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Address,City,State,ZipCode,Phone,Total,OrderDate,CustomerId,OrderDetails")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(order);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Order.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Order.Find(id);
            db.Order.Remove(order);
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
