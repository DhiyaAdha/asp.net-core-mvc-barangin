using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json; // pastikan menggunakan direktif ini


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

/*
* set up calling Routing on asp.net core
*/
app.MapControllers();

// app.UseAuthorization();

// app.MapControllerRoute(
//     name: "default",
//     pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
