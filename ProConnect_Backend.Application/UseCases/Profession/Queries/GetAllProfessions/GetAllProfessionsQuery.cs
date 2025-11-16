using MediatR;
using ProConnect_Backend.Application.DTOsResponse.ProfessionDTOs;

namespace ProConnect_Backend.Application.UseCases.Profession.Queries.GetAllProfessions;

public record GetAllProfessionsQuery : IRequest<IEnumerable<ProfessionResponseDto>>;
