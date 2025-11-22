using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ProConnect_Backend.Application.DTOsResponse.WeeklyAvailabilityDTOs;
using ProConnect_Backend.Domain.Ports;
using System.Security.Claims;

namespace ProConnect_Backend.Application.UseCases.WeeklyAvailability.Commands.UpdateWeeklyAvailabilityAdmin;

public class UpdateWeeklyAvailabilityAdminHandler : IRequestHandler<UpdateWeeklyAvailabilityAdminCommand, WeeklyAvailabilityResponseDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateWeeklyAvailabilityAdminHandler> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UpdateWeeklyAvailabilityAdminHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<UpdateWeeklyAvailabilityAdminHandler> logger,
        IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<WeeklyAvailabilityResponseDto> Handle(UpdateWeeklyAvailabilityAdminCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Admin updating WeeklyAvailability with ID: {WeeklyAvailabilityId} for Professional: {ProfessionalId}", request.Dto.WeeklyAvailabilityId, request.ProfessionalId);

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

        var entity = await _unitOfWork.WeeklyAvailabilityRepository.GetByIdAsync(request.Dto.WeeklyAvailabilityId);
        if (entity == null)
        {
            throw new KeyNotFoundException($"WeeklyAvailability with ID {request.Dto.WeeklyAvailabilityId} not found.");
        }

        // Verify that the availability belongs to the specified professional
        if (entity.ProfessionalId != request.ProfessionalId)
        {
             throw new ArgumentException($"The WeeklyAvailability with ID {request.Dto.WeeklyAvailabilityId} does not belong to Professional with ID {request.ProfessionalId}.");
        }

        if (request.Dto.WeekDay != null)
        {
            var validWeekDays = new[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
            if (!validWeekDays.Contains(request.Dto.WeekDay, StringComparer.OrdinalIgnoreCase))
            {
                throw new ArgumentException("Invalid WeekDay.");
            }
            entity.WeekDay = request.Dto.WeekDay;
        }

        if (request.Dto.StartTime != null)
        {
            if (TimeOnly.TryParse(request.Dto.StartTime, out var startTime))
            {
                entity.StartTime = startTime;
            }
            else
            {
                throw new ArgumentException("Invalid StartTime format.");
            }
        }

        if (request.Dto.EndTime != null)
        {
            if (TimeOnly.TryParse(request.Dto.EndTime, out var endTime))
            {
                entity.EndTime = endTime;
            }
            else
            {
                throw new ArgumentException("Invalid EndTime format.");
            }
        }

        if (entity.StartTime >= entity.EndTime)
        {
            throw new ArgumentException("StartTime must be before EndTime.");
        }

        if (request.Dto.IsAvailable.HasValue)
        {
            entity.IsAvailable = request.Dto.IsAvailable;
        }

        _unitOfWork.WeeklyAvailabilityRepository.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("WeeklyAvailability updated successfully by Admin.");

        return _mapper.Map<WeeklyAvailabilityResponseDto>(entity);
    }
}
