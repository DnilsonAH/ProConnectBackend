using MediatR;
using ProConnect_Backend.Application.DTOsResponse.SpecializationDTOs;

namespace ProConnect_Backend.Application.UseCases.Specialization.Queries.GetSpecializationsByProfession;

public record GetSpecializationsByProfessionQuery(uint ProfessionId) : IRequest<IEnumerable<SpecializationResponseDto>>;
