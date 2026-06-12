using System.Text;
using AntecLMS.Application.Common.Interfaces;
using AntecLMS.Domain.Repositories;
using AntecLMS.Infrastructure.Persistence;
using AntecLMS.Infrastructure.Persistence.Repositories;
using AntecLMS.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace AntecLMS.Infrastructure;

public static class DependencyInjection
{
  public static IServiceCollection AddInfrastructure(
    this IServiceCollection services,
    IConfiguration config
  )
  {
    services.AddDbContext<AppDbContext>(options =>
      options.UseNpgsql(config.GetConnectionString("Default")).UseSnakeCaseNamingConvention()
    );

    services.AddScoped<IUserRepository, UserRepository>();
    services.AddScoped<ICourseRepository, CourseRepository>();
    services.AddScoped<IGroupRepository, GroupRepository>();
    services.AddScoped<ITeacherRepository, TeacherRepository>();
    services.AddScoped<IStudentRepository, StudentRepository>();
    services.AddScoped<IAttendanceRepository, AttendanceRepository>();
    services.AddScoped<IGradeRepository, GradeRepository>();
    services.AddScoped<ILessonRepository, LessonRepository>();
    services.AddScoped<IMaterialRepository, MaterialRepository>();
    services.AddScoped<IUnitOfWork, UnitOfWork>();

    services.AddSingleton<IJwtService, JwtService>();
    services.AddScoped<ICurrentUserService, CurrentUserService>();
    services.AddScoped<IPasswordHasher, PasswordHasher>();
    services.AddHttpContextAccessor();

    services
      .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
      .AddJwtBearer(options =>
      {
        options.TokenValidationParameters = new TokenValidationParameters
        {
          ValidateIssuer = true,
          ValidateAudience = true,
          ValidateLifetime = true,
          ValidateIssuerSigningKey = true,
          ValidIssuer = config["Jwt:Issuer"],
          ValidAudience = config["Jwt:Audience"],
          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!)),
        };

        options.Events = new JwtBearerEvents
        {
          OnTokenValidated = context =>
          {
            var jwtSvc = context.HttpContext.RequestServices.GetRequiredService<IJwtService>();
            var token = context
              .HttpContext.Request.Headers["Authorization"]
              .ToString()
              .Replace("Bearer ", "");
            if (jwtSvc.IsTokenBlacklisted(token))
              context.Fail("Token etibarsızdır.");

            return Task.CompletedTask;
          },
        };
      });

    services.AddAuthorization();

    return services;
  }
}
