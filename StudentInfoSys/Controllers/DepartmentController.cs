using Microsoft.AspNetCore.Mvc;
using StudentInfoSys.Models;
using StudentInfoSys.Services;

namespace StudentInfoSys.Controllers;

[ApiController]
[Route("departments")]
public class DepartmentController(IDepartmentService departmentService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateDepartment([FromBody] CreateDepartmentInput input)
    {
        var department = await departmentService.CreateDepartment(input);
        return CreatedAtAction(
            nameof(GetDepartmentById),
            new { departmentId = department.Id },
            department.ToDto()
        );
    }

    [HttpGet]
    public async Task<IActionResult> GetAllDepartments()
    {
        var departments = await departmentService.GetAllDepartments();

        return Ok(departments);
    }

    [HttpGet("{departmentId}")]
    public async Task<IActionResult> GetDepartmentById(Guid departmentId)
    {
        var department = await departmentService.GetDepartmentById(departmentId);
        if (department == null)
            return NotFound($"Department with name '{departmentId}' not found");

        return Ok(department);
    }

    [HttpGet("{departmentId}/students")]
    public async Task<IActionResult> GetAllStudentsInDepartment(Guid departmentId)
    {
        var students = await departmentService.GetAllStudentsInDepartment(departmentId);
        if (students == null)
            return NotFound($"No students found in department '{departmentId}'.");

        return Ok(students);
    }

    [HttpGet("{departmentId}/lectures")]
    public async Task<IActionResult> GetAllLecturesInDepartment(Guid departmentId)
    {
        var lectures = await departmentService.GetAllLecturesInDepartment(departmentId);
        if (lectures == null)
            return NotFound($"No lectures found in department '{departmentId}'.");

        return Ok(lectures);
    }

    [HttpPost("{departmentId}/students")]
    public async Task<IActionResult> AddStudentToDepartment(Guid departmentId, Guid studentToAddId)
    {
        var department = await departmentService.AddStudentToDepartment(
            departmentId,
            studentToAddId
        );
        if (department == null)
            return NotFound($"Department '{departmentId}' not found.");

        return Ok(department);
    }

    [HttpPost("{departmentId}/lectures")]
    public async Task<IActionResult> AddLectureToDepartment(Guid departmentId, Guid lectureToAddId)
    {
        var department = await departmentService.AddLectureToDepartment(
            departmentId,
            lectureToAddId
        );
        if (department == null)
            return NotFound($"Department '{departmentId}' not found.");

        return Ok(department);
    }

    [HttpDelete("{departmentId:guid}")]
    public async Task<IActionResult> DeleteDepartment(Guid departmentId)
    {
        var department = await departmentService.DeleteDepartment(departmentId);
        if (department == null)
            return NotFound($"Department with ID '{departmentId}' not found.");

        return Ok(department.ToDto());
    }
}
