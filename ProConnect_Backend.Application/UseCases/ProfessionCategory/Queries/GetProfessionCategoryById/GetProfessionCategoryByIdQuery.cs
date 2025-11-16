using MediatR;
using ProConnect_Backend.Application.DTOsResponse.ProfessionCategoryDTOs;

namespace ProConnect_Backend.Application.UseCases.ProfessionCategory.Queries.GetProfessionCategoryById;

public record GetProfessionCategoryByIdQuery(uint CategoryId) : IRequest<ProfessionCategoryResponseDto?>;
