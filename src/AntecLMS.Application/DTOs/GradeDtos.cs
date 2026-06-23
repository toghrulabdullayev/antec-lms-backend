using AntecLMS.Application.Common.Models;

namespace AntecLMS.Application.DTOs;

public record CreateGradeDto(int StudentId, int Score, int MaxScore, string? TeacherNote);

public record UpdateGradeDto(int Score, int MaxScore, string? TeacherNote);

public record GradeResponse(
  int Id,
  int LessonId,
  int StudentId,
  int Score,
  int MaxScore,
  string? TeacherNote,
  DateTime CreatedAt
);



public record GradeJournalResponse(
    List<MyGradeItem> Items,
    int TotalLessons,
    double AveragePercentage,
    int MaxScore,
    int MinScore,
    bool CanAttendFinal,
    double? PredictedFinalGrade
);

public record MyGradeItem(
    int Id,
    string Topic,
    DateTime LessonDate,
    int Score,
    int MaxScore,
    string? TeacherNote
)
{
  public double Percentage => MaxScore > 0 ? (double)Score / MaxScore * 100 : 0;
};

