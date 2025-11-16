using MediatR;

namespace ProConnect_Backend.Application.UseCases.ProfileSpecialization.Commands.RemoveSpecialization;

public record RemoveSpecializationCommand(uint ProfileId, uint SpecializationId) : IRequest<bool>;
