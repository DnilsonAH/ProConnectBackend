using MediatR;

namespace ProConnect_Backend.Application.UseCases.WeeklyAvailability.Commands.DeleteWeeklyAvailabilityAdmin;

public record DeleteWeeklyAvailabilityAdminCommand(uint ProfessionalId, uint WeeklyAvailabilityId) : IRequest;
