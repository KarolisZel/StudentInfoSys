using StudentIS.Models;

namespace StudentInfoSys.Models.Inputs;

public record CreateStudentInput(string Name, Guid? DepartmentId, List<Lecture>? Lectures);
