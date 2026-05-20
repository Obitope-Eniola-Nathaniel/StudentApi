namespace StudentApi.DTOs;

public class StudentResponse
{
    public Guid Id { get; set; }

    public string FullName { get; set; } = default!;

    public string Email { get; set; } = default!;

    public string? PhoneNumber { get; set; }
}
