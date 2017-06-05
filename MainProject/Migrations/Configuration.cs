namespace MainProject.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MainProject.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(MainProject.Models.ApplicationDbContext context)
        {
            var roles = new[]
        {
            "Admin",
            "Parent",
            "Child"
        };

            // USE THE FOLLOWING PATTERN TO ADD DEFAULT USERS TO YOUR SYSTEM
            // ROLES CAN BE COMMA SEPERATED TO ADD MULTIPLE ROLES
            // ROLES PROVIDED MUST EXIST IN THE LIST ABOVE
            var users = new[]
            {
            new {Email = "admin@mail.uc.edu", Pwd = "Password.123", Roles = "Admin"},
            new {Email = "parent1@mail.uc.edu", Pwd = "Password.123", Roles = "Parent"},
            new {Email = "parent2@mail.uc.edu", Pwd = "Password.123", Roles = "Parent"},
            new {Email = "child1p1@mail.uc.edu", Pwd = "Password.123", Roles = "Child"},
            new {Email = "child2p1@mail.uc.edu", Pwd = "Password.123", Roles = "Child"},
            new {Email = "child1p2@mail.uc.edu", Pwd = "Password.123", Roles = "Child"},
            new {Email = "child2p2@mail.uc.edu", Pwd = "Password.123", Roles = "Child"},
        };

            // DO NOT MODIFY THE CODE BELOW THIS LINE
            roles.ToList().ForEach(r => context.Roles.AddOrUpdate(x => x.Name,
                new Microsoft.AspNet.Identity.EntityFramework.IdentityRole { Id = Guid.NewGuid().ToString(), Name = r }));
            foreach (var user in users)
            {
                ApplicationUserManager mgr = new ApplicationUserManager(
                    new Microsoft.AspNet.Identity.EntityFramework.UserStore<Models.ApplicationUser>(context));
                Models.ApplicationUser existingUser = context.Users.FirstOrDefault(x => x.UserName == user.Email);
                if (existingUser != null) Microsoft.AspNet.Identity.UserManagerExtensions.Delete(mgr, existingUser);
                Models.ApplicationUser au = new Models.ApplicationUser { Email = user.Email, UserName = user.Email };
                var result = mgr.CreateAsync(au, user.Pwd).Result;
                if (!string.IsNullOrEmpty(user.Roles))
                    Microsoft.AspNet.Identity.UserManagerExtensions.AddToRoles(mgr, au.Id, user.Roles.Split(','));
            }
        }
    }
}
