using Microsoft.EntityFrameworkCore;
using Practica_2.Infrastructure.Data;
using Practica_2.Infrastructure.Repository;
using Practica_2.Application.Services;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
var conexion = builder.Configuration
    .GetConnectionString("ConexionSQLite")
    ?? throw new InvalidOperationException(
        "No se encontro la conexion SQLite.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(conexion));

builder.Services.AddScoped(
    typeof(IRepositorioGenerico<>),
    typeof(RepositorioGenerico<>));

builder.Services.AddScoped<IProductoServicio, ProductoServicio>();


builder.Services.AddProblemDetails();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}


app.UseExceptionHandler();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
