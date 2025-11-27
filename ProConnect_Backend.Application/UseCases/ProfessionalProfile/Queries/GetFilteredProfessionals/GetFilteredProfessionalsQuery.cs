using MediatR;
using ProConnect_Backend.Application.DTOsResponse.ProfessionalProfileDTOs;
using ProConnect_Backend.Domain.DTOsRequest.ProfessionalProfileDTOs;

namespace ProConnect_Backend.Application.UseCases.ProfessionalProfile.Queries.GetFilteredProfessionals;

public record GetFilteredProfessionalsQuery(FilterProfessionalsRequestDto FilterDto) 
    : IRequest<PagedProfessionalResponseDto>;