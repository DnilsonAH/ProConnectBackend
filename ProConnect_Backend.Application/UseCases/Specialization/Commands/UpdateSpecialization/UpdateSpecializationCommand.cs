using MediatR;
using ProConnect_Backend.Application.DTOsResponse.SpecializationDTOs;
using ProConnect_Backend.Domain.DTOsRequest.SpecializationDTOs;

namespace ProConnect_Backend.Application.UseCases.Specialization.Commands.UpdateSpecialization;

public record UpdateSpecializationCommand(uint SpecializationId, UpdateSpecializationRequestDto Dto) 
    : IRequest<SpecializationResponseDto>;
