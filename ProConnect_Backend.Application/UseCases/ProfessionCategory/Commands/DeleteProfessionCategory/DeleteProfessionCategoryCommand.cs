using MediatR;

namespace ProConnect_Backend.Application.UseCases.ProfessionCategory.Commands.DeleteProfessionCategory;

public record DeleteProfessionCategoryCommand(uint CategoryId) : IRequest<bool>;
