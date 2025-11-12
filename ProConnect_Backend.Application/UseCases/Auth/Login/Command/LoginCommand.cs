using MediatR;
using ProConnect_Backend.Application.DTOsResponse.LoginDTOs;
using ProConnect_Backend.Domain.DTOsRequest.AuthDtos;

namespace ProConnect_Backend.Application.UseCases.Login.Command;

public record LoginCommand(LoginRequestDto LoginDto) : IRequest<LoginResponseDto?>;