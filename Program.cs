using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rotativa.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Tambah MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Setup Rotativa
RotativaConfiguration.Setup(builder.Environment.WebRootPath, "Rotativa");

// Routing & middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
