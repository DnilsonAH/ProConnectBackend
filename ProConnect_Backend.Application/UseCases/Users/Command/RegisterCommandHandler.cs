using AutoMapper;
using Microsoft.AspNetCore.Identity;
using ProConnect_Backend.Application.DTOsResponse.UserDtos;
using ProConnect_Backend.Domain.DTOsRequest.AuthDtos;
using ProConnect_Backend.Domain.Entities;
using ProConnect_Backend.Domain.Ports;

namespace ProConnect_Backend.Application.UseCases.Users.Command;

public class RegisterCommandHandler
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly PasswordHasher<User> _passwordHasher;

    public RegisterCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _passwordHasher = new PasswordHasher<User>();
    }

    public async Task<RegisterResponseDto> Handle(RegisterCommand command)
    {
        var dto = command.Dto;

        // Mapear DTO a entidad (automapper configurado)
        var user = _mapper.Map<User>(dto);

        // Asignar valores no mapeados
        user.Role = string.IsNullOrEmpty(user.Role) ? "Client" : user.Role;

        // Hashear la contraseña
        user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);

        // Persistir usando UoW
        await _unitOfWork.UserRepository.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();

        return new RegisterResponseDto
        {
            Id = user.UserId,
            Name = user.Name,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Role = user.Role
        };
    }
}
