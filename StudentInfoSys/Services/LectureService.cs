using Microsoft.EntityFrameworkCore;
using StudentInfoSys.Models;

namespace StudentInfoSys.Services;

public interface ILectureService
{
    Task<Lecture> CreateLecture(CreateLectureInput input);
    Task<List<Lecture>> GetAllLectures();
    Task<Lecture?> GetLectureById(Guid lectureId);
    Task<List<Lecture>?> GetAllLecturesSortedByStudent();
    Task<Lecture?> AddStudentToLecture(Guid lectureId, Guid studentToAddId);
    Task<Lecture?> DeleteLecture(Guid lectureId);
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
        var lecture = new Lecture { Title = input.Title, };

        if (input.Students is not null)
        {
            foreach (var student in input.Students)
            {
                var existingStudent = await context.Students.FindAsync(student.Id);
                if (existingStudent is not null)
                    lecture.Students?.Add(existingStudent);
            }
        }

        if (input.Departments is not null)
        {
            lecture.Departments = new List<Department>();
            foreach (var department in input.Departments)
            {
                var existingDepartment = await context.Departments.FindAsync(department.Id);
                if (existingDepartment is not null)
                    lecture.Departments.Add(existingDepartment);
            }
        }

        context.Lectures.Add(lecture);
        await context.SaveChangesAsync();
        return lecture;
    }

    public async Task<List<Lecture>> GetAllLectures()
    {
        var result = await context
            .Lectures.AsNoTracking()
            .Include(l => l.Students)
            .Include(l => l.Departments)
            .ToListAsync();

        return result;
    }

    public async Task<Lecture?> GetLectureById(Guid lectureId)
    {
        var result = await context
            .Lectures.AsNoTracking()
            .Where(l => l.Id == lectureId)
            .Include(l => l.Students)
            .Include(l => l.Departments)
            .FirstAsync();

        return result;
    }

    public async Task<List<Lecture>?> GetAllLecturesSortedByStudent()
    {
        var result = await context
            .Lectures.AsNoTracking()
            .Include(x => x.Students)
            .OrderBy(x => x.Students.Count)
            .ToListAsync();
        return result;
    }

    public async Task<Lecture?> AddStudentToLecture(Guid lectureId, Guid studentToAddId)
    {
        var lecture = await context
            .Lectures.AsNoTracking()
            .Where(l => l.Id == lectureId)
            .Include(l => l.Students)
            .FirstAsync();

        if (lecture is null)
            return null;

        if (lecture.Students.Any(s => s.Id == studentToAddId))
            return lecture;

        var studentToAdd = new Student { Id = studentToAddId };

        context.Students.Attach(studentToAdd);
        lecture.Students?.Add(studentToAdd);

        await context.SaveChangesAsync();

        return lecture;
    }

    public async Task<Lecture?> DeleteLecture(Guid lectureId)
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

        return lecture;
    }
}
