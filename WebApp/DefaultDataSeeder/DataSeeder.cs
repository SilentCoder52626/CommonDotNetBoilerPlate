using DomainModule.Entity;
using InfrastructureModule.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace WebApp.DefaultDataSeeder
{
    public class DataSeeder
    {
  
        public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();
                context.Database.EnsureCreated();
                var RoleSuperAdmin = User.TypeSuperAdmin;
                //Roles
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(RoleSuperAdmin))
                    await roleManager.CreateAsync(new IdentityRole(RoleSuperAdmin));
               
                //Users
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<User>>();
                string adminUserEmail = "admin@gmail.com";

                var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
                if (adminUser == null)
                {
                    var newAdminUser = new User("admin","admin", adminUserEmail, User.TypeSuperAdmin)
                    {
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(newAdminUser, "admin");
                    await userManager.AddToRoleAsync(newAdminUser, RoleSuperAdmin);
                }



            }
        }
    }
}
