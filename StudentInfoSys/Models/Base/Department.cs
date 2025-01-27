namespace StudentIS.Models;

public class Department()
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Name { get; set; }

    public List<Student>? Students { get; set; } = [];
    public List<Lecture>? Lectures { get; set; } = [];

    // public Department(string name, Lecture lecture)
    //     : this(name)
    // {
    //     Lectures.Add(lecture);
    // }
    //
    // public Department(string name, Student student)
    //     : this(name)
    // {
    //     Students.Add(student);
    // }
}
