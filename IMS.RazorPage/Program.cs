using IMS.Models;
using IMS.Models.Common;
using IMS.Models.Interfaces;
using IMS.Models.Repositories;
using IMS_View.Models.Interfaces;
using IMS_View.Models.Repositories;
using IMS_View.Services.Interfaces;
using IMS_VIew.Services.Interfaces;
using IMS_View.Services.Services;
using IMS_VIew.Services.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbContext"));
    //b => b.MigrationsAssembly("IMS"));
    //options.UseSqlServer(b => b.MigrationsAssembly("WarrantyManagement"));
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(o =>
    {
        o.LoginPath = "/Login";
    });

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllersWithViews();

builder.Services.AddAutoMapper(typeof(MapperProfile).Assembly);
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//Repository
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();


//Service
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IRoleService, RoleService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
