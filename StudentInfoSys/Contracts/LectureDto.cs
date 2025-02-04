namespace StudentInfoSys.Contracts;

public class LectureDto
{
    public Guid Id { get; set; }
    public required string Title { get; set; }

    public List<StudentNoExtraDto>? Students { get; set; }
    public List<DepartmentNoExtraDto>? Departments { get; set; }
}
