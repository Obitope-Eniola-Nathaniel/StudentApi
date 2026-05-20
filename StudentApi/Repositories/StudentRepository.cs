using Microsoft.EntityFrameworkCore;
using StudentApi.Data;
using StudentApi.Models;

namespace StudentApi.Repositories;

public class StudentRepository : IStudentRepository
{
    private readonly AppDbContext _context;

    public StudentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Student?> GetByEmailAsync(string email)
    {
        return await _context.Students.FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower() && !x.IsDeleted);
    }

    public async Task<Student> CreateAsync(Student student)
    {
        await _context.Students.AddAsync(student);
        await _context.SaveChangesAsync();

        return student;
    }

}
