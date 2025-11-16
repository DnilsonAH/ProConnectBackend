using MediatR;
using ProConnect_Backend.Application.DTOsResponse.AuthDTOs;
using ProConnect_Backend.Domain.DTOsRequest.AuthDtos;

namespace ProConnect_Backend.Application.UseCases.Auth.Commands.Register;

public record RegisterCommand(RegisterRequestDto RegisterDto) : IRequest<RegisterResponseDto>;
