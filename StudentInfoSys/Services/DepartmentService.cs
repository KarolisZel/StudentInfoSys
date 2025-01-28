using Microsoft.EntityFrameworkCore;
using StudentInfoSys.Models;

namespace StudentInfoSys.Services;

public interface IDepartmentService
{
    Task<Department> CreateDepartment(CreateDepartmentInput input);
    Task<Department> GetDepartmentByName(string departmentName);
    Task<List<Student>> GetAllStudentsInDepartment(string departmentName);
    Task<List<Lecture>> GetAllLecturesInDepartment(string departmentName);
    Task<Department> AddStudentToDepartment(string departmentName, Student studentToAdd);
    Task<Department> AddLectureToDepartment(string departmentName, Lecture lectureToAdd);
    Task<Department> DeleteDepartment(Guid id);
}

public record CreateDepartmentInput(string Name, List<Student>? Students, List<Lecture>? Lectures);

public class DepartmentService(UniversityContext context, ILogger<DepartmentService> logger)
    : IDepartmentService
{
    public async Task<Department> CreateDepartment(CreateDepartmentInput input)
    {
        var department = new Department
        {
            Name = input.Name,
            Students = input.Students,
            Lectures = input.Lectures,
        };

        context.Departments.Add(department);

        await context.SaveChangesAsync();

        return department;
    }

    public async Task<Department> GetDepartmentByName(string departmentName)
    {
        var result = await context
            .Departments.AsNoTracking()
            .FirstOrDefaultAsync(d => d.Name.Contains(departmentName));

        if (result is null)
            throw new NullReferenceException("Department not found!");

        return result;
    }

    public async Task<List<Student>> GetAllStudentsInDepartment(string departmentName)
    {
        var department = await GetDepartmentByName(departmentName);

        if (department is null)
        {
            throw new NullReferenceException("Department not found!");
        }

        return context.Students.Where(s => s.DepartmentId == department.Id).ToList();
    }

    public async Task<List<Lecture>> GetAllLecturesInDepartment(string departmentName)
    {
        var department = await GetDepartmentByName(departmentName);

        if (department is null)
        {
            throw new NullReferenceException("Department not found!");
        }

        return context.Lectures.Where(l => l.Departments.Contains(department)).ToList();
    }

    public async Task<Department> AddStudentToDepartment(
        string departmentName,
        Student studentToAdd
    )
    {
        var department = await GetDepartmentByName(departmentName);

        if (department is null)
            throw new NullReferenceException("Department not found!");

        department.Students?.Add(studentToAdd);
        studentToAdd.DepartmentId = department.Id;

        await context.SaveChangesAsync();

        return department;
    }

    public async Task<Department> AddLectureToDepartment(
        string departmentName,
        Lecture lectureToAdd
    )
    {
        var department = await GetDepartmentByName(departmentName);

        if (department is null)
            throw new NullReferenceException("Department not found!");

        department.Lectures?.Add(lectureToAdd);
        lectureToAdd.Departments?.Add(department);

        await context.SaveChangesAsync();

        return department;
    }

    public async Task<Department> DeleteDepartment(Guid id)
    {
        var department = await context.Departments.FirstOrDefaultAsync(d => d.Id == id);

        if (department is null)
        {
            throw new NullReferenceException("Department not found!");
        }

        context.Departments.Remove(department);

        await context.SaveChangesAsync();

        return department;
    }
}
