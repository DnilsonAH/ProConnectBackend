using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ProConnect_Backend.Application.DTOsResponse.WeeklyAvailabilityDTOs;
using ProConnect_Backend.Domain.Entities;
using ProConnect_Backend.Domain.Ports;
using System.Security.Claims;

namespace ProConnect_Backend.Application.UseCases.WeeklyAvailability.Commands.CreateWeeklyAvailability;

public class CreateWeeklyAvailabilityHandler : IRequestHandler<CreateWeeklyAvailabilityCommand, WeeklyAvailabilityResponseDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateWeeklyAvailabilityHandler> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CreateWeeklyAvailabilityHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<CreateWeeklyAvailabilityHandler> logger,
        IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<WeeklyAvailabilityResponseDto> Handle(CreateWeeklyAvailabilityCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating new WeeklyAvailability");

        var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            throw new UnauthorizedAccessException("User is not authenticated.");
        }

        var userId = uint.Parse(userIdClaim.Value);

        // Validate TimeOnly parsing
        if (!TimeOnly.TryParse(request.Dto.StartTime, out var startTime))
        {
            throw new ArgumentException("Invalid StartTime format.");
        }

        if (!TimeOnly.TryParse(request.Dto.EndTime, out var endTime))
        {
            throw new ArgumentException("Invalid EndTime format.");
        }

        if (startTime >= endTime)
        {
            throw new ArgumentException("StartTime must be before EndTime.");
        }

        // Validate WeekDay
        var validWeekDays = new[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
        if (!validWeekDays.Contains(request.Dto.WeekDay, StringComparer.OrdinalIgnoreCase))
        {
             throw new ArgumentException("Invalid WeekDay.");
        }

        var entity = new Domain.Entities.WeeklyAvailability
        {
            ProfessionalId = userId,
            WeekDay = request.Dto.WeekDay,
            StartTime = startTime,
            EndTime = endTime,
            IsAvailable = request.Dto.IsAvailable
        };

        await _unitOfWork.WeeklyAvailabilityRepository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("WeeklyAvailability created successfully with ID: {WeeklyAvailabilityId}", entity.WeeklyAvailabilityId);

        return _mapper.Map<WeeklyAvailabilityResponseDto>(entity);
    }
}
