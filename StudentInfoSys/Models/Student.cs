using System.Text.Json.Serialization;
using StudentInfoSys.Contracts;

namespace StudentInfoSys.Models;

public class Student()
{
    public Guid Id { get; set; }
    public string Name { get; set; }

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
            Lectures =
                Lectures?.Select(l => l.ToNoExtraDto()).ToList() ?? new List<LectureNoExtraDto>(),
        };

    public StudentNoExtraDto ToNoExtraDto() =>
        new()
        {
            Id = Id,
            Name = Name,
            DepartmentId = DepartmentId
        };
}
