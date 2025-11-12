using AutoMapper;
// TODO: Descomentar después del scaffolding cuando las entidades estén generadas
// using ProConnect_Backend.Application.DTOsResponse.SpecialityDtos;
// using ProConnect_Backend.Application.DTOsResponse.UserDtos;
// using ProConnect_Backend.Domain.Entities;

namespace ProConnect_Backend.Application.Mapping;

public class AutoMapping : Profile
{
    // TODO: Descomentar después del scaffolding cuando las entidades estén generadas
    //Configuraciones de mapeo entre DTOs y Entidades
    /*En caso de que se agreguen nuevas entidades o dtos, agregar las configuraciones aqui
    .ForMember es para mapear propiedades que no coinciden en nombre o que necesitan conversiones especiales
    https://stackoverflow.com/questions/6985000/how-to-use-automapper-formember
         - Si tu dto tiene una variable que no esta en ela entidad, usar ForMember para poder mapearla
         - Si tienes nombres diferentes entre dto y entidad, usar ForMember para mapearla
     */
    public AutoMapping()
    {
        // //Dto -> Entity (Input para POST/PUT)
        // //ejemplo: CreateMap<UserDto, User>();
        // CreateMap<SpecialtyDto, Specialty>();
        // // Mapear DTO de registro a entidad User
        // CreateMap<ProConnect_Backend.Domain.DTOsRequest.AuthDtos.RegisterRequestDto, ProConnect_Backend.Domain.Entities.User>()
        //     .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
        //     .ForMember(dest => dest.Role, opt => opt.Ignore())
        //     .ForMember(dest => dest.UserId, opt => opt.Ignore())
        //     .ForMember(dest => dest.RegistrationDate, opt => opt.Ignore());
        // CreateMap<SpecialtyGetDto, Specialty>();
        //
        // //Entity -> Dto (Output para GET)
        // //ejemplo: CreateMap<User, UserDto>();
        // CreateMap<Specialty, SpecialtyDto>();
        // CreateMap<Specialty, SpecialtyGetDto>();
        // CreateMap<User, UserDetailsDto>();

    }
}