using Supabase;
using Fintech.Application.Interfaces;
using Fintech.Application.Services;
using Fintech.Domain.Interfaces;
using Fintech.Infrastructure.MappingProfiles;
using Fintech.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using DotNetEnv.Configuration;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add .env file to the container.
builder.Configuration.AddDotNetEnv();

// Add services to the container.

// Add Supabase client to the container.
var url = builder.Configuration["SUPABASE_URL"];
var key = builder.Configuration["SUPABASE_KEY"];
var secret = builder.Configuration["SUPABASE_JWT_SECRET"];

if (string.IsNullOrWhiteSpace(url) || string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(secret))
    throw new Exception("Faltan las variables de entorno SUPABASE_URL, SUPABASE_KEY o SUPABASE_JWT_SECRET");

var options = new SupabaseOptions
{
    AutoRefreshToken = true,
    AutoConnectRealtime = true,
};
builder.Services.AddSingleton(provider => new Client(url, key, options));

// Add AutoMapper to the container.
builder.Services.AddAutoMapper(cfg => { }, typeof(UserProfile));

// Add repositories to the container.
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Add services to the container.
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Add controllers to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwksUrl = $"{url}/auth/v1/.well-known/jwks.json";

        using var client = new HttpClient();
        var json = client.GetStringAsync(jwksUrl).Result; // Realizar la llamada de forma síncrona
        var jwks = new JsonWebKeySet(json);
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            RequireExpirationTime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKeys = jwks.Keys,
            ValidIssuer = $"{url}/auth/v1",
            ValidAudience = "authenticated"
        };
    });
builder.Services.AddAuthorization();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "API", Version = "v1" });

    var securitySchema = new OpenApiSecurityScheme
    {
        Description = "Autorización JWT usando el esquema Bearer. Ingresa 'Bearer' [espacio] y luego tu token en el campo de abajo. Ejemplo: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    };

    c.AddSecurityDefinition("Bearer", securitySchema);

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Middlewares

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHttpsRedirection();
}

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
