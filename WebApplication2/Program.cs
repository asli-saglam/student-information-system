using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllersWithViews();

// DbContext servisini ekleyin (ConnectionString appsettings.json'da olmalý)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Özel route'lar
app.MapControllerRoute(
    name: "ogrenciDersListesi",
    pattern: "Ogrenci/DersListesi",
    defaults: new { controller = "Ogrenci", action = "OgrenciDersListesi" });

app.MapControllerRoute(
    name: "dersIstatistikleri",
    pattern: "Ders/Istatistikler",
    defaults: new { controller = "Ders", action = "DersIstatistikleri" });
app.Run();
