using Microsoft.EntityFrameworkCore;
using Project2_FAQs.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register DbContext with the connection string
builder.Services.AddDbContext<FaqContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("FaqContext")));

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

// Default route for MVC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Faqs}/{action=Index}/{id?}");

app.Run();
