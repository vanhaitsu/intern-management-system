using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using IMS.Repositories;
using IMS.Repositories.Common;
using IMS.Repositories.Interfaces;
using IMS.Repositories.Repositories;
using IMS.Services.Interfaces;
using IMS.Services.Services;

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
//builder.Services.AddScoped<ITraineeRepository, TraineeRepository>();
//builder.Services.AddScoped<ITrainingProgramRepository, TrainingProgramRepository>();

//Service
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IRoleService, RoleService>();
//builder.Services.AddScoped<ITraineeService, TraneeService>();
//builder.Services.AddScoped<ITrainingProgramService, TrainingProgramService>();
//builder.Services.AddScoped<IEmailService, EmailService>();

var app = builder.Build();
await InitialSeeding.Initialize(app.Services);

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
//private static async Task InitializeRolesAsync(IServiceProvider serviceProvider)
//{
//    await InitialSeeding.Initialize(serviceProvider);
//}