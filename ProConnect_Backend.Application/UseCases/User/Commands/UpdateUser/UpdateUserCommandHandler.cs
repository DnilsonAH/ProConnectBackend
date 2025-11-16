using MediatR;
using ProConnect_Backend.Application.DTOsResponse.UserDTOs;
using ProConnect_Backend.Domain.Ports;

namespace ProConnect_Backend.Application.UseCases.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, GetUserInfoResponseDto?>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateUserCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<GetUserInfoResponseDto?> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.UserRepository.GetByIdAsync(request.UserId);

        if (user == null)
        {
            return null;
        }

        // Actualizar campos si se proporcionan
        if (!string.IsNullOrWhiteSpace(request.UpdateDto.FirstName))
            user.FirstName = request.UpdateDto.FirstName;

        if (!string.IsNullOrWhiteSpace(request.UpdateDto.SecondName))
            user.SecondName = request.UpdateDto.SecondName;

        if (!string.IsNullOrWhiteSpace(request.UpdateDto.FirstSurname))
            user.FirstSurname = request.UpdateDto.FirstSurname;

        if (!string.IsNullOrWhiteSpace(request.UpdateDto.SecondSurname))
            user.SecondSurname = request.UpdateDto.SecondSurname;

        if (!string.IsNullOrWhiteSpace(request.UpdateDto.PhoneNumber))
            user.PhoneNumber = request.UpdateDto.PhoneNumber;

        if (!string.IsNullOrWhiteSpace(request.UpdateDto.PhotoUrl))
            user.PhotoUrl = request.UpdateDto.PhotoUrl;

        _unitOfWork.UserRepository.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new GetUserInfoResponseDto
        {
            UserId = user.UserId,
            FirstName = user.FirstName,
            LastName = user.FirstSurname,
            Email = user.Email,
            Role = user.Role,
            Phone = user.PhoneNumber,
            Country = "",
            RegistrationDate = user.RegistrationDate
        };
    }
}
