using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ProConnect_Backend.Domain.Ports;
using System.Security.Claims;

namespace ProConnect_Backend.Application.UseCases.WeeklyAvailability.Commands.DeleteWeeklyAvailability;

public class DeleteWeeklyAvailabilityHandler : IRequestHandler<DeleteWeeklyAvailabilityCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteWeeklyAvailabilityHandler> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DeleteWeeklyAvailabilityHandler(
        IUnitOfWork unitOfWork,
        ILogger<DeleteWeeklyAvailabilityHandler> logger,
        IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task Handle(DeleteWeeklyAvailabilityCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting WeeklyAvailability with ID: {WeeklyAvailabilityId}", request.WeeklyAvailabilityId);

        var entity = await _unitOfWork.WeeklyAvailabilityRepository.GetByIdAsync(request.WeeklyAvailabilityId);
        if (entity == null)
        {
            throw new KeyNotFoundException($"WeeklyAvailability with ID {request.WeeklyAvailabilityId} not found.");
        }

        var user = _httpContextAccessor.HttpContext?.User;
        if (user == null)
        {
            throw new UnauthorizedAccessException("User is not authenticated.");
        }

        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
        var userId = uint.Parse(userIdClaim.Value);

        if (entity.ProfessionalId != userId)
        {
            throw new UnauthorizedAccessException("You are not authorized to delete this availability.");
        }

        _unitOfWork.WeeklyAvailabilityRepository.Delete(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("WeeklyAvailability deleted successfully.");
    }
}
