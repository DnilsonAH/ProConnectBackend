using MediatR;
using ProConnect_Backend.Application.DTOsResponse.ProfessionCategoryDTOs;
using ProConnect_Backend.Domain.DTOsRequest.ProfessionCategoryDTOs;

namespace ProConnect_Backend.Application.UseCases.ProfessionCategory.Commands.CreateProfessionCategory;

public record CreateProfessionCategoryCommand(CreateProfessionCategoryRequestDto Dto) 
    : IRequest<ProfessionCategoryResponseDto>;
