using StudentApi.Models;

namespace StudentApi.Repositories;

public interface IStudentRepository
{
    Task<Student?> GetByEmailAsync(string email);

    Task<Student> CreateAsync(Student student);

    Task<List<Student>> GetAllAsync();

    Task<Student?> GetByIdAsync(Guid id);

    Task<Student?> GetByEmailExceptCurrentAsync(string email, Guid currentStudentId);

    Task<Student> UpdateAsync(Student student);

    Task<bool> SoftDeleteAsync(Guid id);
}
