using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DaisyStudy.Data;
using DaisyStudy.Core;
using DaisyStudy.Core.Repositories;
using DaisyStudy.Repositories;
using DaisyStudy.Application.Common;
using DaisyStudy.Application.System.Users;
using DaisyStudy.Application.Catalog.Classes;
using System.Configuration;
using DaisyStudy.Models.VNPAY;
using DaisyStudy.Controllers.Hubs;
using DaisyStudy.Application.Common.SignalR;
using DaisyStudy.Application.Catalog.Rooms;

var builder = WebApplication.CreateBuilder(args);
var mvcBuilder = builder.Services.AddRazorPages();
var connectionString = builder.Configuration.GetConnectionString("ApplicationDbContextConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddSession(options => options.IdleTimeout = TimeSpan.FromMinutes(30));

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

if (builder.Environment.IsDevelopment())
{
    mvcBuilder.AddRazorRuntimeCompilation();
}

builder.Services.AddAuthentication()
    .AddGoogle(googleOptions =>
        {
            IConfigurationSection googleAuthNSection = builder.Configuration.GetSection("Authentication:Google");
            googleOptions.ClientId = googleAuthNSection["ClientId"];
            googleOptions.ClientSecret = googleAuthNSection["ClientSecret"];
        })
    .AddFacebook(facebookOptions =>
    {
        IConfigurationSection facebookAuthNSection = builder.Configuration.GetSection("Authentication:Facebook");
        facebookOptions.ClientId = facebookAuthNSection["AppId"];
        facebookOptions.ClientSecret = facebookAuthNSection["AppSecret"];
    })
    .AddMicrosoftAccount(microsoftOptions =>
    {
        IConfigurationSection microsoftAuthNSection = builder.Configuration.GetSection("Authentication:Microsoft");
        microsoftOptions.ClientId = microsoftAuthNSection["ClientId"];
        microsoftOptions.ClientSecret = microsoftAuthNSection["ClientSecret"];
    });
    
    builder.Services.AddTransient<IStorageService, FileStorageService>();

    builder.Services.AddSignalR();

// Add services to the container.
builder.Services.AddControllersWithViews();

#region Authorization

AddAuthorizationPolicies();

#endregion

AddTransient();

builder.Services.AddSingleton(builder.Configuration.GetSection("VNPayConfig").Get<VNPayConfig>());

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

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
app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<ChatHub>("/chatHub");

app.MapRazorPages();

app.Run();


void AddAuthorizationPolicies()
{
    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("EmployeeOnly", policy => policy.RequireClaim("EmployeeNumber"));
    });

    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy(Constants.Policies.RequireAdmin, policy => policy.RequireRole(Constants.Roles.Administrator));
        options.AddPolicy(Constants.Policies.RequireManager, policy => policy.RequireRole(Constants.Roles.Manager));
    });
}

void AddTransient()
{
    builder.Services.AddTransient<IUserRepository, UserRepository>();
    builder.Services.AddTransient<IRoleRepository, RoleRepository>();
    builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
    builder.Services.AddTransient<IUserService, UserService>();
    builder.Services.AddTransient<IClassService, ClassService>();
    builder.Services.AddTransient<IFileValidator, FileValidator>();
    builder.Services.AddTransient<IRoomService, RoomService>();
}