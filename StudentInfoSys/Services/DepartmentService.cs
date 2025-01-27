using Microsoft.EntityFrameworkCore;
using StudentInfoSys.Models.Inputs;
using StudentIS.Models;

namespace StudentInfoSys.Services;

public class DepartmentService
{
    public async Task<Department> CreateDepartment(AppContext context, CreateDepartmentInput input)
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

    public Department? GetDepartmentByName(AppContext context, string departmentName)
    {
        return context
            .Departments.AsNoTracking()
            .FirstOrDefault(d => d.Name.Contains(departmentName));
    }

    public List<Student> GetAllStudentsInDepartment(AppContext context, string departmentName)
    {
        var department = GetDepartmentByName(context, departmentName);

        if (department is null)
        {
            throw new NullReferenceException("Department not found!");
        }

        return context.Students.Where(s => s.DepartmentId == department.Id).ToList();
    }

    public List<Lecture> GetAllLecturesInDepartment(AppContext context, string departmentName)
    {
        var department = GetDepartmentByName(context, departmentName);

        if (department is null)
        {
            throw new NullReferenceException("Department not found!");
        }

        return context.Lectures.Where(l => l.Departments.Contains(department)).ToList();
    }

    public async Task<Department> AddStudentToDepartment(
        AppContext context,
        string departmentName,
        Student studentToAdd
    )
    {
        var department = GetDepartmentByName(context, departmentName);

        if (department is null)
            throw new NullReferenceException("Department not found!");

        department.Students?.Add(studentToAdd);
        studentToAdd.DepartmentId = department.Id;

        await context.SaveChangesAsync();

        return department;
    }

    public async Task<Department> AddLectureToDepartment(
        AppContext context,
        string departmentName,
        Lecture lectureToAdd
    )
    {
        var department = await context.Departments.FirstOrDefaultAsync(d =>
            d.Name == departmentName
        );

        if (department is null)
            throw new NullReferenceException("Department not found!");

        department.Lectures?.Add(lectureToAdd);
        lectureToAdd.Departments?.Add(department);

        await context.SaveChangesAsync();

        return department;
    }

    public async Task<Department> DeleteDepartment(AppContext context, Guid id)
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
