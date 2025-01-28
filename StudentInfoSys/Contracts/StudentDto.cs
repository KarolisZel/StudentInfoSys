namespace StudentInfoSys.Contracts;

public class StudentDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }

    public Guid? DepartmentId { get; set; }
    public DepartmentDto? Department { get; set; }
    public List<LectureDto>? Lectures { get; set; }
}
