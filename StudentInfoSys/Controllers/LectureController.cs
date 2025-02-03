using Microsoft.AspNetCore.Mvc;
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

    [HttpGet]
    public async Task<IActionResult> GetAllLectures()
    {
        var lectures = await lectureService.GetAllLectures();

        return Ok(lectures);
    }

    [HttpGet("{lectureId}")]
    public async Task<IActionResult> GetLectureByName(Guid lectureId)
    {
        var lecture = await lectureService.GetLectureById(lectureId);
        if (lecture == null)
            return NotFound($"Lecture with name '{lectureId}' not found.");

        return Ok(lecture);
    }

    [HttpGet("sorted")]
    public async Task<IActionResult> GetAllLecturesSortedByStudent()
    {
        var lectures = await lectureService.GetAllLecturesSortedByStudent();
        return Ok(lectures);
    }

    [HttpPost("{lectureId}/students")]
    public async Task<IActionResult> AddStudentToLecture(Guid lectureId, Guid studentToAddId)
    {
        var lecture = await lectureService.AddStudentToLecture(lectureId, studentToAddId);
        if (lecture == null)
            return NotFound($"Lecture '{lectureId}' not found.");

        return Ok(lecture);
    }

    [HttpDelete("{lectureId:guid}")]
    public async Task<IActionResult> DeleteLecture(Guid lectureId)
    {
        var lecture = await lectureService.DeleteLecture(lectureId);
        if (lecture == null)
            return NotFound($"Lecture with ID '{lectureId}' not found.");

        return Ok(lecture);
    }
}
