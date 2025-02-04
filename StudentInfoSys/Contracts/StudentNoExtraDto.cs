namespace StudentInfoSys.Contracts;

public class StudentNoExtraDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public Guid? DepartmentId { get; set; }
}
