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
        new()
        {
            Id = Id,
            Name = Name,
            DepartmentId = DepartmentId,
            Department = Department?.ToDto(),
            Lectures = Lectures?.Select(l => l.ToDto()).ToList() ?? new List<LectureDto>(),
        };

    public static Student FromDto(StudentDto dto) =>
        new()
        {
            Id = dto.Id,
            Name = dto.Name,
            DepartmentId = dto.DepartmentId,
            Department = dto.Department != null ? Department.FromDto(dto.Department) : null,
            Lectures = dto.Lectures?.Select(Lecture.FromDto).ToList() ?? new List<Lecture>(),
        };
}
