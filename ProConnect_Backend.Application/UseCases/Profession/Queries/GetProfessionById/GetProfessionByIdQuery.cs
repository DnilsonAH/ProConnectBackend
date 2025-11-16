using MediatR;
using ProConnect_Backend.Application.DTOsResponse.ProfessionDTOs;

namespace ProConnect_Backend.Application.UseCases.Profession.Queries.GetProfessionById;

public record GetProfessionByIdQuery(uint ProfessionId) : IRequest<ProfessionResponseDto?>;
