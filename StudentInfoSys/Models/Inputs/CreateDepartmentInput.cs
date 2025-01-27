using StudentIS.Models;

namespace StudentInfoSys.Models.Inputs;

public record CreateDepartmentInput(string Name, List<Student>? Students, List<Lecture>? Lectures);
