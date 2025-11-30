using MediatR;

namespace ProConnect_Backend.Application.UseCases.ProfessionalProfile.Commands.DeleteProfessionalProfile;

public record DeleteProfessionalProfileCommand(uint ProfileId) : IRequest;
