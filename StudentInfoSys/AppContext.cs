using Microsoft.EntityFrameworkCore;
using StudentIS.Models;

namespace StudentInfoSys;

public class AppContext(DbContextOptions<AppContext> options) : DbContext(options)
{
    public DbSet<Department> Departments { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Lecture> Lectures { get; set; }
}
