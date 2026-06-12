using AntecLMS.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AntecLMS.Application;

public static class DependencyInjection
{
  public static IServiceCollection AddApplication(this IServiceCollection services)
  {
    services.AddScoped<ICourseService, CourseService>();
    services.AddScoped<IGroupService, GroupService>();
    services.AddScoped<IStudentService, StudentService>();
    services.AddScoped<ITeacherService, TeacherService>();
    services.AddScoped<IUserService, UserService>();
    services.AddScoped<ILessonService, LessonService>();
    services.AddScoped<IAttendanceService, AttendanceService>();
    services.AddScoped<IGradeService, GradeService>();
    services.AddScoped<IMaterialService, MaterialService>();
    services.AddScoped<IAuthService, AuthService>();
    services.AddScoped<IDashboardService, DashboardService>();
    services.AddScoped<ITeacherDashboardService, TeacherDashboardService>();
    services.AddScoped<IReportService, ReportService>();

    return services;
  }
}
