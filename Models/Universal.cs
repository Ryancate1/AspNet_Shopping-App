using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using rcateShoppingApp1.Models.CodeFirst;

namespace rcateShoppingApp1.Models
{
    public class Universal : Controller
    {
        public ApplicationDbContext db = new ApplicationDbContext();

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (User.Identity.IsAuthenticated)
            {
                var user = db.Users.Find(User.Identity.GetUserId());
                ViewBag.FirstName = user.FirstName;
                ViewBag.LastName = user.LastName;
                ViewBag.FullName = user.FullName;
               
                var myCart = db.CartItems.AsNoTracking().Where(c => c.CustomerId == user.Id).ToList();
                ViewBag.CartItems = myCart;
                ViewBag.TotalCartItems = myCart.Sum(c => c.Count);
                decimal Total = 0;
                foreach (var item in myCart)
                {
                    Total += item.Count * item.Item.Price;
                }
                ViewBag.CartTotal = Total;
                base.OnActionExecuting(filterContext);
                
            }
        }
    }
}