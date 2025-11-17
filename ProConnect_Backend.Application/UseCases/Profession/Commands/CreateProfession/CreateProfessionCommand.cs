using MediatR;
using ProConnect_Backend.Application.DTOsResponse.ProfessionDTOs;
using ProConnect_Backend.Domain.DTOsRequest.ProfessionDTOs;

namespace ProConnect_Backend.Application.UseCases.Profession.Commands.CreateProfession;

public record CreateProfessionCommand(CreateProfessionRequestDto Dto) 
    : IRequest<ProfessionResponseDto>;
