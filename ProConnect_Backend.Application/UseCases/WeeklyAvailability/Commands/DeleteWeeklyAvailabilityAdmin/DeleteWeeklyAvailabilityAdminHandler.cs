using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ProConnect_Backend.Domain.Ports;
using System.Security.Claims;

namespace ProConnect_Backend.Application.UseCases.WeeklyAvailability.Commands.DeleteWeeklyAvailabilityAdmin;

public class DeleteWeeklyAvailabilityAdminHandler : IRequestHandler<DeleteWeeklyAvailabilityAdminCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteWeeklyAvailabilityAdminHandler> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DeleteWeeklyAvailabilityAdminHandler(
        IUnitOfWork unitOfWork,
        ILogger<DeleteWeeklyAvailabilityAdminHandler> logger,
        IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task Handle(DeleteWeeklyAvailabilityAdminCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Admin deleting WeeklyAvailability with ID: {WeeklyAvailabilityId} for Professional: {ProfessionalId}", request.WeeklyAvailabilityId, request.ProfessionalId);

        var user = _httpContextAccessor.HttpContext?.User;
        if (user == null)
        {
            throw new UnauthorizedAccessException("User is not authenticated.");
        }

        var userRoleClaim = user.FindFirst(ClaimTypes.Role);
        if (userRoleClaim == null || userRoleClaim.Value != "Admin")
        {
             throw new UnauthorizedAccessException("Only Admins can perform this action.");
        }

        var entity = await _unitOfWork.WeeklyAvailabilityRepository.GetByIdAsync(request.WeeklyAvailabilityId);
        if (entity == null)
        {
            throw new KeyNotFoundException($"WeeklyAvailability with ID {request.WeeklyAvailabilityId} not found.");
        }

        // Verify that the availability belongs to the specified professional
        if (entity.ProfessionalId != request.ProfessionalId)
        {
             throw new ArgumentException($"The WeeklyAvailability with ID {request.WeeklyAvailabilityId} does not belong to Professional with ID {request.ProfessionalId}.");
        }

        _unitOfWork.WeeklyAvailabilityRepository.Delete(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("WeeklyAvailability deleted successfully by Admin.");
    }
}
