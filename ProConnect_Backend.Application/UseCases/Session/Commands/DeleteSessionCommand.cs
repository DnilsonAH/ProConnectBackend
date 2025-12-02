using MediatR;
using ProConnect_Backend.Domain.Ports;

namespace ProConnect_Backend.Application.UseCases.Session.Commands;

public class DeleteSessionCommand : IRequest<bool>
{
    public uint Id { get; set; }
}

public class DeleteSessionCommandHandler : IRequestHandler<DeleteSessionCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteSessionCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteSessionCommand request, CancellationToken cancellationToken)
    {
        var session = await _unitOfWork.SessionRepository.GetByIdAsync(request.Id);
        if (session == null)
        {
            return false;
        }

        _unitOfWork.SessionRepository.Delete(session);
        var result = await _unitOfWork.SaveChangesAsync();

        return result > 0;
    }
}
