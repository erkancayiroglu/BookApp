using BookApp.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<DataContext>(options => // servis ekleme i�lemi
{
    var config = builder.Configuration;
    var connectionString = config.GetConnectionString("database"); // bu k�s�m deveploment.jsona aktard���m�z kodla yap�ldu
                                                                   //"database" ile json dosyas�nada db yoluna ula�aca��z
    options.UseSqlite(connectionString);//options.UseSqlite("Data Source=database.db")); // SQLite kullan�yorsan    
});

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
    pattern: "{controller=User}/{action=Index}/{id?}");

app.Run();
