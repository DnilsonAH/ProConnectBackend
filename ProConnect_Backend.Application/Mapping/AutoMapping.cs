using AutoMapper;
using ProConnect_Backend.Application.DTOsResponse.AuthDTOs;
using ProConnect_Backend.Application.DTOsResponse.LoginDTOs;
using ProConnect_Backend.Application.DTOsResponse.UserDTOs;
using ProConnect_Backend.Application.DTOsResponse.ProfessionCategoryDTOs;
using ProConnect_Backend.Application.DTOsResponse.ProfessionDTOs;
using ProConnect_Backend.Application.DTOsResponse.SpecializationDTOs;
using ProConnect_Backend.Application.DTOsResponse.ProfileSpecializationDTOs;
using ProConnect_Backend.Domain.DTOsRequest.AuthDtos;
using ProConnect_Backend.Domain.DTOsRequest.ProfessionCategoryDTOs;
using ProConnect_Backend.Domain.DTOsRequest.ProfessionDTOs;
using ProConnect_Backend.Domain.DTOsRequest.SpecializationDTOs;
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
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()) // Se asigna en el handler
            .ForMember(dest => dest.Role, opt => opt.Ignore()) // Se asigna "Client" por defecto en handler
            .ForMember(dest => dest.UserId, opt => opt.Ignore()) // Auto-generado
            .ForMember(dest => dest.RegistrationDate, opt => opt.Ignore()); // Se asigna en handler

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

        // ============================================================
        // MAPEOS PARA PROFESSION CATEGORY
        // ============================================================
        
        // CreateProfessionCategoryRequestDto -> ProfessionCategory
        CreateMap<CreateProfessionCategoryRequestDto, ProfessionCategory>()
            .ForMember(dest => dest.CategoryId, opt => opt.Ignore()) // Auto-generado
            .ForMember(dest => dest.Professions, opt => opt.Ignore()); // No se asigna en creación

        // ProfessionCategory -> ProfessionCategoryResponseDto
        CreateMap<ProfessionCategory, ProfessionCategoryResponseDto>()
            .ForMember(dest => dest.TotalProfessions, opt => opt.Ignore()); // Se calcula en el handler

        // ============================================================
        // MAPEOS PARA PROFESSION
        // ============================================================
        
        // CreateProfessionRequestDto -> Profession
        CreateMap<CreateProfessionRequestDto, Profession>()
            .ForMember(dest => dest.ProfessionId, opt => opt.Ignore()) // Auto-generado
            .ForMember(dest => dest.Category, opt => opt.Ignore()) // Se carga desde DB
            .ForMember(dest => dest.Specializations, opt => opt.Ignore()); // No se asigna en creación

        // Profession -> ProfessionResponseDto
        CreateMap<Profession, ProfessionResponseDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.Ignore()) // Se asigna en el handler
            .ForMember(dest => dest.TotalSpecializations, opt => opt.Ignore()); // Se calcula en el handler

        // ============================================================
        // MAPEOS PARA SPECIALIZATION
        // ============================================================
        
        // CreateSpecializationRequestDto -> Specialization
        CreateMap<CreateSpecializationRequestDto, Specialization>()
            .ForMember(dest => dest.SpecializationId, opt => opt.Ignore()) // Auto-generado
            .ForMember(dest => dest.Profession, opt => opt.Ignore()) // Se carga desde DB
            .ForMember(dest => dest.ProfileSpecializations, opt => opt.Ignore()); // No se asigna en creación

        // Specialization -> SpecializationResponseDto
        CreateMap<Specialization, SpecializationResponseDto>()
            .ForMember(dest => dest.ProfessionName, opt => opt.Ignore()) // Se asigna en el handler
            .ForMember(dest => dest.TotalProfiles, opt => opt.Ignore()); // Se calcula en el handler

        // ============================================================
        // MAPEOS PARA PROFILE SPECIALIZATION
        // ============================================================
        
        // ProfileSpecialization -> ProfileSpecializationResponseDto
        CreateMap<ProfileSpecialization, ProfileSpecializationResponseDto>()
            .ForMember(dest => dest.SpecializationName, opt => opt.Ignore()) // Se asigna en el handler
            .ForMember(dest => dest.ProfessionName, opt => opt.Ignore()); // Se asigna en el handler
    }
}