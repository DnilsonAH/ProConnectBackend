using MediatR;
using ProConnect_Backend.Application.DTOsResponse.UserDTOs;
using ProConnect_Backend.Domain.DTOsRequest.UserDTOs;

namespace ProConnect_Backend.Application.UseCases.Users.Commands.UpdateUser;

public record UpdateUserCommand(uint UserId, UpdateUserRequestDto UpdateDto) : IRequest<GetUserInfoResponseDto?>;
