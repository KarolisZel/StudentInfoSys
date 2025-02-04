using Microsoft.AspNetCore.Mvc;
using StudentInfoSys.Models;
using StudentInfoSys.Services;

namespace StudentInfoSys.Controllers;

[ApiController]
public class DepartmentsController(IDepartmentService departmentService) : ControllerBase
{
    [HttpPost("department")]
    public async Task<IActionResult> CreateDepartment([FromBody] CreateDepartmentInput input)
    {
        var department = await departmentService.CreateDepartmentAsync(input);
        return CreatedAtAction(
            nameof(GetDepartmentById),
            new { departmentId = department.Id },
            department
        );
    }

    [HttpGet("departments")]
    public async Task<IActionResult> GetAllDepartments()
    {
        var departments = await departmentService.GetAllDepartmentsAsync();

        return Ok(departments);
    }

    [HttpGet("department/{departmentId:guid}")]
    public async Task<IActionResult> GetDepartmentById(Guid departmentId)
    {
        var department = await departmentService.GetDepartmentByIdAsync(departmentId);
        if (department is null)
            return NotFound($"Department with name '{departmentId}' not found");

        return Ok(department);
    }

    [HttpGet("{departmentId:guid}/students")]
    public async Task<IActionResult> GetAllStudentsInDepartment(Guid departmentId)
    {
        var students = await departmentService.GetAllStudentsInDepartmentAsync(departmentId);
        if (students is null)
            return NotFound($"No students found in department '{departmentId}'.");

        return Ok(students);
    }

    [HttpGet("{departmentId:guid}/lectures")]
    public async Task<IActionResult> GetAllLecturesInDepartment(Guid departmentId)
    {
        var lectures = await departmentService.GetAllLecturesInDepartmentAsync(departmentId);
        if (lectures is null)
            return NotFound($"No lectures found in department '{departmentId}'.");

        return Ok(lectures);
    }

    [HttpPost("{departmentId:guid}/students")]
    public async Task<IActionResult> AddStudentToDepartment(Guid departmentId, Guid studentToAddId)
    {
        var department = await departmentService.AddStudentToDepartmentAsync(
            departmentId,
            studentToAddId
        );
        if (department is null)
            return NotFound($"Department '{departmentId}' not found.");

        return Ok(department);
    }

    [HttpPost("{departmentId:guid}/lectures")]
    public async Task<IActionResult> AddLectureToDepartment(Guid departmentId, Guid lectureToAddId)
    {
        var department = await departmentService.AddLectureToDepartmentAsync(
            departmentId,
            lectureToAddId
        );
        if (department is null)
            return NotFound($"Department '{departmentId}' not found.");

        return Ok(department);
    }

    [HttpDelete("{departmentId:guid}")]
    public async Task<IActionResult> DeleteDepartment(Guid departmentId)
    {
        var department = await departmentService.DeleteDepartmentAsync(departmentId);
        if (department is null)
            return NotFound($"Department with ID '{departmentId}' not found.");

        return Ok(department);
    }
}
