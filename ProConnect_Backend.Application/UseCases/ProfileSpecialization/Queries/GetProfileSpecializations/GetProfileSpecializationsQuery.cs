using MediatR;
using ProConnect_Backend.Application.DTOsResponse.ProfileSpecializationDTOs;

namespace ProConnect_Backend.Application.UseCases.ProfileSpecialization.Queries.GetProfileSpecializations;

public record GetProfileSpecializationsQuery(uint ProfileId) : IRequest<IEnumerable<ProfileSpecializationResponseDto>>;
