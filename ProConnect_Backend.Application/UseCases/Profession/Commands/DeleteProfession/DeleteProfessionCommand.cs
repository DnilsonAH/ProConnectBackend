using MediatR;

namespace ProConnect_Backend.Application.UseCases.Profession.Commands.DeleteProfession;

public record DeleteProfessionCommand(uint ProfessionId) : IRequest<bool>;
