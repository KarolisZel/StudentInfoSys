using Microsoft.EntityFrameworkCore;
using StudentInfoSys.Models;

namespace StudentInfoSys.Services;

public interface IStudentService
{
    Task<Student> CreateStudent(CreateStudentInput input);
    Task<Student?> GetStudentByName(string studentName);

    Task<Student?> ChangeStudentDepartment(string studentName, Department newDepartment);

    Task<Student?> AddLectureToStudent(string studentName, Lecture lectureToAdd);

    Task<Student?> RemoveLectureFromStudent(string studentName, Lecture lectureToRemove);

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

    public async Task<Student?> GetStudentByName(string studentName)
    {
        var result = await context
            .Students.AsNoTracking()
            .FirstOrDefaultAsync(s => s.Name.Equals(studentName));

        return result;
    }

    public async Task<Student?> ChangeStudentDepartment(
        string studentName,
        Department newDepartment
    )
    {
        var student = await GetStudentByName(studentName);

        if (student is null)
            return null;

        student.DepartmentId = newDepartment.Id;
        student.Lectures?.Clear();
        await context.SaveChangesAsync();

        return student;
    }

    public async Task<Student?> AddLectureToStudent(string studentName, Lecture lectureToAdd)
    {
        var student = await GetStudentByName(studentName);

        if (student is null)
            return null;

        student.Lectures?.Add(lectureToAdd);
        lectureToAdd.Students?.Add(student);

        await context.SaveChangesAsync();

        return student;
    }

    public async Task<Student?> RemoveLectureFromStudent(
        string studentName,
        Lecture lectureToRemove
    )
    {
        var student = await GetStudentByName(studentName);

        if (student is null)
            return null;

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
