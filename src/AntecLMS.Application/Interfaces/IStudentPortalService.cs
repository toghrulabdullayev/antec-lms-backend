using AntecLMS.Application.Common.Models;
using AntecLMS.Application.DTOs;

namespace AntecLMS.Application.Services;

public interface IStudentPortalService
{
  Task<Result<MyDashboardResponse>> GetMyDashboardAsync(CancellationToken ct);
  Task<Result<List<MyLessonItem>>> GetMyLessonsAsync(CancellationToken ct);
  Task<Result<List<MyAttendanceItem>>> GetMyAttendanceAsync(CancellationToken ct);
  Task<Result<List<MyGradeItem>>> GetMyGradesAsync(CancellationToken ct);
  Task<Result<List<MyMaterialDetail>>> GetMyMaterialsAsync(CancellationToken ct);
  Task<Result<MyProfileResponse>> GetMyProfileAsync(CancellationToken ct);
  Task<Result> ChangePasswordAsync(
    string currentPassword,
    string newPassword,
    CancellationToken ct
  );
}
