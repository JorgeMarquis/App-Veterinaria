using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using PRJ_VETERINARIA.DataAccessLayer.Models;

var builder = WebApplication.CreateBuilder(args);

// MVC
builder.Services.AddControllersWithViews()
    .AddRazorOptions(options =>
    {
        // Include the UI/Views folders in the search locations so views placed under UI/Views
        // (like UI/Views/Account/Login.cshtml) are found by the view engine.
        options.ViewLocationFormats.Add("/UI/Views/{1}/{0}.cshtml");
        options.ViewLocationFormats.Add("/UI/Views/Shared/{0}.cshtml");
    });

// DB
builder.Services.AddDbContext<BDVeterinariaContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CadenaVeterinaria")));

// AUTH COOKIES
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
        options.Cookie.HttpOnly = true;
        options.Cookie.Name = "VetAuthCookie";
    });

builder.Services.AddAuthorization();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();   // IMPORTANTE
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}")
    .WithStaticAssets();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<BDVeterinariaContext>();

    var usuario = db.Usuarios.FirstOrDefault(u => u.Email == "admin@veterinaria.com");

    if (usuario != null)
    {
        usuario.PasswordHash = BCrypt.Net.BCrypt.HashPassword("grupo3");
        usuario.Activo = true;
        db.SaveChanges();
    }
}


app.Run();