using Microsoft.EntityFrameworkCore;
using StudentInfoSys.Models.Inputs;
using StudentIS.Models;

namespace StudentInfoSys.Services;

public class LectureService
{
    public async Task<Lecture> CreateLecture(AppContext context, CreateLectureInput input)
    {
        var lecture = new Lecture
        {
            Title = input.Title,
            Students = input.Students,
            Departments = input.Departments,
        };

        context.Lectures.Add(lecture);
        await context.SaveChangesAsync();
        return lecture;
    }

    public Lecture? GetLectureByName(AppContext context, string lectureName)
    {
        return context.Lectures.AsNoTracking().FirstOrDefault(l => l.Title.Contains(lectureName));
    }

    public async Task<List<Lecture>> GetAllLecturesSortedByStudent(AppContext context)
    {
        return context.Lectures.AsNoTracking().OrderBy(x => x.Students).ToList();
    }

    public async Task<Lecture> AddStudentToLecture(
        AppContext context,
        string lectureName,
        Student studentToAdd
    )
    {
        var lecture = GetLectureByName(context, lectureName);

        if (lecture is null)
        {
            throw new NullReferenceException("Lecture not found!");
        }

        lecture.Students?.Add(studentToAdd);
        studentToAdd.Lectures.Add(lecture);

        await context.SaveChangesAsync();

        return lecture;
    }

    public async Task<Lecture> DeleteLecture(AppContext context, Guid lectureId)
    {
        var lecture = context.Lectures.FirstOrDefault(l => l.Id == lectureId);

        if (lecture is null)
        {
            throw new NullReferenceException("Lecture not found!");
        }

        context.Lectures.Remove(lecture);

        await context.SaveChangesAsync();

        return lecture;
    }
}
