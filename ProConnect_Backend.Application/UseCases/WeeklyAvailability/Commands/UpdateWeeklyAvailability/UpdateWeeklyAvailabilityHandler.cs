using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ProConnect_Backend.Application.DTOsResponse.WeeklyAvailabilityDTOs;
using ProConnect_Backend.Domain.Ports;
using System.Security.Claims;

namespace ProConnect_Backend.Application.UseCases.WeeklyAvailability.Commands.UpdateWeeklyAvailability;

public class UpdateWeeklyAvailabilityHandler : IRequestHandler<UpdateWeeklyAvailabilityCommand, WeeklyAvailabilityResponseDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateWeeklyAvailabilityHandler> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UpdateWeeklyAvailabilityHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<UpdateWeeklyAvailabilityHandler> logger,
        IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<WeeklyAvailabilityResponseDto> Handle(UpdateWeeklyAvailabilityCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating WeeklyAvailability with ID: {WeeklyAvailabilityId}", request.Dto.WeeklyAvailabilityId);

        var entity = await _unitOfWork.WeeklyAvailabilityRepository.GetByIdAsync(request.Dto.WeeklyAvailabilityId);
        if (entity == null)
        {
            throw new KeyNotFoundException($"WeeklyAvailability with ID {request.Dto.WeeklyAvailabilityId} not found.");
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
            throw new UnauthorizedAccessException("You are not authorized to update this availability.");
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

        _logger.LogInformation("WeeklyAvailability updated successfully.");

        return _mapper.Map<WeeklyAvailabilityResponseDto>(entity);
    }
}
