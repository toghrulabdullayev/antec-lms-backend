using AntecLMS.Application.Common.Models;
using AntecLMS.Application.DTOs;

namespace AntecLMS.Application.Services;

public interface IStudentPortalService
{
  Task<Result<MyDashboardResponse>> GetMyDashboardAsync(CancellationToken ct);
  Task<Result<List<MyGroupDetail>>> GetMyGroupsAsync(CancellationToken ct);
  Task<Result<List<MyLessonItem>>> GetMyLessonsAsync(CancellationToken ct);

  Task<Result<MyProfileResponse>> GetMyProfileAsync(CancellationToken ct);
  Task<Result<AttendanceJournalResponse>> GetAttendanceJournalAsync(
    DateTime? start,
    DateTime? end,
    CancellationToken ct
  );
  Task<Result<List<MaterialItem>>> GetMyMaterialsAsync(string? type, CancellationToken ct);
  Task<Result> ChangePasswordAsync(
    string currentPassword,
    string newPassword,
    CancellationToken ct
  );
}
