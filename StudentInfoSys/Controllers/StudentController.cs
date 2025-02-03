using Microsoft.AspNetCore.Mvc;
using StudentInfoSys.Models;
using StudentInfoSys.Services;

namespace StudentInfoSys.Controllers;

[ApiController]
[Route("students")]
public class StudentController(IStudentService studentService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateStudent([FromBody] CreateStudentInput input)
    {
        var student = await studentService.CreateStudent(input);
        return CreatedAtAction(
            nameof(GetStudentById),
            new { studentId = student.Id },
            student.ToDto()
        );
    }

    [HttpGet]
    public async Task<IActionResult> GetAllStudents()
    {
        var students = await studentService.GetAllStudents();

        return Ok(students);
    }

    [HttpGet("{studentId}")]
    public async Task<IActionResult> GetStudentById(Guid studentId)
    {
        var student = await studentService.GetStudentById(studentId);
        if (student == null)
            return NotFound($"Student with name '{studentId}' not found.");

        return Ok(student);
    }

    [HttpPut("{studentId}/department")]
    public async Task<IActionResult> ChangeStudentDepartment(Guid studentId, Guid newDepartmentId)
    {
        var student = await studentService.ChangeStudentDepartment(studentId, newDepartmentId);
        return Ok(student);
    }

    [HttpPost("{studentId}/lectures")]
    public async Task<IActionResult> AddLectureToStudent(Guid studentId, Guid lectureId)
    {
        var student = await studentService.AddLectureToStudent(studentId, lectureId);
        return Ok(student);
    }

    [HttpDelete("{studentId}/lectures")]
    public async Task<IActionResult> RemoveLectureFromStudent(Guid studentId, Guid lectureId)
    {
        var student = await studentService.RemoveLectureFromStudent(studentId, lectureId);
        return Ok(student);
    }

    [HttpDelete("{studentId:guid}")]
    public async Task<IActionResult> DeleteStudent(Guid studentId)
    {
        var student = await studentService.DeleteStudent(studentId);
        if (student == null)
        {
            return NotFound($"Student with ID '{studentId}' not found.");
        }
        return Ok(student);
    }
}
