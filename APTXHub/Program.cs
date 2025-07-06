using APTXHub.Infrastructure.Hubs;
using APTXHub.Infrastructure;
using APTXHub.Infrastructure.Helpers;
using APTXHub.Infrastructure.Models;
using APTXHub.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Localization;
using APTXHub.Infrastructure.Dtos;
using CloudinaryDotNet;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Gioi han upload video
builder.Services.Configure<FormOptions>(o =>
{
    o.MultipartBodyLengthLimit = 52428800; // 50MB
});


// DB configuration
string stringConnection = builder.Configuration.GetConnectionString("Default")!;
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(stringConnection));

// DI: Add services
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<IHashtagService, HashtagService>();
builder.Services.AddScoped<IStoriesService, StoriesSevice>();
builder.Services.AddScoped<IFilesService, FilesSevice>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IFriendsService, FriendsService>();
builder.Services.AddScoped<INotificationService, NotificationService>();    
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<ISearchService, SearchService>();
builder.Services.AddScoped<IMessageService, MessageService>();

// Identity configuration
builder.Services.AddIdentity<User, IdentityRole<int>>(options =>
{
    //Password settings
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 4;
})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Auth/Login"; 
    options.AccessDeniedPath = "/Auth/AccessDenied";
});

// External authentication providers
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddGoogle(options =>
    {
        options.ClientId = builder.Configuration["Auth:Google:ClientId"] ?? "";
        options.ClientSecret = builder.Configuration["Auth:Google:ClientSecret"] ?? "";
        options.CallbackPath = "/signin-google";
    })
    .AddGitHub(options =>
    {
        options.ClientId = builder.Configuration["Auth:GitHub:ClientId"] ?? "";
        options.ClientSecret = builder.Configuration["Auth:GitHub:ClientSecret"] ?? "";
        options.CallbackPath = "/signin-github";
        options.Scope.Add("user:email");
        options.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
    });


builder.Services.AddSignalR();

builder.Services.AddAuthorization();

// Multiple language
builder.Services.AddControllersWithViews()
    .AddViewLocalization()
    .AddDataAnnotationsLocalization();

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[] { "en", "vi" };
    var localizationCultures = supportedCultures.Select(c => new System.Globalization.CultureInfo(c)).ToList();

    options.DefaultRequestCulture = new RequestCulture("en");
    options.SupportedCultures = localizationCultures;
    options.SupportedUICultures = localizationCultures;

    // Tùy chọn: đổi qua cookie
    options.RequestCultureProviders.Insert(0, new CookieRequestCultureProvider());
});


builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));

builder.Services.AddSingleton(s =>
{
    var config = builder.Configuration.GetSection("CloudinarySettings").Get<CloudinarySettings>();
    var acc = new Account(config?.CloudName, config?.ApiKey, config?.ApiSecret);
    return new Cloudinary(acc);
});

var app = builder.Build();

app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

// Init data in database
using (var scope = app.Services.CreateScope())
{
    //var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    //await dbContext.Database.MigrateAsync();
    //await DbInitializer.SeedAsync(dbContext);

    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
    await DbInitializer.SeedUsersAndRolesAsync(userManager, roleManager);
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

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Custom url post detail: /post/{id}
app.MapControllerRoute(
    name: "post_detail",
    pattern: "post/{postid}",
    defaults: new { controller = "Home", action = "Details" });

// Custom url post detail: /user/{id}
app.MapControllerRoute(
    name: "user_detail",
    pattern: "user/{userId}",
    defaults: new { controller = "Users", action = "Details" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<NotificationHub>("/notificationHub");
app.MapHub<ChatHub>("/chatHub");

app.Run();

