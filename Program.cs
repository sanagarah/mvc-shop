using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using mvc_shop.Models;
using mvc_shop.Utility;

var builder = WebApplication.CreateBuilder(args);
var host = builder.Configuration["DATABASE_HOST"];
var port = builder.Configuration["DATABASE_PORT"];
var name = builder.Configuration["DATABASE_NAME"];
var username = builder.Configuration["DATABASE_USERNAME"];
var password = builder.Configuration["DATABASE_PASSWORD"];
var connectionString = $"Server={host};Port={port};Database={name};User Id={username};Password={password};";


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString)
);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddTransient<IEmailSender,EmailSender>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddDefaultTokenProviders().AddDefaultUI().AddEntityFrameworkStores<AppDbContext>();

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

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseSession();

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();