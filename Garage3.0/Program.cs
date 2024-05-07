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
builder.Services.AddScoped<IGarageManager, GarageManager>();
builder.Services.AddScoped<IMemberService, MemberService>();
//Add service for fake data
builder.Services.AddTransient<SeedDataGenerator>();

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
    pattern: "{controller=Members}/{action=Index}/{id?}");

//try dataseeding here
using (var scope = app.Services.CreateScope())
{
    var seedDataGenerator = scope.ServiceProvider.GetRequiredService<SeedDataGenerator>();
    seedDataGenerator.Generate();
}
app.Run();