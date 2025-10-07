using Microsoft.EntityFrameworkCore;
using Project1_TripsLog.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllersWithViews();

// EF Core + SQLite
builder.Services.AddDbContext<TripsContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("TripsDb") ?? "Data Source=Trips.db")
);

var app = builder.Build();

// Ensure DB created (simple approach for student project)
using (var scope = app.Services.CreateScope())
{
    var ctx = scope.ServiceProvider.GetRequiredService<TripsContext>();
    ctx.Database.EnsureCreated();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();