using Microsoft.EntityFrameworkCore;
using StarLogger.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add DbContext
builder.Services.AddDbContext<StarLoggerContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("StarLoggerConnection")));

// Configure routing to enforce lowercase URLs and trailing slashes
builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
    options.AppendTrailingSlash = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// ==============================
// Achievements Routes (public)
// ==============================

// Achievements list
app.MapControllerRoute(
    name: "achievements",
    pattern: "achievements/",
    defaults: new { controller = "Achievements", action = "Index" });

// Achievement create
app.MapControllerRoute(
    name: "achievement-create",
    pattern: "achievements/create/",
    defaults: new { controller = "Achievements", action = "Create" });

// Achievement detail with slug
app.MapControllerRoute(
    name: "achievement-detail",
    pattern: "achievements/{id:int}/{slug}/",
    defaults: new { controller = "Achievements", action = "Details" });

// Achievement edit with slug
app.MapControllerRoute(
    name: "achievement-edit",
    pattern: "achievements/edit/{id:int}/{slug}/",
    defaults: new { controller = "Achievements", action = "Edit" });

// Achievement delete with slug
app.MapControllerRoute(
    name: "achievement-delete",
    pattern: "achievements/delete/{id:int}/{slug}/",
    defaults: new { controller = "Achievements", action = "Delete" });

// ==============================
// Games Routes (public)
// ==============================

// Games list
app.MapControllerRoute(
    name: "games",
    pattern: "games/",
    defaults: new { controller = "Game", action = "Index" });

// Game detail with slug
app.MapControllerRoute(
    name: "game-detail",
    pattern: "games/{id:int}/{slug}/",
    defaults: new { controller = "Game", action = "Details" });

// Game edit with slug
app.MapControllerRoute(
    name: "game-edit",
    pattern: "games/edit/{id:int}/{slug}/",
    defaults: new { controller = "Game", action = "Edit" });

// Game delete with slug
app.MapControllerRoute(
    name: "game-delete",
    pattern: "games/delete/{id:int}/{slug}/",
    defaults: new { controller = "Game", action = "Delete" });

// ==============================
// Manage Area Routes
// ==============================
app.MapControllerRoute(
    name: "manage",
    pattern: "manage/{controller=Game}/{action=Index}/{id?}",
    defaults: new { area = "Manage" });

// ==============================
// Default Fallback
// ==============================
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
