using MediatR;
using ProConnect_Backend.Application.DTOsResponse.ProfessionCategoryDTOs;
using ProConnect_Backend.Domain.DTOsRequest.ProfessionCategoryDTOs;

namespace ProConnect_Backend.Application.UseCases.ProfessionCategory.Commands.UpdateProfessionCategory;

public record UpdateProfessionCategoryCommand(uint CategoryId, UpdateProfessionCategoryRequestDto Dto) 
    : IRequest<ProfessionCategoryResponseDto>;
