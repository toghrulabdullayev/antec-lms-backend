using AntecLMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AntecLMS.Infrastructure.Persistence.Configurations;

public class AttendanceConfiguration : IEntityTypeConfiguration<Attendance>
{
  public void Configure(EntityTypeBuilder<Attendance> builder)
  {
    builder.ToTable("attendances");
    builder.HasKey(a => a.Id);
    builder.Property(a => a.Status).HasConversion<string>().IsRequired().HasMaxLength(50);
    builder.Property(a => a.Reason).HasMaxLength(500);
    builder.Property(a => a.TeacherNote).HasMaxLength(500);

    builder
      .HasOne(a => a.Lesson)
      .WithMany(l => l.Attendances)
      .HasForeignKey(a => a.LessonId)
      .OnDelete(DeleteBehavior.Cascade);

    builder
      .HasOne(a => a.Student)
      .WithMany(s => s.Attendances)
      .HasForeignKey(a => a.StudentId)
      .OnDelete(DeleteBehavior.Cascade);

    builder.HasIndex(a => new { a.LessonId, a.StudentId }).IsUnique();
  }
}
