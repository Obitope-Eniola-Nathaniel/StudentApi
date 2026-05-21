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

    public async Task<List<Student>> GetAllAsync()
    {
        return await _context.Students.Where(x => !x.IsDeleted).OrderByDescending(x => x.DateCreated).ToListAsync();
    }

    public async Task<Student?> GetByIdAsync(Guid id)
    {
        return await _context.Students.FirstOrDefaultAsync(x =>x.Id == id && !x.IsDeleted);
    }

    public async Task<Student?> GetByEmailExceptCurrentAsync(string email, Guid currentStudentId)
    {
        return await _context.Students.FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower() && x.Id != currentStudentId && !x.IsDeleted);
    }

    public async Task<Student> UpdateAsync(Student student)
    {
        _context.Students.Update(student);
        await _context.SaveChangesAsync();

        return student;
    }

    public async Task<bool> SoftDeleteAsync(Guid id)
    {
        var student = await _context.Students.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

        if (student is null)
        {
            return false;
        }

        student.IsDeleted = true;

        _context.Students.Update(student);

        await _context.SaveChangesAsync();

        return true;
    }
}
