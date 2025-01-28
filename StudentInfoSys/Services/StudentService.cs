using Microsoft.EntityFrameworkCore;
using StudentInfoSys.Models;

namespace StudentInfoSys.Services;

public interface IStudentService
{
    Task<Student> CreateStudent(CreateStudentInput input);
    Task<Student> GetStudentByName(string studentName);

    Task<Student> ChangeStudentDepartment(string studentName, Department newDepartment);

    Task<Student> AddLectureToStudent(string studentName, Lecture lectureToAdd);

    Task<Student> RemoveLectureFromStudent(string studentName, Lecture lectureToRemove);

    Task<Student> DeleteStudent(Guid studentId);
}

public record CreateStudentInput(string Name, Guid? DepartmentId, List<Lecture>? Lectures);

public class StudentService(UniversityContext context, ILogger<StudentService> logger)
    : IStudentService
{
    public async Task<Student> CreateStudent(CreateStudentInput input)
    {
        var student = new Student
        {
            Name = input.Name,
            DepartmentId = input.DepartmentId,
            Lectures = input.Lectures,
        };

        context.Students.Add(student);

        await context.SaveChangesAsync();

        return student;
    }

    public async Task<Student> GetStudentByName(string studentName)
    {
        var result = await context
            .Students.AsNoTracking()
            .FirstOrDefaultAsync(s => s.Name.Contains(studentName));

        if (result is null)
        {
            throw new NullReferenceException("Student not found!");
        }

        return result;
    }

    public async Task<Student> ChangeStudentDepartment(string studentName, Department newDepartment)
    {
        var student = await GetStudentByName(studentName);

        if (student is null)
        {
            throw new NullReferenceException("Student not found");
        }

        student.DepartmentId = newDepartment.Id;
        // Does this need to be set? Since it's not set in `CreateStudent` method
        // student.Department = newDepartment;

        await context.SaveChangesAsync();

        return student;
    }

    public async Task<Student> AddLectureToStudent(string studentName, Lecture lectureToAdd)
    {
        var student = await GetStudentByName(studentName);

        if (student is null)
        {
            throw new NullReferenceException("Student not found");
        }

        student.Lectures?.Add(lectureToAdd);
        await context.SaveChangesAsync();

        return student;
    }

    public async Task<Student> RemoveLectureFromStudent(string studentName, Lecture lectureToRemove)
    {
        var student = await GetStudentByName(studentName);

        if (student is null)
        {
            throw new NullReferenceException("Student not found");
        }

        student.Lectures?.Remove(lectureToRemove);

        await context.SaveChangesAsync();

        return student;
    }

    public async Task<Student> DeleteStudent(Guid studentId)
    {
        var student = await context.Students.FirstOrDefaultAsync(s => s.Id == studentId);

        if (student is null)
        {
            throw new NullReferenceException("Student not found");
        }

        context.Students.Remove(student);
        await context.SaveChangesAsync();

        return student;
    }
}
