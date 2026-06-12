using FluentValidation;

namespace AntecLMS.Application.Features.Grades.Commands.CreateGrade;

public class CreateGradeValidator : AbstractValidator<CreateGradeCommand>
{
  public CreateGradeValidator()
  {
    RuleFor(x => x.LessonId).GreaterThan(0);
    RuleFor(x => x.StudentId).GreaterThan(0);
    RuleFor(x => x.Score).InclusiveBetween(0, 1000);
    RuleFor(x => x.MaxScore).InclusiveBetween(1, 1000);
    RuleFor(x => x.TeacherNote).MaximumLength(500);
  }
}
