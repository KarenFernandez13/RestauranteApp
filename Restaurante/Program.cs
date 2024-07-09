using Microsoft.EntityFrameworkCore;
using Restaurante.Models;
<<<<<<< HEAD
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
=======


>>>>>>> cc0ce3aea1527563f64bae33cea5b5c05b7261a2
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<RestauranteContext>(options => options.UseSqlServer("CadenaSQL"));
<<<<<<< HEAD
builder.Services.AddHttpClient<WeatherService>();
builder.Services.AddHttpClient<CurrencyService>();
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)


=======

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
>>>>>>> cc0ce3aea1527563f64bae33cea5b5c05b7261a2
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

    options.AddPolicy("CanVerReseñas", policy =>
        policy.RequireClaim("Permission", "VerReseñas"));

    options.AddPolicy("CanVerPermisosRoles", policy =>
        policy.RequireClaim("Permission", "VerPermisosRoles"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

<<<<<<< HEAD

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}


=======
>>>>>>> cc0ce3aea1527563f64bae33cea5b5c05b7261a2
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
<<<<<<< HEAD

=======
>>>>>>> cc0ce3aea1527563f64bae33cea5b5c05b7261a2
