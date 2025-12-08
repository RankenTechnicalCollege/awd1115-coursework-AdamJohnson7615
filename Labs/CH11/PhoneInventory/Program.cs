using Microsoft.EntityFrameworkCore;
using PhoneInventory;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<PhoneDb>(opt =>     opt.UseInMemoryDatabase("PhoneList")); //dependency injection for PhoneDb
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();
RouteGroupBuilder phones = app.MapGroup("/phones");

phones.MapGet("/", GetAllPhones);
phones.MapPost("/", CreatePhone);
phones.MapGet("/{id}", GetPhone);
phones.MapDelete("/{id}", DeletePhone);
phones.MapPut("/{id}", UpdatePhone);
phones.MapGet("/available", GetAvailablePhones);
app.Run();

static async Task<IResult> GetAllPhones(PhoneDb db)
{
    return TypedResults.Ok(await db.Phones.Select(x => new PhoneDTO(x)).ToArrayAsync());
}
static async Task<IResult> CreatePhone(Phone phone, PhoneDb db)
{
    db.Phones.Add(phone);
    await db.SaveChangesAsync();
    var dto = new PhoneDTO(phone);
    return TypedResults.Created($"/phones/{phone.Id}", dto);
}
static async Task<IResult> GetPhone(int id, PhoneDb db)
{ 
    return await db.Phones.FindAsync(id)
        is Phone phone
        ? TypedResults.Ok(new PhoneDTO(phone))
        : TypedResults.NotFound();
}
static async Task<IResult> DeletePhone(int id, PhoneDb db)
{
    if (await db.Phones.FindAsync(id) is Phone phone)
    {
        db.Phones.Remove(phone);
        await db.SaveChangesAsync();
        return TypedResults.NoContent();
    }
    return TypedResults.NotFound();
}
static async Task<IResult> UpdatePhone(int id, PhoneDTO inputPhone, PhoneDb db)
{
    var phone = await db.Phones.FindAsync(id);
    if (phone is null) return TypedResults.NotFound();
    phone.Make = inputPhone.Make;
    phone.Model = inputPhone.Model;
    phone.IsAvailable = inputPhone.IsAvailable;
    await db.SaveChangesAsync();
    return TypedResults.Ok(new PhoneDTO(phone));
}
static async Task<IResult> GetAvailablePhones(PhoneDb db)
{
    return TypedResults.Ok(await db.Phones.Where(x => x.IsAvailable).Select(x => new PhoneDTO(x)).ToArrayAsync());
}