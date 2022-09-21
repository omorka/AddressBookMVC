using AddressBook.Data;
using AddressBook.Services;
using AddressBook.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseNpgsql(DataUtility.GetConnectionString(builder.Configuration)));

builder.Services.AddScoped<IImageService, BasicImageService>();

builder.Services.AddControllersWithViews();


var app = builder.Build();

var dbContext = app.Services.CreateScope().ServiceProvider.GetRequiredService<ApplicationDbContext>();
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

await dbContext.Database.MigrateAsync();

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

app.Run();
