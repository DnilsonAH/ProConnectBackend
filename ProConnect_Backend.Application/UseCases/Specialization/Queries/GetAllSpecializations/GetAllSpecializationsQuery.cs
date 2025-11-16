using MediatR;
using ProConnect_Backend.Application.DTOsResponse.SpecializationDTOs;

namespace ProConnect_Backend.Application.UseCases.Specialization.Queries.GetAllSpecializations;

public record GetAllSpecializationsQuery : IRequest<IEnumerable<SpecializationResponseDto>>;
