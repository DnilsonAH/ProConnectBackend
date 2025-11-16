using MediatR;
using ProConnect_Backend.Application.DTOsResponse.LoginDTOs;
using ProConnect_Backend.Domain.DTOsRequest.AuthDtos;

namespace ProConnect_Backend.Application.UseCases.Auth.Commands.Login;

public record LoginCommand(LoginRequestDto LoginDto) : IRequest<LoginResponseDto?>;