using AntecLMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AntecLMS.Infrastructure.Persistence.Configurations;

public class LessonConfiguration : IEntityTypeConfiguration<Lesson>
{
  public void Configure(EntityTypeBuilder<Lesson> builder)
  {
    builder.ToTable("lessons");
    builder.HasKey(l => l.Id);
    builder.Property(l => l.Topic).HasMaxLength(300);
    builder.Property(l => l.Note).HasMaxLength(1000);
    builder.Property(l => l.Status).HasConversion<string>().IsRequired().HasMaxLength(50);

    builder
      .HasOne(l => l.Group)
      .WithMany(g => g.Lessons)
      .HasForeignKey(l => l.GroupId)
      .OnDelete(DeleteBehavior.Cascade);

    builder
      .HasOne(l => l.Teacher)
      .WithMany(t => t.Lessons)
      .HasForeignKey(l => l.TeacherId)
      .OnDelete(DeleteBehavior.Restrict);
  }
}
