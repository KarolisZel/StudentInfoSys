using Microsoft.EntityFrameworkCore;
using StudentInfoSys.Contracts;
using StudentInfoSys.Models;

namespace StudentInfoSys.Services;

public interface ILectureService
{
    Task<LectureDto> CreateLectureAsync(CreateLectureInput input);
    Task<List<LectureDto>> GetAllLecturesAsync();
    Task<LectureDto?> GetLectureByIdAsync(Guid lectureId);
    Task<List<LectureDto>?> GetAllLecturesSortedByStudentAsync();
    Task<LectureDto?> AddStudentToLectureAsync(Guid lectureId, Guid studentToAddId);
    Task<LectureDto?> DeleteLectureAsync(Guid lectureId);
}

public record CreateLectureInput(
    string Title,
    List<Student>? Students,
    List<Department>? Departments
);

public class LectureService(UniversityContext context, ILogger<LectureService> logger)
    : ILectureService
{
    public async Task<LectureDto> CreateLectureAsync(CreateLectureInput input)
    {
        var lecture = new Lecture { Title = input.Title, };

        if (input.Students is not null)
        {
            var studentIds = input.Students.Select(x => x.Id).ToArray();

            var existingStudents = input.Students.Where(x => studentIds.Contains(x.Id)).ToArray();

            if (existingStudents.Length > 0)
                lecture.Students?.AddRange(existingStudents);
        }

        if (input.Departments is not null)
        {
            var studentIds = input.Departments.Select(x => x.Id).ToArray();

            var existingDepartments = input
                .Departments.Where(x => studentIds.Contains(x.Id))
                .ToArray();

            if (existingDepartments.Length > 0)
                lecture.Departments?.AddRange(existingDepartments);
        }

        context.Lectures.Add(lecture);
        await context.SaveChangesAsync();
        return lecture.ToDto();
    }

    public async Task<List<LectureDto>> GetAllLecturesAsync()
    {
        var result = await context
            .Lectures.AsNoTracking()
            .Include(l => l.Students)
            .Include(l => l.Departments)
            .Select(x => x.ToDto())
            .ToListAsync();

        return result;
    }

    public async Task<LectureDto?> GetLectureByIdAsync(Guid lectureId)
    {
        var result = await context
            .Lectures.AsNoTracking()
            .Include(l => l.Students)
            .Include(l => l.Departments)
            .FirstOrDefaultAsync(l => l.Id == lectureId);

        return result.ToDto();
    }

    public async Task<List<LectureDto>?> GetAllLecturesSortedByStudentAsync()
    {
        var result = await context
            .Lectures.AsNoTracking()
            .Include(x => x.Students)
            .OrderBy(x => x.Students.Count)
            .Select(x => x.ToDto())
            .ToListAsync();
        return result;
    }

    public async Task<LectureDto?> AddStudentToLectureAsync(Guid lectureId, Guid studentToAddId)
    {
        var lecture = await context
            .Lectures.Include(l => l.Students)
            .FirstOrDefaultAsync(l => l.Id == lectureId);

        if (lecture is null)
            return null;

        if (lecture.Students.Any(s => s.Id == studentToAddId))
            return lecture.ToDto();

        var studentToAdd = new Student { Id = studentToAddId };

        lecture.Students?.Add(studentToAdd);

        await context.SaveChangesAsync();

        return lecture.ToDto();
    }

    public async Task<LectureDto?> DeleteLectureAsync(Guid lectureId)
    {
        var lecture = await context
            .Lectures.Include(l => l.Departments)
            .Include(l => l.Students)
            .FirstOrDefaultAsync(l => l.Id == lectureId);

        if (lecture is null)
            return null;

        if (lecture.Students is not null)
        {
            foreach (var student in lecture.Students)
            {
                student.Lectures?.Remove(lecture);
            }
        }

        if (lecture.Departments is not null)
        {
            foreach (var department in lecture.Departments)
            {
                department.Lectures?.Remove(lecture);
            }
        }

        context.Lectures.Remove(lecture);

        await context.SaveChangesAsync();

        return lecture.ToDto();
    }
}
