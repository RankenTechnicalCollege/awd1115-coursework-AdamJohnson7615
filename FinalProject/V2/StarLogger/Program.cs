using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StarLogger.Data;
using StarLogger.Models;

var builder = WebApplication.CreateBuilder(args);

// 1. Add services to the container.
builder.Services.AddControllersWithViews();

// --- NEW: ADD SESSION SERVICE (CH9 Requirement) ---
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Session lasts 30 min
    options.Cookie.HttpOnly = true; // Security: JS cannot access the cookie
    options.Cookie.IsEssential = true; // GDPR: Required for the app to function
});
// --------------------------------------------------

// --- DATABASE CONNECTION ---
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// --- IDENTITY (AUTHENTICATION) ---
// This enables Login, Register, and Roles using your custom User class
builder.Services.AddDefaultIdentity<User>(options =>
{
    // Simplified password rules for testing/grading
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 3;
})
.AddRoles<IdentityRole>() // Enables the Admin Role
.AddEntityFrameworkStores<ApplicationDbContext>();

// --- CUSTOM SERVICES ---
builder.Services.AddScoped<StarLogger.Services.IIgdbService, StarLogger.Services.IgdbService>();
builder.Services.AddScoped<StarLogger.Repositories.IGameRepository, StarLogger.Repositories.GameRepository>();
var app = builder.Build();

// 2. Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // Ensure static files (CSS/JS) are served
app.UseRouting();

// These two must be in this order:
app.UseAuthentication(); // checks "Who are you?"
app.UseAuthorization();  // checks "Are you allowed?"

// --- NEW: ENABLE SESSION MIDDLEWARE (CH9 Requirement) ---
// Must be AFTER UseRouting and BEFORE MapControllerRoute
app.UseSession();
// --------------------------------------------------------

app.MapStaticAssets();

// IMPORTANT: This enables the Identity pages (Login/Register)
app.MapRazorPages();

// --- CUSTOM ROUTES (CH6 - 5pts) ---

// 1. NEW: Area Route (MUST BE FIRST)
// This handles URLs like /Admin/Users/Index
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

// 2. Friendly Route for About
app.MapControllerRoute(
    name: "about",
    pattern: "About",
    defaults: new { controller = "About", action = "Index" });

// 3. Friendly Route for Profiles
app.MapControllerRoute(
    name: "profile",
    pattern: "Profile/{username}",
    defaults: new { controller = "Profile", action = "Index" });

// --- DEFAULT ROUTE ---
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

// === SEED DATA (ASYNC) ===
// We use a scope to get the services needed for the Seeder
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        // This runs the new Async seeder we wrote to create Admin & Games
        await DbSeeder.SeedAsync(services);
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error seeding database: " + ex.Message);
    }
}
// =========================

app.Run();