using Microsoft.EntityFrameworkCore;
using StudentInfoSys.Models;

namespace StudentInfoSys.Services;

public interface IStudentService
{
    Task<Student> CreateStudent(CreateStudentInput input);
    Task<List<Student>> GetAllStudents();
    Task<Student?> GetStudentById(Guid studentId);

    Task<Student?> ChangeStudentDepartment(Guid studentId, Guid newDepartmentId);

    Task<Student?> AddLectureToStudent(Guid studentId, Guid lectureToAddId);

    Task<Student?> RemoveLectureFromStudent(Guid studentId, Guid lectureToRemoveId);

    Task<Student?> DeleteStudent(Guid studentId);
}

public record CreateStudentInput(string Name, List<Lecture>? Lectures);

public class StudentService(UniversityContext context, ILogger<StudentService> logger)
    : IStudentService
{
    public async Task<Student> CreateStudent(CreateStudentInput input)
    {
        var student = new Student { Name = input.Name, };

        if (input.Lectures is not null)
        {
            foreach (var lecture in input.Lectures)
            {
                var existingLecture = await context.Lectures.FindAsync(lecture.Id);
                if (existingLecture is not null)
                    student.Lectures?.Add(existingLecture);
            }
        }

        context.Students.Add(student);
        await context.SaveChangesAsync();
        return student;
    }

    public async Task<List<Student>> GetAllStudents()
    {
        var result = await context.Students.AsNoTracking().Include(s => s.Lectures).ToListAsync();

        return result;
    }

    public async Task<Student?> GetStudentById(Guid studentId)
    {
        var result = await context
            .Students.AsNoTracking()
            .Where(s => s.Id == studentId)
            .Include(s => s.Lectures)
            .FirstAsync();

        return result;
    }

    public async Task<Student?> ChangeStudentDepartment(Guid studentId, Guid newDepartmentId)
    {
        var student = await context
            .Students.AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == studentId);

        if (student is null)
            return null;

        student.DepartmentId = newDepartmentId;
        student.Lectures?.Clear();

        await context.SaveChangesAsync();

        return student;
    }

    public async Task<Student?> AddLectureToStudent(Guid studentId, Guid lectureToAddId)
    {
        var student = await context
            .Students.AsNoTracking()
            .Where(s => s.Id == studentId)
            .Include(s => s.Lectures)
            .FirstAsync();

        if (student.Lectures.Any(l => l.Id == lectureToAddId))
            return student;

        var lectureToAdd = new Lecture { Id = lectureToAddId };

        context.Lectures.Attach(lectureToAdd);
        student.Lectures?.Add(lectureToAdd);

        await context.SaveChangesAsync();

        return student;
    }

    public async Task<Student?> RemoveLectureFromStudent(Guid studentId, Guid lectureToRemoveId)
    {
        var student = await context
            .Students.AsNoTracking()
            .Where(s => s.Id == studentId)
            .Include(s => s.Lectures)
            .FirstAsync();

        if (!student.Lectures.Any(l => l.Id == lectureToRemoveId))
            return student;

        var lectureToRemove = new Lecture { Id = lectureToRemoveId };
        student.Lectures?.Remove(lectureToRemove);

        await context.SaveChangesAsync();

        return student;
    }

    public async Task<Student?> DeleteStudent(Guid studentId)
    {
        var student = await context
            .Students.Include(s => s.Lectures)
            .FirstOrDefaultAsync(s => s.Id == studentId);

        if (student is null)
            return null;

        if (student.Lectures is not null)
        {
            foreach (var lecture in student.Lectures)
            {
                lecture.Students?.Remove(student);
            }
        }

        context.Students.Remove(student);

        await context.SaveChangesAsync();

        return student;
    }
}
