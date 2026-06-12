using FluentValidation;

namespace AntecLMS.Application.Features.Attendances.Commands.CreateAttendance;

public class CreateAttendanceValidator : AbstractValidator<CreateAttendanceCommand>
{
  public CreateAttendanceValidator()
  {
    RuleFor(x => x.LessonId).GreaterThan(0);
    RuleFor(x => x.StudentId).GreaterThan(0);
    RuleFor(x => x.Status)
      .NotEmpty()
      .Must(s => new[] { "present", "absent", "late", "excused" }.Contains(s.ToLower()));
    RuleFor(x => x.Reason).MaximumLength(500);
    RuleFor(x => x.TeacherNote).MaximumLength(500);
  }
}
