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
}
