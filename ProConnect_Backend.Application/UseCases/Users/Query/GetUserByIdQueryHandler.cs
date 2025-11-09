using AutoMapper;
using ProConnect_Backend.Application.DTOsResponse.UserDtos;
using ProConnect_Backend.Domain.Ports;

namespace ProConnect_Backend.Application.UseCases.Users.Query;

public class GetUserByIdQueryHandler
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetUserByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<UserDetailsDto?> Handle(GetUserByIdQuery query)
    {
        var user = await _unitOfWork.UserRepository.GetByIdAsync(query.UserId);
        return user == null ? null : _mapper.Map<UserDetailsDto>(user);
    }
}