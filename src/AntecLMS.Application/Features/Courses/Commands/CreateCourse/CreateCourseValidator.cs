using FluentValidation;

namespace AntecLMS.Application.Features.Courses.Commands.CreateCourse;

public class CreateCourseValidator : AbstractValidator<CreateCourseCommand>
{
  public CreateCourseValidator()
  {
    RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
    RuleFor(x => x.Price).GreaterThanOrEqualTo(0);
    RuleFor(x => x.ImageUrl).MaximumLength(500);
    RuleFor(x => x.Status)
      .NotEmpty()
      .Must(s => new[] { "active", "inactive" }.Contains(s.ToLower()));
  }
}
