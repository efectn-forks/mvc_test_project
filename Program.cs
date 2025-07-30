using mvc_proje.Database;
using mvc_proje.Database.Repositories;
using mvc_proje.Misc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Create db context
var dbCtx = new AppDbCtx();
dbCtx.Database.EnsureCreated();

// Register the db context with dependency injection
builder.Services.AddSingleton(dbCtx);

// Register repositories
builder.Services.AddSingleton<UserRepository>(provider =>
{
    var context = provider.GetRequiredService<AppDbCtx>();
    return new UserRepository(context);
});

builder.Services.AddSingleton<CategoryRepository>(provider =>
{
    var context = provider.GetRequiredService<AppDbCtx>();
    return new CategoryRepository(context);
});

builder.Services.AddSingleton<ProductRepository>(provider =>
{
    var context = provider.GetRequiredService<AppDbCtx>();
    return new ProductRepository(context);
});

builder.Services.AddSingleton<PostRepository>(provider =>
{
    var context = provider.GetRequiredService<AppDbCtx>();
    return new PostRepository(context);
});

builder.Services.AddSingleton<CommentRepository>(provider =>
{
    var context = provider.GetRequiredService<AppDbCtx>();
    return new CommentRepository(context);
});

builder.Services.AddSingleton<ContactMessageRepository>(provider =>
{
    var context = provider.GetRequiredService<AppDbCtx>();
    return new ContactMessageRepository(context);
});

builder.Services.AddSingleton<FeatureRepository>(provider =>
{
    var context = provider.GetRequiredService<AppDbCtx>();
    return new FeatureRepository(context);
});

builder.Services.AddSingleton<SliderRepository>(provider =>
{
    var context = provider.GetRequiredService<AppDbCtx>();
    return new SliderRepository(context);
});

// Add support for Views/Admin views discovery
builder.Services.AddControllersWithViews()
    .AddRazorOptions(options =>
    {
        options.ViewLocationExpanders.Add(new CustomViewLocationExpander());
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home2}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
