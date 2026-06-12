using FluentValidation;

namespace AntecLMS.Application.Features.Materials.Commands.CreateMaterial;

public class CreateMaterialValidator : AbstractValidator<CreateMaterialCommand>
{
  public CreateMaterialValidator()
  {
    RuleFor(x => x.LessonId).GreaterThan(0);
    RuleFor(x => x.GroupId).GreaterThan(0);
    RuleFor(x => x.TeacherId).GreaterThan(0);
    RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
    RuleFor(x => x.Type).NotEmpty().MaximumLength(50);
    RuleFor(x => x.Url).MaximumLength(1000);
    RuleFor(x => x.FilePath).MaximumLength(500);
    RuleFor(x => x.Description).MaximumLength(1000);
  }
}
