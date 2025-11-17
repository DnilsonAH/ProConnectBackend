using MediatR;
using ProConnect_Backend.Application.DTOsResponse.UserDTOs;

namespace ProConnect_Backend.Application.UseCases.Users.Queries.GetUserInfo;

public record GetUserByIdQuery(uint UserId) : IRequest<GetUserInfoResponseDto?>;
