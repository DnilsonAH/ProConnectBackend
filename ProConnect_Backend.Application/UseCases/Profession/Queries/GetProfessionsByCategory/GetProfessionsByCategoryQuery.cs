using MediatR;
using ProConnect_Backend.Application.DTOsResponse.ProfessionDTOs;

namespace ProConnect_Backend.Application.UseCases.Profession.Queries.GetProfessionsByCategory;

public record GetProfessionsByCategoryQuery(uint CategoryId) : IRequest<IEnumerable<ProfessionResponseDto>>;
