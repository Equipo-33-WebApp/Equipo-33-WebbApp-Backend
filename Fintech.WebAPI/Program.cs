using DotNetEnv;
using Fintech.Application.Interfaces;
using Fintech.Application.Services;
using Fintech.Domain.Interfaces;
using Fintech.Infrastructure.MappingProfiles;
using Fintech.Infrastructure.Repositories;
using Supabase;

var builder = WebApplication.CreateBuilder(args);

// Load environment variables from .env file.
Env.Load();

// Add services to the container.

// Add Supabase client to the container.
var url = Environment.GetEnvironmentVariable("SUPABASE_URL");
var key = Environment.GetEnvironmentVariable("SUPABASE_KEY");

if (string.IsNullOrWhiteSpace(url) || string.IsNullOrWhiteSpace(key))
    throw new Exception("Faltan las variables de entorno SUPABASE_URL o SUPABASE_KEY");

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

// Add controllers to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
