using AntecLMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AntecLMS.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
  public AppDbContext(DbContextOptions<AppDbContext> options)
    : base(options) { }

  public DbSet<User> Users => Set<User>();
  public DbSet<Course> Courses => Set<Course>();
  public DbSet<Group> Groups => Set<Group>();
  public DbSet<Teacher> Teachers => Set<Teacher>();
  public DbSet<Student> Students => Set<Student>();
  public DbSet<GroupStudent> GroupStudents => Set<GroupStudent>();
  public DbSet<Attendance> Attendances => Set<Attendance>();
  public DbSet<Grade> Grades => Set<Grade>();
  public DbSet<Lesson> Lessons => Set<Lesson>();
  public DbSet<Material> Materials => Set<Material>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.HasDefaultSchema("public");
    modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    base.OnModelCreating(modelBuilder);
  }
}
