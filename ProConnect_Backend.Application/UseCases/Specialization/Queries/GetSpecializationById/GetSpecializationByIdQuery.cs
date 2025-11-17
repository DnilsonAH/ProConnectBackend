using MediatR;
using ProConnect_Backend.Application.DTOsResponse.SpecializationDTOs;

namespace ProConnect_Backend.Application.UseCases.Specialization.Queries.GetSpecializationById;

public record GetSpecializationByIdQuery(uint SpecializationId) : IRequest<SpecializationResponseDto?>;
