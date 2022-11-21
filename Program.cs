using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DaisyStudy.Data;
using DaisyStudy.Core;
using DaisyStudy.Core.Repositories;
using DaisyStudy.Repositories;
using DaisyStudy.Application.Common;

var builder = WebApplication.CreateBuilder(args);
var mvcBuilder = builder.Services.AddRazorPages();
var connectionString = builder.Configuration.GetConnectionString("ApplicationDbContextConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

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

// Add services to the container.
builder.Services.AddControllersWithViews();

#region Authorization

AddAuthorizationPolicies();

#endregion

AddScoped();

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

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

void AddScoped()
{
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<IRoleRepository, RoleRepository>();
    builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
}