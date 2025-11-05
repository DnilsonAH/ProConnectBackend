using MediatR;
using ProConnect_Backend.Domain.Ports;

namespace ProConnect_Backend.Application.UseCases.Specialty.Command;

public class DeleteSpecialtyCommand : IRequest
{
    public uint SpecialtyId { get; set; }
}

public class DeleteSpecialtyCommandHandler : IRequestHandler<DeleteSpecialtyCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteSpecialtyCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteSpecialtyCommand request, CancellationToken cancellationToken)
    {
        var specialty = await _unitOfWork.SpecialtyRepository.GetByIdAsync(request.SpecialtyId);
        if (specialty == null)
        {
            throw new Exception($"LA ESPECIALIDAD CON EL ID {request.SpecialtyId} NO FUE ENCONTRADA.");
        }
        _unitOfWork.SpecialtyRepository.Delete(specialty);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
