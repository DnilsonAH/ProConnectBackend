using MediatR;

namespace ProConnect_Backend.Application.UseCases.Auth.Commands.Logout;

public record LogoutCommand(string Token) : IRequest<bool>;

