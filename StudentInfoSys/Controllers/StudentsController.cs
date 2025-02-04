using Microsoft.AspNetCore.Mvc;
using StudentInfoSys.Models;
using StudentInfoSys.Services;

namespace StudentInfoSys.Controllers;

[ApiController]
public class StudentsController(IStudentService studentService) : ControllerBase
{
    [HttpPost("student")]
    public async Task<IActionResult> CreateStudent([FromBody] CreateStudentInput input)
    {
        var student = await studentService.CreateStudentAsync(input);
        return CreatedAtAction(nameof(GetStudentById), new { studentId = student.Id }, student);
    }

    [HttpGet("students")]
    public async Task<IActionResult> GetAllStudents()
    {
        var students = await studentService.GetAllStudents();

        return Ok(students);
    }

    [HttpGet("student/{studentId:guid}")]
    public async Task<IActionResult> GetStudentById(Guid studentId)
    {
        var student = await studentService.GetStudentByIdAsync(studentId);
        if (student is null)
            return NotFound($"Student with name '{studentId}' not found.");

        return Ok(student);
    }

    [HttpPut("{studentId:guid}/department")]
    public async Task<IActionResult> ChangeStudentDepartment(Guid studentId, Guid newDepartmentId)
    {
        var student = await studentService.ChangeStudentDepartmentAsync(studentId, newDepartmentId);
        return Ok(student);
    }

    [HttpPost("{studentId:guid}/lectures")]
    public async Task<IActionResult> AddLectureToStudent(Guid studentId, Guid lectureId)
    {
        var student = await studentService.AddLectureToStudentAsync(studentId, lectureId);
        return Ok(student);
    }

    [HttpDelete("{studentId:guid}/lectures")]
    public async Task<IActionResult> RemoveLectureFromStudent(Guid studentId, Guid lectureId)
    {
        var student = await studentService.RemoveLectureFromStudentAsync(studentId, lectureId);
        return Ok(student);
    }

    [HttpDelete("{studentId:guid}")]
    public async Task<IActionResult> DeleteStudent(Guid studentId)
    {
        var student = await studentService.DeleteStudentAsync(studentId);
        if (student is null)
        {
            return NotFound($"Student with ID '{studentId}' not found.");
        }
        return Ok(student);
    }
}
