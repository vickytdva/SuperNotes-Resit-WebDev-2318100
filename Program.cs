using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Session;
using Microsoft.EntityFrameworkCore;
using SuperNotesBackend.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add session services
builder.Services.AddDistributedMemoryCache();  // In-memory session storage
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(1);  // Session timeout (1 hour)
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Add Entity Framework
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add authentication services with cookie authentication (no JWT)
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/auth/login";  // Redirect to login page
        options.ExpireTimeSpan = TimeSpan.FromHours(1);  // Cookie expiry time
    });

var app = builder.Build();

// Use session middleware
app.UseSession();
app.UseAuthentication();  // Enable authentication
app.UseAuthorization();  // Enable authorization

app.MapControllers();

app.Run();
