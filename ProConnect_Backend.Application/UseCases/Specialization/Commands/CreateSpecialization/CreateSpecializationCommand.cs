using MediatR;
using ProConnect_Backend.Application.DTOsResponse.SpecializationDTOs;
using ProConnect_Backend.Domain.DTOsRequest.SpecializationDTOs;

namespace ProConnect_Backend.Application.UseCases.Specialization.Commands.CreateSpecialization;

public record CreateSpecializationCommand(CreateSpecializationRequestDto Dto) 
    : IRequest<SpecializationResponseDto>;
