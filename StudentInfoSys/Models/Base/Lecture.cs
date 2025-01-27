namespace StudentIS.Models;

public class Lecture()
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Title { get; set; }

    public List<Student>? Students { get; set; } = [];
    public List<Department>? Departments { get; set; } = [];

    // public Lecture(string title, Student student)
    //     : this(title)
    // {
    //     Students.Add(student);
    // }
    //
    // public Lecture(string title, Department department)
    //     : this(title)
    // {
    //     Departments.Add(department);
    // }
}
