namespace CarRentalSystem.Migrations
{
    using CarRentalSystem.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<CarRentalSystem.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(CarRentalSystem.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.
            //  This method will be called after migrating to the latest version.
            if (!context.Roles.Any(r => r.Name == RoleName.CanManange))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = RoleName.CanManange };
                manager.Create(role);
            }
            if (!context.Users.Any(u => u.UserName == "admin@admin.com"))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new ApplicationUser { UserName = "admin@admin.com", EmployeeName = "admin" };

                manager.Create(user, "SecretPass1!");
                manager.AddToRole(user.Id, RoleName.CanManange);
            }
           
        }
    }
}
