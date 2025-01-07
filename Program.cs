using Catedra3Backend.src.Data;
using Catedra3Backend.src.Helpers;
using Catedra3Backend.src.Models;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<Seeders>();

// Configurar la base de datos SQLite
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlite("Data Source=Cat3.db"));

// Configurar Identity
builder.Services.AddIdentity<User, IdentityRole<int>>()
    .AddEntityFrameworkStores<DataContext>()
    .AddDefaultTokenProviders();

// Configurar Cloudinary
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("Cloudinary"));

builder.Services.AddSingleton(serviceProvider =>
{
    var settings = serviceProvider.GetRequiredService<IOptions<CloudinarySettings>>().Value;
    var account = new Account(settings.CloudName, settings.ApiKey, settings.ApiSecret);
    return new Cloudinary(account);
});

// Configurar controladores
builder.Services.AddControllers();

var app = builder.Build();

// Configurar migraciones y seeding
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<DataContext>();
    
    // Aplica migraciones
    await context.Database.MigrateAsync();
    
    // Realiza el seeding
    Console.WriteLine("Ejecutando el método Seed...");
    await Seeders.Seed(services);
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
