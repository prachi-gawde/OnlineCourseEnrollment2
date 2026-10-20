// Create the application builder.
var builder = WebApplication.CreateBuilder(args);

// Register MVC services.
builder.Services.AddControllersWithViews();

// Register distributed memory cache for session storage.
builder.Services.AddDistributedMemoryCache();

// Register session services.
builder.Services.AddSession();

// Register in-memory caching.
builder.Services.AddMemoryCache();

// Register response caching.
builder.Services.AddResponseCaching();

// Build the application.
var app = builder.Build();

// Enable exception handling outside development.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

// Enable HTTPS redirection.
app.UseHttpsRedirection();

// Enable static files.
app.UseStaticFiles();

// Enable routing.
app.UseRouting();

// Enable response caching middleware.
app.UseResponseCaching();

// Enable session middleware.
app.UseSession();

// Enable authorization middleware.
app.UseAuthorization();

// Configure the default route.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Course}/{action=Index}/{id?}");

// Start the application.
app.Run();8