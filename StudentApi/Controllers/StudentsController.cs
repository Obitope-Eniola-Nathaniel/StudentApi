using Microsoft.AspNetCore.Mvc;
using StudentApi.Common;
using StudentApi.DTOs;
using StudentApi.Models;
using StudentApi.Repositories;

namespace StudentApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : Controller
{
    private readonly IStudentRepository _studentRepository;

    public StudentsController(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }

    [HttpPost]
    public async Task<IActionResult> CreateStudent([FromBody] CreateStudentRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ApiResponse<object>
            {
                Message = "Validation failed",
                ResponseCode = "99",
                Data = ModelState
            });
        }


        var existingStudent = await _studentRepository.GetByEmailAsync(request.Email);

        if (existingStudent is not null)
        {
            return Conflict(new ApiResponse<object>
            {
                Message = "Email already exists",
                ResponseCode = "99"
            });
        }

        var student = new Student
        {
            Id = Guid.NewGuid(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber
        };

        var createdStudent = await _studentRepository.CreateAsync(student);

        var response = new StudentResponse
        {
            Id = createdStudent.Id,
            FullName = $"{createdStudent.FirstName} {createdStudent.LastName}",
            Email = createdStudent.Email,
            PhoneNumber = createdStudent.PhoneNumber
        };

        return Ok(new ApiResponse<StudentResponse>
        {
            Message = "Student created successfully",
            ResponseCode = "00",
            Data = response
        });
    }

    [HttpGet]
    public async Task<IActionResult> GetAllStudents()
    {
        var students = await _studentRepository.GetAllAsync();

        var response = students.Select(student => new StudentResponse
        {
            Id = student.Id,
            FullName = $"{student.FirstName} {student.LastName}",
            Email = student.Email,
            PhoneNumber = student.PhoneNumber
        }).ToList();

        return Ok(new ApiResponse<List<StudentResponse>>
        {
            Message = "Students retrieved successfully",
            ResponseCode = "00",
            Data = response
        });
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetStudentById(Guid id)
    {
        var student = await _studentRepository.GetByIdAsync(id);

        if (student is null)
        {
            return NotFound(new ApiResponse<object>
            {
                Message = "Student not found",
                ResponseCode = "99"
            });
        }

        var response = new StudentResponse
        {
            Id = student.Id,
            FullName = $"{student.FirstName} {student.LastName}",
            Email = student.Email,
            PhoneNumber = student.PhoneNumber
        };

        return Ok(new ApiResponse<StudentResponse>
        {
            Message = "Student retrieved successfully",
            ResponseCode = "00",
            Data = response
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStudent(Guid id, [FromBody] UpdateStudentRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ApiResponse<object>
            {
                Message = "Validation failed",
                ResponseCode = "99",
                Data = ModelState
            });
        }

        var existingStudent = await _studentRepository.GetByIdAsync(id);

        if (existingStudent is null)
        {
            return NotFound(new ApiResponse<object>
            {
                Message = "Student not found",
                ResponseCode = "99"
            });
        }

        var duplicateEmailStudent = await _studentRepository.GetByEmailExceptCurrentAsync(request.Email, id);

        if (duplicateEmailStudent is not null)
        {
            return Conflict(new ApiResponse<object>
            {
                Message = "Email already exists",
                ResponseCode = "99"
            });
        }

        existingStudent.FirstName = request.FirstName;
        existingStudent.LastName = request.LastName;
        existingStudent.Email = request.Email;
        existingStudent.PhoneNumber = request.PhoneNumber;

        var updatedStudent = await _studentRepository.UpdateAsync(existingStudent);

        var response = new StudentResponse
        {
            Id = updatedStudent.Id,
            FullName =$"{updatedStudent.FirstName} {updatedStudent.LastName}",
            Email = updatedStudent.Email,
            PhoneNumber = updatedStudent.PhoneNumber
        };

        return Ok(new ApiResponse<StudentResponse>
        {
            Message = "Student updated successfully",
            ResponseCode = "00",
            Data = response
        });
    }


    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteStudent(Guid id)
    {

        var deleted = await _studentRepository.SoftDeleteAsync(id);

        if (!deleted)
        {
            return NotFound(new ApiResponse<object>
            {
                Message = "Student not found",
                ResponseCode = "99"
            });
        }

        return Ok(new ApiResponse<object>
        {
            Message = "Student deleted successfully",
            ResponseCode = "00"
        });
    }
}
