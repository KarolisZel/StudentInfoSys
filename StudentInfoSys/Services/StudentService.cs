using Microsoft.EntityFrameworkCore;
using StudentInfoSys.Contracts;
using StudentInfoSys.Models;

namespace StudentInfoSys.Services;

public interface IStudentService
{
    Task<StudentDto> CreateStudentAsync(CreateStudentInput input);
    Task<List<StudentDto>> GetAllStudents();
    Task<StudentDto?> GetStudentByIdAsync(Guid studentId);

    Task<StudentDto?> ChangeStudentDepartmentAsync(Guid studentId, Guid newDepartmentId);

    Task<StudentDto?> AddLectureToStudentAsync(Guid studentId, Guid lectureToAddId);

    Task<StudentDto?> RemoveLectureFromStudentAsync(Guid studentId, Guid lectureToRemoveId);

    Task<StudentDto?> DeleteStudentAsync(Guid studentId);
}

public record CreateStudentInput(string Name, List<Lecture>? Lectures);

public class StudentService(UniversityContext context, ILogger<StudentService> logger)
    : IStudentService
{
    public async Task<StudentDto> CreateStudentAsync(CreateStudentInput input)
    {
        var student = new Student { Name = input.Name, };

        if (input.Lectures is not null)
        {
            var lectureIds = input.Lectures.Select(x => x.Id).ToArray();

            var existingLectures = input.Lectures.Where(x => lectureIds.Contains(x.Id)).ToArray();

            if (existingLectures.Length > 0)
                student.Lectures?.AddRange(existingLectures);
        }

        context.Students.Add(student);
        await context.SaveChangesAsync();
        return student.ToDto();
    }

    public async Task<List<StudentDto>> GetAllStudents()
    {
        var result = await context
            .Students.AsNoTracking()
            .Include(s => s.Department)
            .Include(s => s.Lectures)
            .Select(x => x.ToDto())
            .ToListAsync();

        return result;
    }

    public async Task<StudentDto?> GetStudentByIdAsync(Guid studentId)
    {
        var result = await context
            .Students.AsNoTracking()
            .Include(s => s.Lectures)
            .FirstOrDefaultAsync(s => s.Id == studentId);

        return result?.ToDto();
    }

    public async Task<StudentDto?> ChangeStudentDepartmentAsync(
        Guid studentId,
        Guid newDepartmentId
    )
    {
        var student = await context.Students.FirstOrDefaultAsync(s => s.Id == studentId);

        if (student is null)
            return null;

        student.DepartmentId = newDepartmentId;

        student.Lectures?.Clear();

        await context.SaveChangesAsync();

        return student.ToDto();
    }

    public async Task<StudentDto?> AddLectureToStudentAsync(Guid studentId, Guid lectureToAddId)
    {
        var student = await context
            .Students.Include(s => s.Lectures)
            .FirstOrDefaultAsync(s => s.Id == studentId);

        if (student.Lectures.Any(l => l.Id == lectureToAddId))
            return student.ToDto();

        var lectureToAdd = new Lecture { Id = lectureToAddId };

        student.Lectures?.Add(lectureToAdd);

        await context.SaveChangesAsync();

        return student.ToDto();
    }

    public async Task<StudentDto?> RemoveLectureFromStudentAsync(
        Guid studentId,
        Guid lectureToRemoveId
    )
    {
        var student = await context
            .Students.AsNoTracking()
            .Include(s => s.Lectures)
            .FirstOrDefaultAsync(s => s.Id == studentId);

        if (!student.Lectures.Any(l => l.Id == lectureToRemoveId))
            return student.ToDto();

        var lectureToRemove = new Lecture { Id = lectureToRemoveId };
        student.Lectures?.Remove(lectureToRemove);

        await context.SaveChangesAsync();

        return student.ToDto();
    }

    public async Task<StudentDto?> DeleteStudentAsync(Guid studentId)
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

        return student.ToDto();
    }
}
