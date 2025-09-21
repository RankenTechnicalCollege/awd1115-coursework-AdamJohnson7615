using Microsoft.EntityFrameworkCore;
using Project1_ContactManager.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure EF Core - update connection string name as appropriate.
// Example using LocalDB (change to your DB provider/connection)
builder.Services.AddDbContext<ContactContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ContactContext")));

// Make URLs lowercase and append trailing slash
builder.Services.Configure<Microsoft.AspNetCore.Routing.RouteOptions>(options =>
{
    options.LowercaseUrls = true;
    options.AppendTrailingSlash = true;
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Default route with optional slug at the end.
// Pattern: /contacts/details/1/john-doe/
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=contacts}/{action=index}/{id?}/{slug?}");

app.Run();
