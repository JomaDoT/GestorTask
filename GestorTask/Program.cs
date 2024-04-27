using Asp.Versioning;
using GestorTask.Application.Models;
using GestorTask.Infraestructure.Models;
using GestorTask.setup;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
string MyAllowSpecificOrigins = "gestortask_cors";

var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? string.Empty);

var tokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuer = true,
    ValidateAudience = true,
    ValidateLifetime = true,
    ValidateIssuerSigningKey = true,
    ValidIssuer = builder.Configuration["Jwt:Issuer"],
    ValidAudience = builder.Configuration["Jwt:Issuer"],
    IssuerSigningKey = new SymmetricSecurityKey(key),
    ClockSkew = TimeSpan.Zero
};
var connectionString = builder.Configuration.GetConnectionString("defaultconnection");

//CryptProvider
var securitySetting = new SecuritySetting();
builder.Configuration.GetSection(nameof(SecuritySetting)).Bind(securitySetting);

if (!builder.Services.Any(x => x.ServiceType == typeof(SecuritySetting)))
    builder.Services.AddSingleton(securitySetting);

builder.Services.RegisterServices(tokenValidationParameters,
              MyAllowSpecificOrigins,
              connectionString);

var app = builder.Build();

app.ConfigureProject(MyAllowSpecificOrigins);


app.Run();
