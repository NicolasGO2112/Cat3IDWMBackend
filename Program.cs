using Catedra3Backend.src.Data;
using Catedra3Backend.src.Helpers;
using Catedra3Backend.src.interfaces;
using Catedra3Backend.src.Models;
using Catedra3Backend.src.Repositoy;
using Catedra3Backend.src.Services;
using CloudinaryDotNet;
using DotNetEnv;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;


//cargamos las variables de entorno
Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<Seeders>();

// Configurar la base de datos SQLite
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlite(Environment.GetEnvironmentVariable("DATABASE_URL")));

// Configurar Identity
builder.Services.AddIdentity<User, IdentityRole<int>>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false; // Quitamos el requisito de símbolo
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = true;
    options.Password.RequiredUniqueChars = 1;
})
    .AddEntityFrameworkStores<DataContext>()
    .AddDefaultTokenProviders();
    
builder.Services.AddAuthentication(
                opt =>
                {
                    opt.DefaultAuthenticateScheme = 
                    opt.DefaultChallengeScheme = 
                    opt.DefaultForbidScheme =
                    opt.DefaultScheme =
                    opt.DefaultSignInScheme = 
                    opt.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
                }).AddJwtBearer(opt => {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                      ValidateIssuer = true,
                      ValidIssuer = Environment.GetEnvironmentVariable("JWT_IUSSER"),
                      ValidateAudience = true,
                      ValidAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE"),
                      ValidateIssuerSigningKey = true,
                      ValidateLifetime = true, // Habilita la validación de tiempo de vida
                      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_SIGNINKEY") ?? throw new ArgumentNullException("JWT_SIGINKEY"))),
                    };
                    
                });
        
// Configurar Cloudinary
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("Cloudinary"));

builder.Services.AddSingleton(serviceProvider =>
{
    var settings = serviceProvider.GetRequiredService<IOptions<CloudinarySettings>>().Value;
    var account = new Account(     
                settings.CloudName = Environment.GetEnvironmentVariable("CLOUDINARY_CLOUD_NAME")!,
                settings.ApiKey = Environment.GetEnvironmentVariable("CLOUDINARY_API_KEY")!,
                settings.ApiSecret = Environment.GetEnvironmentVariable("CLOUDINARY_API_SECRET")!);
    return new Cloudinary(account);
});
builder.Services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();
 builder.Services.AddCors(options =>
            {
                options.AddPolicy("allowAll", policy =>
                {
                    policy.WithOrigins("http://localhost:4200")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
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
app.UseCors("allowAll");

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
