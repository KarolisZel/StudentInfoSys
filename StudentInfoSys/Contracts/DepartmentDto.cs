namespace StudentInfoSys.Contracts;

public class DepartmentDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }

    public List<StudentDto>? Students { get; set; }
    public List<LectureDto>? Lectures { get; set; }
}
