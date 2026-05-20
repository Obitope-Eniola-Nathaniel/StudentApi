using StudentApi.Models;

namespace StudentApi.Repositories;

public interface IStudentRepository
{
    Task<Student?> GetByEmailAsync(string email);

    Task<Student> CreateAsync(Student student);
}
