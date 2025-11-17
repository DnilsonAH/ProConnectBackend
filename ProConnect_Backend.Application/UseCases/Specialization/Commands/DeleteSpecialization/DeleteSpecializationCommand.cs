using MediatR;

namespace ProConnect_Backend.Application.UseCases.Specialization.Commands.DeleteSpecialization;

public record DeleteSpecializationCommand(uint SpecializationId) : IRequest<bool>;
