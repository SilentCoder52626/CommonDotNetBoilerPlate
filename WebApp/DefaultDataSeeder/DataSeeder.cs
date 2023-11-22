using DomainModule.Entity;
using DomainModule.ServiceInterface;
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
                var RoleGeneral = User.TypeGeneral;


                var Permissions = new List<string>()
                {
                    "User-ResetPassword",
                    "User-Update",

                };
                //Roles
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var roleService = serviceScope.ServiceProvider.GetRequiredService<RoleServiceInterface>();

                if (!await roleManager.RoleExistsAsync(RoleSuperAdmin))
                    await roleManager.CreateAsync(new IdentityRole(RoleSuperAdmin));
                
                if (!await roleManager.RoleExistsAsync(RoleGeneral))
                {
                    await roleManager.CreateAsync(new IdentityRole(RoleGeneral));
                    await roleService.AssignPermissionInBulk(RoleGeneral, Permissions);
                }
                    
    
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
