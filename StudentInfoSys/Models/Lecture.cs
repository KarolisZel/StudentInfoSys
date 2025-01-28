using StudentInfoSys.Contracts;

namespace StudentInfoSys.Models;

public class Lecture()
{
    public Guid Id { get; set; }
    public required string Title { get; set; }

    public List<Student>? Students { get; set; }
    public List<Department>? Departments { get; set; }

    public LectureDto ToDto() => new LectureDto { Id = Id, Title = Title, };

    public Lecture FromDto(LectureDto dto) => new() { Title = dto.Title, };
}
