using System.Text.Json.Serialization;
using StudentInfoSys.Contracts;

namespace StudentInfoSys.Models;

public class Lecture()
{
    public Guid Id { get; set; }
    public string Title { get; set; }

    public List<Student>? Students { get; set; }

    public List<Department>? Departments { get; set; }

    public LectureDto ToDto() =>
        new()
        {
            Id = Id,
            Title = Title,
            Students =
                Students?.Select(s => s.ToNoExtraDto()).ToList() ?? new List<StudentNoExtraDto>(),
            Departments =
                Departments?.Select(d => d.ToNoExtraDto()).ToList()
                ?? new List<DepartmentNoExtraDto>()
        };

    public LectureNoExtraDto ToNoExtraDto() => new() { Id = Id, Title = Title };
}
