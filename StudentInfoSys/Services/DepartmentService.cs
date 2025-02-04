using Microsoft.EntityFrameworkCore;
using StudentInfoSys.Contracts;
using StudentInfoSys.Models;

namespace StudentInfoSys.Services;

public interface IDepartmentService
{
    Task<DepartmentDto> CreateDepartmentAsync(CreateDepartmentInput input);
    Task<List<DepartmentDto>> GetAllDepartmentsAsync();
    Task<DepartmentDto?> GetDepartmentByIdAsync(Guid departmentId);
    Task<List<StudentDto>?> GetAllStudentsInDepartmentAsync(Guid departmentId);
    Task<List<LectureDto>?> GetAllLecturesInDepartmentAsync(Guid departmentId);
    Task<DepartmentDto?> AddStudentToDepartmentAsync(Guid departmentId, Guid studentToAddId);
    Task<DepartmentDto?> AddLectureToDepartmentAsync(Guid departmentId, Guid lectureToAddId);
    Task<DepartmentDto?> DeleteDepartmentAsync(Guid id);
}

public record CreateDepartmentInput(string Name, List<Student>? Students, List<Lecture>? Lectures);

public class DepartmentService(UniversityContext context, ILogger<DepartmentService> logger)
    : IDepartmentService
{
    public async Task<DepartmentDto> CreateDepartmentAsync(CreateDepartmentInput input)
    {
        var department = new Department { Name = input.Name, };

        if (input.Students is not null)
        {
            var studentIds = input.Students.Select(x => x.Id).ToArray();

            var existingStudents = input.Students.Where(x => studentIds.Contains(x.Id)).ToArray();

            if (existingStudents.Length > 0)
                department.Students?.AddRange(existingStudents);
        }

        if (input.Lectures is not null)
        {
            var lectureIds = input.Lectures.Select(x => x.Id).ToArray();

            var existingLectures = input.Lectures.Where(x => lectureIds.Contains(x.Id)).ToArray();

            if (existingLectures.Length > 0)
                department.Lectures?.AddRange(existingLectures);
        }

        context.Departments.Add(department);

        await context.SaveChangesAsync();

        return department.ToDto();
    }

    public async Task<List<DepartmentDto>> GetAllDepartmentsAsync()
    {
        var departments = await context
            .Departments.AsNoTracking()
            .Include(d => d.Lectures)
            .Include(d => d.Students)
            .Select(x => x.ToDto())
            .ToListAsync();

        return departments;
    }

    public async Task<DepartmentDto?> GetDepartmentByIdAsync(Guid departmentId)
    {
        var result = await context
            .Departments.AsNoTracking()
            .Include(d => d.Lectures)
            .Include(d => d.Students)
            .FirstOrDefaultAsync(d => d.Id == departmentId);

        return result.ToDto();
    }

    public async Task<List<StudentDto>?> GetAllStudentsInDepartmentAsync(Guid departmentId)
    {
        var department = await context
            .Departments.AsNoTracking()
            .Include(d => d.Students)
            .FirstOrDefaultAsync(d => d.Id == departmentId);

        return department.Students?.Select(x => x.ToDto()).ToList();
    }

    public async Task<List<LectureDto>?> GetAllLecturesInDepartmentAsync(Guid departmentId)
    {
        var department = await context
            .Departments.AsNoTracking()
            .Include(d => d.Lectures)
            .FirstOrDefaultAsync(d => d.Id == departmentId);

        return department.Lectures?.Select(x => x.ToDto()).ToList();
    }

    public async Task<DepartmentDto?> AddStudentToDepartmentAsync(
        Guid departmentId,
        Guid studentToAddId
    )
    {
        var department = await context
            .Departments.Include(d => d.Students)
            .FirstOrDefaultAsync(d => d.Id == departmentId);

        if (department is null)
            return null;

        if (department.Students.Any(s => s.Id == studentToAddId))
            return department.ToDto();

        var studentToAdd = new Student { Id = studentToAddId };

        department.Students?.Add(studentToAdd);

        studentToAdd.DepartmentId = departmentId;

        await context.SaveChangesAsync();

        return department.ToDto();
    }

    public async Task<DepartmentDto?> AddLectureToDepartmentAsync(
        Guid departmentId,
        Guid lectureToAddId
    )
    {
        var department = await context
            .Departments.Include(d => d.Lectures)
            .FirstOrDefaultAsync(d => d.Id == departmentId);

        if (department is null)
            return null;

        if (department.Lectures.Any(l => l.Id == lectureToAddId))
            return department.ToDto();

        var lectureToAdd = new Lecture { Id = lectureToAddId };

        department.Lectures?.Add(lectureToAdd);

        await context.SaveChangesAsync();

        return department.ToDto();
    }

    public async Task<DepartmentDto?> DeleteDepartmentAsync(Guid id)
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

        return department.ToDto();
    }
}
