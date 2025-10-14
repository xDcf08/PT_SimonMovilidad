using Microsoft.AspNetCore.Authentication.JwtBearer;
using SimonMovilidad.API.Hubs;
using SimonMovilidad.API.Middleware;
using SimonMovilidad.API.OptionsSetup;
using SimonMovilidad.API.Services;
using SimonMovilidad.Application;
using SimonMovilidad.Application.Abstractions.Authentication;
using SimonMovilidad.Application.Abstractions.Notifier;
using SimonMovilidad.Persistence;
using SimonMovilidad.Persistence.Authentication;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

const string WebAppOrigin = "http://localhost:5173";

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowWebApp", 
        policy =>
        {
            policy.WithOrigins(WebAppOrigin)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

builder.Services.ConfigureOptions<JwtOptionsSetup>();
builder.Services.ConfigureOptions<JwtBearerOptionsSetup>();
builder.Services.AddTransient<IJwtProvider, JwtProvider>();
builder.Services.AddScoped<ITelemetryNotifier, TelemetryNotifier>();

builder.Services.AddSignalR();
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddApplication()
    .AddPersistence(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseCors("AllowWebApp");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHub<TelemetryHub>("/hubs/telemetryHub");

app.Run();
