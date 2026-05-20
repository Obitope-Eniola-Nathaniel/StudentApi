using System.ComponentModel.DataAnnotations;

namespace StudentApi.DTOs;

public class CreateStudentRequest
{
    [Required]
    public string FirstName { get; set; } = default!;

    [Required]
    public string LastName { get; set; } = default!;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = default!;


    public string? PhoneNumber { get; set; }
}
