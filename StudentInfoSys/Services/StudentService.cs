using Microsoft.EntityFrameworkCore;
using StudentInfoSys.Models.Inputs;
using StudentIS.Models;

namespace StudentInfoSys.Services;

public class StudentService
{
    public async Task<Student> CreateStudent(AppContext context, CreateStudentInput input)
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

    public Student? GetStudentByName(AppContext context, string studentName)
    {
        return context.Students.AsNoTracking().FirstOrDefault(s => s.Name.Contains(studentName));
    }

    public async Task<Student> ChangeStudentDepartment(
        AppContext context,
        string studentName,
        Department newDepartment
    )
    {
        var student = GetStudentByName(context, studentName);

        if (student is null)
        {
            throw new NullReferenceException("Student not found");
        }

        student.DepartmentId = newDepartment.Id;
        // Does this need to be set? Since it's not set on line 11
        student.Department = newDepartment;

        await context.SaveChangesAsync();

        return student;
    }

    public async Task<Student> AddLectureToStudent(
        AppContext context,
        string studentName,
        Lecture lectureToAdd
    )
    {
        var student = GetStudentByName(context, studentName);

        if (student is null)
        {
            throw new NullReferenceException("Student not found");
        }

        student.Lectures?.Add(lectureToAdd);
        await context.SaveChangesAsync();

        return student;
    }

    public async Task<Student> RemoveLectureFromStudent(
        AppContext context,
        string studentName,
        Lecture lectureToRemove
    )
    {
        var student = GetStudentByName(context, studentName);

        if (student is null)
        {
            throw new NullReferenceException("Student not found");
        }

        student.Lectures?.Remove(lectureToRemove);

        await context.SaveChangesAsync();

        return student;
    }

    // public Dictionary<Student, List<Lecture>> GetLecturesByStudent(
    //     StudentInfoSystemContext context
    // )
    // {
    //     var result = new Dictionary<Student, List<Lecture>>();
    //     var students = context.Students.ToList();
    //     var lectures = context.Lectures.ToList();
    //
    //     foreach (var student in students)
    //     {
    //         result.Add(student,lectures.Where(l=> l.Students.FindAll(s=>s.Id == student.Id)).);
    //     }
    //
    //     return result;
    // }

    public async Task<Student> DeleteStudent(AppContext context, Guid studentId)
    {
        var student = context.Students.FirstOrDefault(s => s.Id == studentId);

        if (student is null)
        {
            throw new NullReferenceException("Student not found");
        }

        context.Students.Remove(student);
        await context.SaveChangesAsync();

        return student;
    }
}
