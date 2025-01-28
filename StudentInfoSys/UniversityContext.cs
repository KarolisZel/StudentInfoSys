using Microsoft.EntityFrameworkCore;
using StudentInfoSys.Models;

namespace StudentInfoSys;

public class UniversityContext(DbContextOptions<UniversityContext> options) : DbContext(options)
{
    public DbSet<Department> Departments { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Lecture> Lectures { get; set; }
}
