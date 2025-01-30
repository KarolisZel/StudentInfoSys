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
            nameof(GetDepartmentByName),
            new { departmentName = department.Name },
            department.ToDto()
        );
    }

    [HttpGet("{departmentName}")]
    public async Task<IActionResult> GetDepartmentByName(string departmentName)
    {
        var department = await departmentService.GetDepartmentByName(departmentName);
        if (department == null)
            return NotFound($"Department with name '{departmentName}' not found");

        return Ok(department.ToDto());
    }

    [HttpGet("{departmentName}/students")]
    public async Task<IActionResult> GetAllStudentsInDepartment(string departmentName)
    {
        var students = await departmentService.GetAllStudentsInDepartment(departmentName);
        if (students == null)
            return NotFound($"No students found in department '{departmentName}'.");

        return Ok(students);
    }

    [HttpGet("{departmentName}/lectures")]
    public async Task<IActionResult> GetAllLecturesInDepartment(string departmentName)
    {
        var lectures = await departmentService.GetAllLecturesInDepartment(departmentName);
        if (lectures == null)
            return NotFound($"No lectures found in department '{departmentName}'.");

        return Ok(lectures);
    }

    [HttpPost("{departmentName}/students")]
    public async Task<IActionResult> AddStudentToDepartment(
        string departmentName,
        [FromBody] Student studentToAdd
    )
    {
        var department = await departmentService.AddStudentToDepartment(
            departmentName,
            studentToAdd
        );
        if (department == null)
            return NotFound($"Department '{departmentName}' not found.");

        return Ok(department.ToDto());
    }

    [HttpPost("{departmentName}/lectures")]
    public async Task<IActionResult> AddLectureToDepartment(
        string departmentName,
        [FromBody] Lecture lectureToAdd
    )
    {
        var department = await departmentService.AddLectureToDepartment(
            departmentName,
            lectureToAdd
        );
        if (department == null)
            return NotFound($"Department '{departmentName}' not found.");

        return Ok(department.ToDto());
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
