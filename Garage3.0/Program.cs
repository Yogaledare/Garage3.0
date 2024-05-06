using FluentValidation;
using FluentValidation.AspNetCore;
using Garage3._0.Data;
using Garage3._0.Controllers;
using Garage3._0.Models;
using Garage3._0.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddFluentValidationAutoValidation();
// builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<Program>(); 


builder.Services.AddDbContext<GarageDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("GarageContext")));
//Add service about garage manager
builder.Services.AddScoped<GarageManager>();
builder.Services.AddScoped<IMemberService, MemberService>();
//Add service for fake data
//builder.Services.AddTransient<FakeDataGenerator>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
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

//


app.Run();