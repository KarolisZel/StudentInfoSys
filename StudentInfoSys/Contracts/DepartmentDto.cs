namespace StudentInfoSys.Contracts;

public class DepartmentDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }

    public List<StudentNoExtraDto>? Students { get; set; }
    public List<LectureNoExtraDto>? Lectures { get; set; }
}
