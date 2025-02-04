using StudentInfoSys.Services;

namespace StudentInfoSys.Helpers;

public static class StartupExtensions
{
    public static void RegisterServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IDepartmentService, DepartmentService>();
        builder.Services.AddScoped<ILectureService, LectureService>();
        builder.Services.AddScoped<IStudentService, StudentService>();
    }
}
