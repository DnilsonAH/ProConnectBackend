using MediatR;
using ProConnect_Backend.Application.DTOsResponse.ProfessionalProfileDTOs;

namespace ProConnect_Backend.Application.UseCases.ProfessionalProfile.Queries.GetProfessionalProfileById;

public record GetProfessionalProfileByIdQuery(uint ProfileId) : IRequest<ProfessionalProfileResponseDto?>;
