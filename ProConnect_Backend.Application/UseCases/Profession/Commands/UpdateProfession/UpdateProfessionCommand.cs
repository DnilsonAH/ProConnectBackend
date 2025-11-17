using MediatR;
using ProConnect_Backend.Application.DTOsResponse.ProfessionDTOs;
using ProConnect_Backend.Domain.DTOsRequest.ProfessionDTOs;

namespace ProConnect_Backend.Application.UseCases.Profession.Commands.UpdateProfession;

public record UpdateProfessionCommand(uint ProfessionId, UpdateProfessionRequestDto Dto) 
    : IRequest<ProfessionResponseDto>;
