namespace StudentApi.Models;

public class Student
{
    public Guid Id { get; set; }

    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;

    public string Email { get; set; } = default!;

    public string? PhoneNumber { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
}
