using Microsoft.EntityFrameworkCore;
using Restaurante.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Globalization;

var cultureInfo = new CultureInfo("en-US");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient("currencyLayer", c =>
{
    c.BaseAddress = new Uri("http://apilayer.net/api/");
});

builder.Services.AddTransient<CurrencyLayerService>();
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<RestauranteContext>(options => options.UseSqlServer("CadenaSQL"));
builder.Services.AddHttpClient<WeatherService>();
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)

    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
    }); // para poder configurar la sesion de usuario

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CanVerListados", policy =>
        policy.RequireClaim("Permission", "VerListados"));

    options.AddPolicy("CanVerOrdenes", policy =>
        policy.RequireClaim("Permission", "VerOrdenes"));

    options.AddPolicy("CanVerReservas", policy =>
        policy.RequireClaim("Permission", "VerReservas"));

    options.AddPolicy("CanVerClientes", policy =>
        policy.RequireClaim("Permission", "VerClientes"));

    options.AddPolicy("CanVerProductos", policy =>
        policy.RequireClaim("Permission", "VerProductos"));

    options.AddPolicy("CanVerReseņas", policy =>
        policy.RequireClaim("Permission", "VerReseņas"));

    options.AddPolicy("CanVerPermisosRoles", policy =>
        policy.RequireClaim("Permission", "VerPermisosRoles"));
});

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

