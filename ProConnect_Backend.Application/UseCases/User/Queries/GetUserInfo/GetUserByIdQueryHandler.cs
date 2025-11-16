using MediatR;
using ProConnect_Backend.Application.DTOsResponse.UserDTOs;
using ProConnect_Backend.Domain.Ports;

namespace ProConnect_Backend.Application.UseCases.Users.Queries.GetUserInfo;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, GetUserInfoResponseDto?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetUserByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<GetUserInfoResponseDto?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.UserRepository.GetByIdAsync(request.UserId);

        if (user == null)
        {
            return null;
        }

        return new GetUserInfoResponseDto
        {
            UserId = user.UserId,
            FirstName = user.FirstName,
            LastName = user.FirstSurname,
            Email = user.Email,
            Role = user.Role,
            Phone = user.PhoneNumber,
            Country = "", // No tenemos campo Country en User
            RegistrationDate = user.RegistrationDate
        };
    }
}
