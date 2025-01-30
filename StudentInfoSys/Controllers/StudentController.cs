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
            nameof(GetStudentByName),
            new { studentName = student.Name },
            student.ToDto()
        );
    }

    [HttpGet("{studentName}")]
    public async Task<IActionResult> GetStudentByName(string studentName)
    {
        var student = await studentService.GetStudentByName(studentName);
        if (student == null)
            return NotFound($"Student with name '{studentName}' not found.");

        return Ok(student.ToDto());
    }

    [HttpPut("{studentName}/department")]
    public async Task<IActionResult> ChangeStudentDepartment(
        string studentName,
        [FromBody] Department newDepartment
    )
    {
        var student = await studentService.ChangeStudentDepartment(studentName, newDepartment);
        return Ok(student.ToDto());
    }

    [HttpPost("{studentName}/lectures")]
    public async Task<IActionResult> AddLectureToStudent(
        string studentName,
        [FromBody] Lecture lecture
    )
    {
        var student = await studentService.AddLectureToStudent(studentName, lecture);
        return Ok(student.ToDto());
    }

    [HttpDelete("{studentName}/lectures")]
    public async Task<IActionResult> RemoveLectureFromStudent(
        string studentName,
        [FromBody] Lecture lecture
    )
    {
        var student = await studentService.RemoveLectureFromStudent(studentName, lecture);
        return Ok(student.ToDto());
    }

    [HttpDelete("{studentId:guid}")]
    public async Task<IActionResult> DeleteStudent(Guid studentId)
    {
        var student = await studentService.DeleteStudent(studentId);
        if (student == null)
        {
            return NotFound($"Student with ID '{studentId}' not found.");
        }
        return Ok(student.ToDto());
    }
}
