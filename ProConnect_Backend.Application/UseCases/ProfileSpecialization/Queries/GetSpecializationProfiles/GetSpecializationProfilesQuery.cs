using MediatR;
using ProConnect_Backend.Application.DTOsResponse.ProfileSpecializationDTOs;

namespace ProConnect_Backend.Application.UseCases.ProfileSpecialization.Queries.GetSpecializationProfiles;

public record GetSpecializationProfilesQuery(uint SpecializationId) : IRequest<IEnumerable<ProfileSpecializationResponseDto>>;
