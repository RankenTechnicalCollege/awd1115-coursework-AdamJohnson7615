using Microsoft.EntityFrameworkCore;
using Project1_FAQs.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();

// Add DbContext (update the connection string to match your setup)
builder.Services.AddDbContext<FaqContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("FaqContext")));


var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// Custom route for FAQs with optional static segments and trailing slash
app.MapControllerRoute(
    name: "faqs",
    pattern: "faqs/{topicId?}/{categoryId?}/",
    defaults: new { controller = "Faqs", action = "Index" }
);

// Default route points root URL directly to FAQs Index
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Faqs}/{action=Index}/{id?}"
);

app.Run();
