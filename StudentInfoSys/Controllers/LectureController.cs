using Microsoft.AspNetCore.Mvc;
using StudentInfoSys.Models;
using StudentInfoSys.Services;

namespace StudentInfoSys.Controllers;

[ApiController]
[Route("lectures")]
public class LectureController(ILectureService lectureService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateLecture([FromBody] CreateLectureInput input)
    {
        var lecture = await lectureService.CreateLecture(input);
        return CreatedAtAction(
            nameof(GetLectureByName),
            new { lectureName = lecture.Title },
            lecture.ToDto()
        );
    }

    [HttpGet("{lectureName}")]
    public async Task<IActionResult> GetLectureByName(string lectureName)
    {
        var lecture = await lectureService.GetLectureByName(lectureName);
        if (lecture == null)
            return NotFound($"Lecture with name '{lectureName}' not found.");

        return Ok(lecture.ToDto());
    }

    [HttpGet("sorted")]
    public async Task<IActionResult> GetAllLecturesSortedByStudent()
    {
        var lectures = await lectureService.GetAllLecturesSortedByStudent();
        return Ok(lectures);
    }

    [HttpPost("{lectureName}/students")]
    public async Task<IActionResult> AddStudentToLecture(
        string lectureName,
        [FromBody] Student studentToAdd
    )
    {
        var lecture = await lectureService.AddStudentToLecture(lectureName, studentToAdd);
        if (lecture == null)
            return NotFound($"Lecture '{lectureName}' not found.");

        return Ok(lecture.ToDto());
    }

    [HttpDelete("{lectureId:guid}")]
    public async Task<IActionResult> DeleteLecture(Guid lectureId)
    {
        var lecture = await lectureService.DeleteLecture(lectureId);
        if (lecture == null)
            return NotFound($"Lecture with ID '{lectureId}' not found.");

        return Ok(lecture.ToDto()); // 204 No Content
    }
}
