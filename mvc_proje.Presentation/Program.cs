using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using Microsoft.EntityFrameworkCore;
using mvc_proje.Application.Services;
using mvc_proje.Application.Services.Admin;
using mvc_proje.Domain.Enums;
using mvc_proje.Domain.Interfaces;
using mvc_proje.Infrastructure.Database.Context;
using mvc_proje.Application.UnitOfWork;
using mvc_proje.Domain.Misc;
using mvc_proje.Misc;
using AdminOrderService = mvc_proje.Application.Services.Admin.OrderService;
using OrderService = mvc_proje.Application.Services.OrderService;
using AdminCommentService = mvc_proje.Application.Services.Admin.CommentService;
using CommentService = mvc_proje.Application.Services.CommentService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add configuration for Iyizico payment gateway
builder.Services.AddOptions();
builder.Services.Configure<IyizicoConfig>(builder.Configuration.GetSection("Iyizico"));

// Create db context
using (var dbContext = new AppDbCtx())
{
    // Ensure the database is created
    dbContext.Database.EnsureCreated();
}

// Register the db context with dependency injection
builder.Services.AddDbContext<AppDbCtx>(ServiceLifetime.Scoped);


// Register unit of work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Register services
var services = new[]
{
    typeof(CategoryService),
    typeof(ContactMessageService),
    typeof(FeatureService),
    typeof(AdminOrderService),
    typeof(OrderService),
    typeof(ProductService),
    typeof(AdminCommentService),
    typeof(CommentService),
    typeof(OrderTrackService),
    typeof(PostService),
    typeof(ReviewService),
    typeof(SliderService),
    typeof(TagService),
    typeof(UserService),
    typeof(AboutUsService),
    typeof(AuthService),
    typeof(CartService),
    typeof(HomepageService),
    typeof(ProfileService),
    typeof(ProductFeatureService),
    typeof(ImageService),
    typeof(SearchService)
};

foreach (var service in services)
{
    builder.Services.AddScoped(service);
}

// Register settings and about us services manually
var resourcesPath = Path.Combine(Directory.GetCurrentDirectory(), "Resources");

builder.Services.AddSingleton<SettingsService>(sp =>
{
    var settingsService = new SettingsService(Path.Combine(resourcesPath, "settings.json"));
    return settingsService;
});

builder.Services.AddSingleton<AboutUsService>(sp =>
{
    var aboutUsService = new AboutUsService(Path.Combine(resourcesPath, "about-us.json"));
    return aboutUsService;
});

builder.Services.AddSession();

// Add support for Views/Admin views discovery
builder.Services.AddControllersWithViews()
    .AddRazorOptions(options => { options.ViewLocationExpanders.Add(new CustomViewLocationExpander()); });

builder.Services.AddAuthentication()
    .AddCookie(options => { options.LoginPath = "/auth/login/"; });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy =>
        policy.RequireRole(nameof(Role.Admin)));

    options.AddPolicy("UserPolicy", policy =>
        policy.RequireRole(nameof(Role.User), nameof(Role.Admin)));
});

StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration);

var app = builder.Build();

app.UseStatusCodePagesWithReExecute("/404");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession();

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Homepage}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();