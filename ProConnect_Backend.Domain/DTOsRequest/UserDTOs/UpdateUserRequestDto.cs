using System.ComponentModel.DataAnnotations;

namespace ProConnect_Backend.Domain.DTOsRequest.UserDTOs;

public class UpdateUserRequestDto
{
    [MaxLength(50)]
    public string? FirstName { get; set; }

    [MaxLength(50)]
    public string? SecondName { get; set; }

    [MaxLength(50)]
    public string? FirstSurname { get; set; }

    [MaxLength(50)]
    public string? SecondSurname { get; set; }

    [Phone]
    [MaxLength(20)]
    public string? PhoneNumber { get; set; }

    [Url]
    [MaxLength(500)]
    public string? PhotoUrl { get; set; }
}
