using Microsoft.AspNetCore.Mvc;
using StudentInfoSys.Services;

namespace StudentInfoSys.Controllers;

[ApiController]
public class LecturesController(ILectureService lectureService) : ControllerBase
{
    [HttpPost("lecture")]
    public async Task<IActionResult> CreateLecture([FromBody] CreateLectureInput input)
    {
        var lecture = await lectureService.CreateLectureAsync(input);
        return CreatedAtAction(
            nameof(GetLectureById),
            new { lectureName = lecture.Title },
            lecture
        );
    }

    [HttpGet("lectures")]
    public async Task<IActionResult> GetAllLectures()
    {
        var lectures = await lectureService.GetAllLecturesAsync();

        return Ok(lectures);
    }

    [HttpGet("lecture/{lectureId:guid}")]
    public async Task<IActionResult> GetLectureById(Guid lectureId)
    {
        var lecture = await lectureService.GetLectureByIdAsync(lectureId);
        if (lecture is null)
            return NotFound($"Lecture with name '{lectureId}' not found.");

        return Ok(lecture);
    }

    [HttpGet("sorted")]
    public async Task<IActionResult> GetAllLecturesSortedByStudent()
    {
        var lectures = await lectureService.GetAllLecturesSortedByStudentAsync();
        return Ok(lectures);
    }

    [HttpPost("{lectureId:guid}/students")]
    public async Task<IActionResult> AddStudentToLecture(Guid lectureId, Guid studentToAddId)
    {
        var lecture = await lectureService.AddStudentToLectureAsync(lectureId, studentToAddId);
        if (lecture is null)
            return NotFound($"Lecture '{lectureId}' not found.");

        return Ok(lecture);
    }

    [HttpDelete("{lectureId:guid}")]
    public async Task<IActionResult> DeleteLecture(Guid lectureId)
    {
        var lecture = await lectureService.DeleteLectureAsync(lectureId);
        if (lecture is null)
            return NotFound($"Lecture with ID '{lectureId}' not found.");

        return Ok(lecture);
    }
}
