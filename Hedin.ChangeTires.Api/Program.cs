using FluentValidation;
using Hedin.ChangeTires.Api;
using Hedin.ChangeTires.Api.Configurations;
using Hedin.ChangeTires.Api.ExternalIntegrations;
using Hedin.ChangeTires.Api.Services;
using Hedin.ChangeTires.Api.Services.Interfaces;
using Hedin.ChangeTires.Api.Strategies;
using Hedin.ChangeTires.Api.Strategies.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ExternalServiceSettings>(builder.Configuration.GetSection("ExternalServiceSettings"));

builder.Services.AddSingleton(resolver =>
    resolver.GetRequiredService<IOptions<ExternalServiceSettings>>().Value);

builder.Services.AddTransient<IPricingService, PricingService>();
builder.Services.AddTransient<ITirePricing, TirePricing>();
builder.Services.AddTransient<ICarTypePricing, CarTypePricing>();
builder.Services.AddTransient<IBookingService, BookingService>();
builder.Services.AddTransient<IExternalSlotApiClient, ExternalSlotApiClient>();

builder.Services.AddValidatorsFromAssemblyContaining(typeof(CarValidator));
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(cfg =>
    {
        cfg.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidIssuer = builder.Configuration["JwtIssuer"],
            ValidAudience = builder.Configuration["JwtIssuer"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JwtKey"]))
        };
    });

builder.Services.AddAuthorization();
//Add support to logging with SERILOG
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddDbContext<CarContext>(opt => opt.UseInMemoryDatabase("CarDB"));

builder.Services.AddHttpClient<ExternalSlotApiClient>((serviceProvider, client) =>
{
    var settings = serviceProvider.GetRequiredService<ExternalServiceSettings>();
    client.BaseAddress = new Uri(settings.Url);
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
   {
     new OpenApiSecurityScheme
     {
       Reference = new OpenApiReference
       {
         Type = ReferenceType.SecurityScheme,
         Id = "Bearer"
       }
      },
      new string[] { }
    }
  });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    };
});

var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";

var app = builder.Build();

//Add support to logging request with SERILOG
app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

var externalServiceSettings = app.Services.GetRequiredService<ExternalServiceSettings>();

app.RegisterEndpoints();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseHttpsRedirection();

app.Run();

public partial class Program
{ }

