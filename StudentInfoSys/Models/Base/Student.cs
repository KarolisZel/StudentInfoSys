namespace StudentIS.Models;

public class Student()
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Name { get; set; }

    public Guid? DepartmentId { get; set; }
    public Department? Department { get; set; }
    public List<Lecture>? Lectures { get; set; } = [];

    // public Student(string name, Lecture lecture)
    //     : this(name)
    // {
    //     Lectures.Add(lecture);
    // }
    //
    // public Student(string name, Department department)
    //     : this(name)
    // {
    //     DepartmentId = department.Id;
    //     Department = department;
    // }
}
