using System.Text.Json.Serialization;
using StudentInfoSys.Contracts;

namespace StudentInfoSys.Models;

public class Department()
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public List<Student>? Students { get; set; }

    public List<Lecture>? Lectures { get; set; }

    public DepartmentDto ToDto() =>
        new()
        {
            Id = Id,
            Name = Name,
            Students =
                Students?.Select(s => s.ToNoExtraDto()).ToList() ?? new List<StudentNoExtraDto>(),
            Lectures =
                Lectures?.Select(l => l.ToNoExtraDto()).ToList() ?? new List<LectureNoExtraDto>(),
        };

    public DepartmentNoExtraDto ToNoExtraDto() => new() { Id = Id, Name = Name };
}
