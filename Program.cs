using Blog.Data;
using Blog.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    })
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });
builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseMySQL("Server=localhost;Database=blog;Uid=root;Pwd=password;"));  
builder.Services.AddDbContext<BlogDbContext>(options =>
            options.UseMySQL("Server=localhost;Database=blog;Uid=root;Pwd=password;"));               
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
using (var serviceProvider = builder.Services.BuildServiceProvider())
{
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>(); 
    
    if (!roleManager.RoleExistsAsync("Admin").Result)
        {
            var adminRole = new IdentityRole("Admin");
            var result = roleManager.CreateAsync(adminRole).Result;
        }

        if (!roleManager.RoleExistsAsync("User").Result)
        {
            var userRole = new IdentityRole("User");
            var result = roleManager.CreateAsync(userRole).Result;
        }
}
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("UserAndAdmin", policy =>
        policy.RequireRole("User", "Admin"));
});

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.BuildServiceProvider().GetService<ApplicationDbContext>().Database.Migrate();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);
app.MapControllerRoute(
    name: "blog",
    pattern: "Blog/{action=Index}/{id?}",
    defaults: new { controller = "Blog" }
);

app.Run();
