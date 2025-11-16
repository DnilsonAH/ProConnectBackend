using MediatR;
using ProConnect_Backend.Application.DTOsResponse.ProfileSpecializationDTOs;
using ProConnect_Backend.Domain.DTOsRequest.ProfileSpecializationDTOs;

namespace ProConnect_Backend.Application.UseCases.ProfileSpecialization.Commands.AssignSpecialization;

public record AssignSpecializationCommand(AssignSpecializationRequestDto Dto) 
    : IRequest<ProfileSpecializationResponseDto>;
