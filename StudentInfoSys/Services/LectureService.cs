using Microsoft.EntityFrameworkCore;
using StudentInfoSys.Models;

namespace StudentInfoSys.Services;

public interface ILectureService
{
    Task<Lecture> CreateLecture(CreateLectureInput input);
    Task<Lecture> GetLectureByName(string lectureName);
    Task<List<Lecture>> GetAllLecturesSortedByStudent();
    Task<Lecture> AddStudentToLecture(string lectureName, Student studentToAdd);
    Task<Lecture> DeleteLecture(Guid lectureId);
}

public record CreateLectureInput(
    string Title,
    List<Student>? Students,
    List<Department>? Departments
);

public class LectureService(UniversityContext context, ILogger<LectureService> logger)
    : ILectureService
{
    public async Task<Lecture> CreateLecture(CreateLectureInput input)
    {
        var lecture = new Lecture
        {
            Title = input.Title,
            Students = input.Students,
            Departments = input.Departments,
        };

        context.Lectures.Add(lecture);
        await context.SaveChangesAsync();
        return lecture;
    }

    public async Task<Lecture> GetLectureByName(string lectureName)
    {
        var result = await context
            .Lectures.AsNoTracking()
            .FirstOrDefaultAsync(l => l.Title.Contains(lectureName));

        if (result is null)
            throw new NullReferenceException("Lecture not found!");

        return result;
    }

    public async Task<List<Lecture>> GetAllLecturesSortedByStudent()
    {
        var result = await context
            .Lectures.AsNoTracking()
            .Include(x => x.Students)
            .OrderBy(x => x.Students)
            .ToListAsync();
        return result;
    }

    public async Task<Lecture> AddStudentToLecture(string lectureName, Student studentToAdd)
    {
        var lecture = await GetLectureByName(lectureName);

        if (lecture is null)
        {
            throw new NullReferenceException("Lecture not found!");
        }

        lecture.Students?.Add(studentToAdd);
        studentToAdd.Lectures.Add(lecture);

        await context.SaveChangesAsync();

        return lecture;
    }

    public async Task<Lecture> DeleteLecture(Guid lectureId)
    {
        var lecture = await context.Lectures.FirstOrDefaultAsync(l => l.Id == lectureId);

        if (lecture is null)
        {
            throw new NullReferenceException("Lecture not found!");
        }

        context.Lectures.Remove(lecture);

        await context.SaveChangesAsync();

        return lecture;
    }
}
