using StudentInfoSys.Contracts;

namespace StudentInfoSys.Models;

public class Department()
{
    public Guid Id { get; set; }
    public required string Name { get; set; }

    public List<Student>? Students { get; set; }
    public List<Lecture>? Lectures { get; set; }

    public DepartmentDto ToDto() =>
        new()
        {
            Id = Id,
            Name = Name,
            Students = Students?.Select(s => s.ToDto()).ToList() ?? new List<StudentDto>(),
            Lectures = Lectures?.Select(l => l.ToDto()).ToList() ?? new List<LectureDto>(),
        };

    public static Department FromDto(DepartmentDto dto) =>
        new()
        {
            Id = dto.Id,
            Name = dto.Name,
            Students = dto.Students?.Select(Student.FromDto).ToList() ?? new List<Student>(),
            Lectures = dto.Lectures?.Select(Lecture.FromDto).ToList() ?? new List<Lecture>(),
        };
}
