using FluentValidation;

namespace AntecLMS.Application.Features.Lessons.Commands.CreateLesson;

public class CreateLessonValidator : AbstractValidator<CreateLessonCommand>
{
  public CreateLessonValidator()
  {
    RuleFor(x => x.GroupId).GreaterThan(0);
    RuleFor(x => x.TeacherId).GreaterThan(0);
    RuleFor(x => x.LessonDate).NotEmpty();
    RuleFor(x => x.Topic).NotEmpty().MaximumLength(500);
    RuleFor(x => x.Status)
      .Must(s => s == null || new[] { "draft", "completed" }.Contains(s.ToLower()));
  }
}
