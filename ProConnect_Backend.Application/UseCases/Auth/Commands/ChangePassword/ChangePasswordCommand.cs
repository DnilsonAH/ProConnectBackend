using MediatR;
using ProConnect_Backend.Application.DTOsResponse.AuthDTOs;
using ProConnect_Backend.Domain.DTOsRequest.AuthDtos;

namespace ProConnect_Backend.Application.UseCases.Auth.Commands.ChangePassword;

public record ChangePasswordCommand(uint UserId, ChangePasswordRequestDto RequestDto) : IRequest<ChangePasswordResponseDto>;
