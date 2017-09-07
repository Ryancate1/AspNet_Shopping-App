namespace rcateShoppingApp1.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using rcateShoppingApp1.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<rcateShoppingApp1.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(rcateShoppingApp1.Models.ApplicationDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(context));
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }

            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));
            if (!context.Users.Any(u => u.Email == "ryan.cate@yahoo.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "ryan.cate@yahoo.com",
                    Email = "ryan.cate@yahoo.com",
                    FirstName = "Ryan",
                    LastName = "Cate",
                }, "Password1!");
            }
            var userId = userManager.FindByEmail("ryan.cate@yahoo.com").Id;
            userManager.AddToRole(userId, "Admin");
        }
    }
}
