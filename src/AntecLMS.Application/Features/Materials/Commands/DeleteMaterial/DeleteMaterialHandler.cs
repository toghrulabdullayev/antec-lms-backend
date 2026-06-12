using AntecLMS.Application.Common.Exceptions;
using AntecLMS.Application.Common.Models;
using AntecLMS.Domain.Repositories;
using MediatR;

namespace AntecLMS.Application.Features.Materials.Commands.DeleteMaterial;

public class DeleteMaterialHandler : IRequestHandler<DeleteMaterialCommand, Result>
{
  private readonly IMaterialRepository _materials;
  private readonly IUnitOfWork _uow;

  public DeleteMaterialHandler(IMaterialRepository materials, IUnitOfWork uow)
  {
    _materials = materials;
    _uow = uow;
  }

  public async Task<Result> Handle(DeleteMaterialCommand request, CancellationToken ct)
  {
    var material =
      await _materials.GetByIdAsync(request.Id, ct)
      ?? throw new NotFoundException("Material", request.Id);

    _materials.Remove(material);
    await _uow.SaveChangesAsync(ct);
    return Result.Success();
  }
}
