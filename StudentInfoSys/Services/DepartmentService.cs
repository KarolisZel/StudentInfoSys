using Microsoft.EntityFrameworkCore;
using StudentInfoSys.Models;

namespace StudentInfoSys.Services;

public interface IDepartmentService
{
    Task<Department> CreateDepartment(CreateDepartmentInput input);
    Task<Department?> GetDepartmentByName(string departmentName);
    Task<List<Student>?> GetAllStudentsInDepartment(string departmentName);
    Task<List<Lecture>?> GetAllLecturesInDepartment(string departmentName);
    Task<Department?> AddStudentToDepartment(string departmentName, Student studentToAdd);
    Task<Department?> AddLectureToDepartment(string departmentName, Lecture lectureToAdd);
    Task<Department?> DeleteDepartment(Guid id);
}

public record CreateDepartmentInput(string Name, List<Student>? Students, List<Lecture>? Lectures);

public class DepartmentService(UniversityContext context, ILogger<DepartmentService> logger)
    : IDepartmentService
{
    public async Task<Department> CreateDepartment(CreateDepartmentInput input)
    {
        var department = new Department { Name = input.Name, };

        if (input.Students is not null)
        {
            foreach (var student in input.Students)
            {
                var existingStudent = await context.Students.FindAsync(student.Id);
                if (existingStudent is not null)
                    department.Students?.Add(existingStudent);
            }
        }

        if (input.Lectures is not null)
        {
            foreach (var lecture in input.Lectures)
            {
                var existingLecture = await context.Lectures.FindAsync(lecture.Id);
                if (existingLecture is not null)
                    department.Lectures?.Add(existingLecture);
            }
        }

        context.Departments.Add(department);
        await context.SaveChangesAsync();
        return department;
    }

    public async Task<Department?> GetDepartmentByName(string departmentName)
    {
        var result = await context
            .Departments.AsNoTracking()
            .FirstOrDefaultAsync(d => d.Name.Equals(departmentName));

        return result;
    }

    public async Task<List<Student>?> GetAllStudentsInDepartment(string departmentName)
    {
        var department = await GetDepartmentByName(departmentName);

        return department is null
            ? null
            : context.Students.Where(s => s.DepartmentId == department.Id).ToList();
    }

    public async Task<List<Lecture>?> GetAllLecturesInDepartment(string departmentName)
    {
        var department = await GetDepartmentByName(departmentName);

        return department is null
            ? null
            : context.Lectures.Where(l => l.Departments.Contains(department)).ToList();
    }

    public async Task<Department?> AddStudentToDepartment(
        string departmentName,
        Student studentToAdd
    )
    {
        var department = await GetDepartmentByName(departmentName);

        if (department is null)
            return null;

        if (studentToAdd.DepartmentId is not null)
        {
            var existingDepartment = await context.Departments.FirstOrDefaultAsync(d =>
                d.Id == studentToAdd.DepartmentId
            );
            return existingDepartment;
        }

        department.Students?.Add(studentToAdd);
        studentToAdd.DepartmentId = department.Id;

        await context.SaveChangesAsync();

        return department;
    }

    public async Task<Department?> AddLectureToDepartment(
        string departmentName,
        Lecture lectureToAdd
    )
    {
        var department = await GetDepartmentByName(departmentName);

        if (department is null)
            return null;

        department.Lectures?.Add(lectureToAdd);
        lectureToAdd.Departments?.Add(department);

        await context.SaveChangesAsync();

        return department;
    }

    public async Task<Department?> DeleteDepartment(Guid id)
    {
        var department = await context
            .Departments.Include(d => d.Students)
            .Include(d => d.Lectures)
            .FirstOrDefaultAsync(d => d.Id == id);

        if (department is null)
            return null;

        if (department.Students is not null)
        {
            foreach (var student in department.Students)
            {
                student.Department = null;
                student.DepartmentId = null;
            }
        }

        if (department.Lectures is not null)
        {
            foreach (var lecture in department.Lectures)
            {
                lecture.Departments?.Remove(department);
            }
        }

        context.Departments.Remove(department);

        await context.SaveChangesAsync();

        return department;
    }
}
