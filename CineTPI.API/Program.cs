using CineTPI.API.Middleware; 
using CineTPI.Domain.Interfaces;
using CineTPI.Domain.Models;
using CineTPI.Domain.Repositories;
using CineTPI.Domain.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");


builder.Services.AddDbContext<CineDBContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Forzamos a que la API use camelCase (ej: "idFuncion")
        // en lugar de PascalCase (ej: "IdFuncion") para el JSON.
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IPeliculaRepository, PeliculaRepository>();
builder.Services.AddScoped<IFuncionRepository, FuncionRepository>();
builder.Services.AddScoped<IButacaRepository, ButacaRepository>();
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IButacaRepository, ButacaRepository>();
builder.Services.AddScoped<IReservaRepository, ReservaRepository>();

// 7. Registrar el AuthService
builder.Services.AddScoped<IAuthService, AuthService>();

// 8. Configurar Autenticación JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });


var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseDefaultFiles(); // <-- Busca index.html como página por defecto
app.UseStaticFiles();  // <-- Permite servir archivos desde wwwroot

app.UseHttpsRedirection();

app.UseAuthentication(); // <-- ¿Quién sos?
app.UseAuthorization();  // <-- ¿Tenés permiso?

app.MapControllers();

app.Run();