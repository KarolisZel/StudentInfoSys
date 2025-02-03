using Microsoft.EntityFrameworkCore;
using StudentInfoSys.Models;

namespace StudentInfoSys.Services;

public interface IDepartmentService
{
    Task<Department> CreateDepartment(CreateDepartmentInput input);
    Task<List<Department>> GetAllDepartments();
    Task<Department?> GetDepartmentById(Guid departmentId);
    Task<List<Student>?> GetAllStudentsInDepartment(Guid departmentId);
    Task<List<Lecture>?> GetAllLecturesInDepartment(Guid departmentId);
    Task<Department?> AddStudentToDepartment(Guid departmentId, Guid studentToAddId);
    Task<Department?> AddLectureToDepartment(Guid departmentId, Guid lectureToAddId);
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

    public async Task<List<Department>> GetAllDepartments()
    {
        var departments = await context
            .Departments.AsNoTracking()
            .Include(d => d.Lectures)
            .Include(d => d.Students)
            .ToListAsync();

        return departments;
    }

    public async Task<Department?> GetDepartmentById(Guid departmentId)
    {
        var result = await context
            .Departments.AsNoTracking()
            .Where(d => d.Id == departmentId)
            .Include(d => d.Lectures)
            .Include(d => d.Students)
            .FirstAsync();

        return result;
    }

    public async Task<List<Student>?> GetAllStudentsInDepartment(Guid departmentId)
    {
        var department = await context
            .Departments.AsNoTracking()
            .Where(d => d.Id == departmentId)
            .Include(d => d.Students)
            .FirstAsync();

        return department.Students?.ToList();
    }

    public async Task<List<Lecture>?> GetAllLecturesInDepartment(Guid departmentId)
    {
        var department = await context
            .Departments.AsNoTracking()
            .Where(d => d.Id == departmentId)
            .Include(d => d.Lectures)
            .FirstAsync();

        return department.Lectures?.ToList();
    }

    public async Task<Department?> AddStudentToDepartment(Guid departmentId, Guid studentToAddId)
    {
        var department = await context
            .Departments.AsNoTracking()
            .Where(d => d.Id == departmentId)
            .Include(d => d.Students)
            .FirstAsync();

        if (department is null)
            return null;

        if (department.Students.Any(s => s.Id == studentToAddId))
            return department;

        var studentToAdd = new Student { Id = studentToAddId };

        context.Students.Attach(studentToAdd);
        department.Students?.Add(studentToAdd);

        studentToAdd.DepartmentId = departmentId;

        await context.SaveChangesAsync();

        return department;
    }

    public async Task<Department?> AddLectureToDepartment(Guid departmentId, Guid lectureToAddId)
    {
        var department = await context
            .Departments.AsNoTracking()
            .Where(d => d.Id == departmentId)
            .Include(d => d.Lectures)
            .FirstAsync();

        if (department is null)
            return null;

        if (department.Lectures.Any(l => l.Id == lectureToAddId))
            return department;

        var lectureToAdd = new Lecture { Id = lectureToAddId };

        context.Lectures.Attach(lectureToAdd);
        department.Lectures?.Add(lectureToAdd);

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
