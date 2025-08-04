using mvc_proje.Database;
using mvc_proje.Database.Entities;
using mvc_proje.Database.Repositories;
using mvc_proje.Misc;
using mvc_proje.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Create db context
var dbCtx = new AppDbCtx();
dbCtx.Database.EnsureCreated();

// Register the db context with dependency injection
builder.Services.AddSingleton(dbCtx);

// Register repositories
var repositories = new[]
{
    typeof(UserRepository),
    typeof(CategoryRepository),
    typeof(ProductRepository),
    typeof(PostRepository),
    typeof(CommentRepository),
    typeof(ContactMessageRepository),
    typeof(FeatureRepository),
    typeof(SliderRepository),
    typeof(ReviewRepository),
    typeof(TagRepository),
    typeof(OrderRepository),
    typeof(OrderTrackRepository),
};

foreach (var repository in repositories)
{
    builder.Services.AddSingleton(repository, provider =>
    {
        var context = provider.GetRequiredService<AppDbCtx>();
        return Activator.CreateInstance(repository, context);
    });
}

// Register custom services
builder.Services.AddSingleton<IAboutUsService>(new AboutUsService());

builder.Services.AddSingleton<ISettingsService>(new SettingsService());

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
        policy.RequireRole(nameof(Role.User)));
});

var app = builder.Build();

app.UseStatusCodePagesWithReExecute("/404");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

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