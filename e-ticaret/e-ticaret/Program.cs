using DataAccessLayer.Connections;
using e_ticaret.Validations;
using EntityLayer.Entity;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using e_ticaret.Models;
using e_ticaret.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
    

builder.Services.AddDbContext<TContext>();
builder.Services.AddIdentity<AppUser, AppRole>
    (options =>
    {
        options.User.AllowedUserNameCharacters += "ðÐÝÞþýÖöüÜçÇ";
    }
    ).AddEntityFrameworkStores<TContext>().AddDefaultTokenProviders();

builder.Services.AddMvc(config =>
{
    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    config.Filters.Add(new AuthorizeFilter(policy));
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Login/Index";
    options.AccessDeniedPath = "/Access/Denied";
    options.LogoutPath = "/Home/Index";
    options.ExpireTimeSpan = TimeSpan.FromDays(1);
    options.Cookie.HttpOnly = true;
    options.SlidingExpiration = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddScoped<CartController>();

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

app.UseMiddleware<CustomRedirectMiddleware>();

app.UseEndpoints(endpoints =>
{
	endpoints.MapControllerRoute(
	  name: "areas",
	  pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
	);
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
