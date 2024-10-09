using Avtotest.Data.Context;
using Avtotest.Data.Entities.TestEntities;
using Avtotest.Data.Repositories;
using Avtotest.Services.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Avtotest.Data.Entities;


var builder = WebApplication.CreateBuilder(args);







// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddScoped<TestRepository>();
builder.Services.AddScoped<TestService>();



AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);


builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<AppDbContext>();


builder.Services.AddMemoryCache();
builder.Services.AddScoped<IResultRepository, ResultRepository>();


builder.Services.AddRazorPages();



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

app.MapRazorPages();

app.Run();
