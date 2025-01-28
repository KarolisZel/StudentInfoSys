using StudentInfoSys.Contracts;

namespace StudentInfoSys.Models;

public class Department()
{
    public Guid Id { get; set; }
    public required string Name { get; set; }

    public List<Student>? Students { get; set; }
    public List<Lecture>? Lectures { get; set; }

    public DepartmentDto ToDto() => new() { Id = Id, Name = Name, };

    public static Department FromDto(DepartmentDto dto) => new() { Name = dto.Name, };
}
