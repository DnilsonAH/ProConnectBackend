using MediatR;

namespace ProConnect_Backend.Application.UseCases.WeeklyAvailability.Commands.DeleteWeeklyAvailability;

public record DeleteWeeklyAvailabilityCommand(uint WeeklyAvailabilityId) : IRequest;
