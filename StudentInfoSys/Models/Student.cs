using StudentInfoSys.Contracts;

namespace StudentInfoSys.Models;

public class Student()
{
    public Guid Id { get; set; }
    public required string Name { get; set; }

    public Guid? DepartmentId { get; set; }
    public Department? Department { get; set; }
    public List<Lecture>? Lectures { get; set; }

    public StudentDto ToDto() =>
        new StudentDto
        {
            Id = Id,
            Name = Name,
            DepartmentId = DepartmentId,
            Department = Department?.ToDto(),
        };

    public Student FromDto(StudentDto dto) =>
        new() { Name = dto.Name, DepartmentId = dto.DepartmentId, };
}
