using AutoMapper;
using ProConnect_Backend.Core.DTOs;
using ProConnect_Backend.Core.Entities;
using ProConnect_Backend.Core.Repositories.Interfaces;
using ProConnect_Backend.Core.Services.interfaces;

namespace ProConnect_Backend.Core.Services;

public class UserService : GenericService<User, UserDto>, IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    public UserService(IUserRepository repository, IMapper mapper, IUnitOfWork unitOfWork) : base(repository, mapper)
    {
        _unitOfWork = unitOfWork;
    }
}