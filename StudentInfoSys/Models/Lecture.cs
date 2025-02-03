using System.Text.Json.Serialization;
using StudentInfoSys.Contracts;

namespace StudentInfoSys.Models;

public class Lecture()
{
    public Guid Id { get; set; }
    public string Title { get; set; }

    [JsonIgnore]
    public List<Student>? Students { get; set; }

    [JsonIgnore]
    public List<Department>? Departments { get; set; }

    public LectureDto ToDto() =>
        new()
        {
            Id = Id,
            Title = Title,
            Students = Students?.Select(s => s.ToDto()).ToList() ?? new List<StudentDto>(),
            Departments = Departments?.Select(d => d.ToDto()).ToList() ?? new List<DepartmentDto>()
        };

    public static Lecture FromDto(LectureDto dto) =>
        new()
        {
            Id = dto.Id,
            Title = dto.Title,
            Students = dto.Students?.Select(Student.FromDto).ToList() ?? new List<Student>(),
            Departments =
                dto.Departments?.Select(Department.FromDto).ToList() ?? new List<Department>()
        };
}
