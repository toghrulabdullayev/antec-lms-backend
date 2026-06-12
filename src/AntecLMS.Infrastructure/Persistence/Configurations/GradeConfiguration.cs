using AntecLMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AntecLMS.Infrastructure.Persistence.Configurations;

public class GradeConfiguration : IEntityTypeConfiguration<Grade>
{
  public void Configure(EntityTypeBuilder<Grade> builder)
  {
    builder.ToTable("grades");
    builder.HasKey(g => g.Id);
    builder.Property(g => g.Score).IsRequired();
    builder.Property(g => g.MaxScore).IsRequired();
    builder.Property(g => g.TeacherNote).HasMaxLength(500);

    builder
      .HasOne(g => g.Lesson)
      .WithMany(l => l.Grades)
      .HasForeignKey(g => g.LessonId)
      .OnDelete(DeleteBehavior.Cascade);

    builder
      .HasOne(g => g.Student)
      .WithMany(s => s.Grades)
      .HasForeignKey(g => g.StudentId)
      .OnDelete(DeleteBehavior.Cascade);
  }
}
