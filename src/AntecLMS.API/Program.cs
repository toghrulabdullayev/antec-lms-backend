using System.Text.Json;
using AntecLMS.API.Middleware;
using AntecLMS.Application;
using AntecLMS.Infrastructure;
using AntecLMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder
  .Services.AddControllers()
  .AddJsonOptions(options =>
  {
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
  });
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
  c.SwaggerDoc("v1", new OpenApiInfo { Title = "AntecLMS API", Version = "v1" });
  c.AddSecurityDefinition(
    "Bearer",
    new OpenApiSecurityScheme
    {
      Name = "Authorization",
      Type = SecuritySchemeType.Http,
      Scheme = "Bearer",
      BearerFormat = "JWT",
      In = ParameterLocation.Header,
      Description = "JWT token daxil edin: Bearer {token}",
    }
  );
  c.AddSecurityRequirement(
    new OpenApiSecurityRequirement
    {
      {
        new OpenApiSecurityScheme
        {
          Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" },
        },
        Array.Empty<string>()
      },
    }
  );
});

builder.Services.AddCors(options =>
{
  options.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
  var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
  await db.Database.MigrateAsync();
}

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

await app.RunAsync();
