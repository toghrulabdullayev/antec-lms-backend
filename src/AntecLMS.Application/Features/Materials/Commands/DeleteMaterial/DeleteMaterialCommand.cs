using AntecLMS.Application.Common.Models;
using MediatR;

namespace AntecLMS.Application.Features.Materials.Commands.DeleteMaterial;

public record DeleteMaterialCommand(int Id) : IRequest<Result>;
