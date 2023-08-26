using DomainModule.Entity;
using InfrastructureModule.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Newtonsoft.Json;
using NToastNotify;
using WebApp.AuthenticationAuthorization;
using WebApp.CustomTokenProvider;
using WebApp.DefaultDataSeeder;
using WebApp.DiConfig;
using WebApp.Extensions;

var builder = WebApplication.CreateBuilder(args);

var CustomEmailTokenProposeString = "Email_Token_Provider";
// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders()
    .AddTokenProvider<CustomEmailTokenProvider<User>>(CustomEmailTokenProposeString);

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 4;
    options.Password.RequireDigit = false;
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.SignIn.RequireConfirmedEmail = true;
    options.Tokens.EmailConfirmationTokenProvider = CustomEmailTokenProposeString;
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.MaxValue; // lock out forever until the user is unlocked manually by admin
}
);
//Token lifespan config
builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
{
    options.TokenLifespan = TimeSpan.FromHours(1);
});


//Custome Token provider config -- for token the lifespan is 1 hour but for email token its 20 minute
builder.Services.Configure<CustomEmailTokenProviderOptions>(options => options.TokenLifespan = TimeSpan.FromMinutes(20));

builder.Services.AddMvc()
     .AddMvcOptions(options =>
     {
         var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
         options.Filters.Add(new AuthorizeFilter(policy));

     })
    .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null)
    .AddNToastNotifyToastr(new ToastrOptions()
    {
        ProgressBar = true,
        TimeOut = 1500,
        PositionClass = ToastPositions.TopRight
    });


builder.Services.ConfigureAuthentication();
builder.Services.UseDIConfig();
builder.Services.Configure<CookiePolicyOptions>(
               options =>
               {
                   // This lambda determines whether user consent for non-essential cookies is needed for a given request.

                   options.MinimumSameSitePolicy = SameSiteMode.Lax;
               }
           );
builder.Services.AddHttpContextAccessor();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
ServiceActivator.Configure(builder.Services.BuildServiceProvider());
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseCookiePolicy();
app.UseAuthorization();

app.MapControllerRoute(
    name: "MyArea",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

DataSeeder.SeedUsersAndRolesAsync(app).Wait();

app.Run();
