using InstagramScraper.Models;
using InstagramScraper.Service;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.Configure<InstagramApiSettings>(
    builder.Configuration.GetSection("InstagramApi"));

// Register HttpClient
builder.Services.AddHttpClient("InstagramAPI", client =>
{
    client.Timeout = TimeSpan.FromSeconds(30);
});

// Register InstagramService
builder.Services.AddScoped<InstagramService>();

// Configure forwarded headers for Render.com
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Important: Place UseForwardedHeaders at the top of the middleware pipeline
app.UseForwardedHeaders();

// Handle HTTPS redirection
if (!app.Environment.IsDevelopment()) 
{
    app.Use((context, next) =>
    {
        if (context.Request.Headers["X-Forwarded-Proto"].ToString().Equals("https", StringComparison.OrdinalIgnoreCase))
        {
            context.Request.Scheme = "https";
        }
        return next();
    });
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
