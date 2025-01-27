using StudentIS.Models;

namespace StudentInfoSys.Models.Inputs;

public record CreateLectureInput(
    string Title,
    List<Student>? Students,
    List<Department>? Departments
);
