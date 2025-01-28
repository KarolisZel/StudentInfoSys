namespace StudentInfoSys.Contracts;

public class LectureDto
{
    public Guid Id { get; set; }
    public required string Title { get; set; }

    public List<StudentDto>? Students { get; set; }
    public List<DepartmentDto>? Departments { get; set; }
}
