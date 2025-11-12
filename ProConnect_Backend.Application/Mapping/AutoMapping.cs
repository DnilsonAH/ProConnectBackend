using AutoMapper;
using ProConnect_Backend.Application.DTOsResponse.AuthDTOs;
using ProConnect_Backend.Application.DTOsResponse.LoginDTOs;
using ProConnect_Backend.Application.DTOsResponse.UserDTOs;
using ProConnect_Backend.Domain.DTOsRequest.AuthDtos;
using ProConnect_Backend.Domain.Entities;

namespace ProConnect_Backend.Application.Mapping;

public class AutoMapping : Profile
{
    public AutoMapping()
    {
        // ============================================================
        // MAPEOS PARA AUTH (Login, Register)
        // ============================================================
        
        // RegisterRequestDto -> User (para crear usuario)
        CreateMap<RegisterRequestDto, User>()
            .ForMember(dest => dest.FirstSurname, opt => opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Phone))
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
            .ForMember(dest => dest.Role, opt => opt.Ignore())
            .ForMember(dest => dest.UserId, opt => opt.Ignore())
            .ForMember(dest => dest.RegistrationDate, opt => opt.Ignore())
            .ForMember(dest => dest.SecondName, opt => opt.Ignore())
            .ForMember(dest => dest.SecondSurname, opt => opt.Ignore())
            .ForMember(dest => dest.PhotoUrl, opt => opt.Ignore());

        // User -> LoginResponseDto
        CreateMap<User, LoginResponseDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.Token, opt => opt.Ignore());

        // User -> RegisterResponseDto
        CreateMap<User, RegisterResponseDto>()
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.FirstSurname))
            .ForMember(dest => dest.Token, opt => opt.Ignore());

        // ============================================================
        // MAPEOS PARA USER INFO (GetUserInfo)
        // ============================================================
        
        // User -> GetUserInfoResponseDto
        CreateMap<User, GetUserInfoResponseDto>()
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.FirstSurname))
            .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.Country, opt => opt.Ignore()); // No existe en User
    }
}