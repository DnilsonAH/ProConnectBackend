using MediatR;
using ProConnect_Backend.Application.DTOsResponse.ProfessionCategoryDTOs;

namespace ProConnect_Backend.Application.UseCases.ProfessionCategory.Queries.GetAllProfessionCategories;

public record GetAllProfessionCategoriesQuery : IRequest<IEnumerable<ProfessionCategoryResponseDto>>;
