using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json; 
using MySqlConnector;
using barangin.Database;

var builder = WebApplication.CreateBuilder(args);

/*
* Add services to the container.
* setup calling controllers on asp.net core
*/
builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    {
        // Konfigurasi opsional untuk Newtonsoft.Json
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        // Tambahkan konfigurasi lain sesuai kebutuhan
    });

/*
* Setup database Mysql
*/
builder.Services.AddScoped<ConnectionDb>(provider =>
{
    // Ambil connection string dari appsettings.json
    string connectionString = builder.Configuration.GetConnectionString("Default");

    // Membuat instance ConnectionDb dan memberikan connection string
    return new ConnectionDb(connectionString);
});

var app = builder.Build(); // Pindahkan ini ke atas

// Panggil TestConnection setelah membuat instance ConnectionDb
using (var scope = app.Services.CreateScope())
{
    var connectionDb = scope.ServiceProvider.GetRequiredService<ConnectionDb>();
    connectionDb.TestConnection();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

/*
* set up calling Routing on asp.net core
*/
app.MapControllers();

// app.UseAuthorization();

// app.MapControllerRoute(
//     name: "default",
//     pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
