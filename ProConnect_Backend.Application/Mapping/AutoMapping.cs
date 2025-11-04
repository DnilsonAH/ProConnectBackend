using AutoMapper;
using ProConnect_Backend.Domain.Entities;

namespace ProConnect_Backend.Application.Mapping;

public class AutoMapping : Profile
{
    //Configuraciones de mapeo entre DTOs y Entidades
    /*En caso de que se agreguen nuevas entidades o dtos, agregar las configuraciones aqui
    .ForMember es para mapear propiedades que no coinciden en nombre o que necesitan conversiones especiales
    https://stackoverflow.com/questions/6985000/how-to-use-automapper-formember
         - Si tu dto tiene una variable que no esta en ela entidad, usar ForMember para poder mapearla
         - Si tienes nombres diferentes entre dto y entidad, usar ForMember para mapearla
     */
    public AutoMapping()
    {
        //Dto -> Entity (Input para POST/PUT)
        //ejemplo: CreateMap<UserDto, User>();
        
        //Entity -> Dto (Output para GET)
        //ejemplo: CreateMap<User, UserDto>();

         }
}