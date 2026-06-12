using AntecLMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AntecLMS.Infrastructure.Persistence.Configurations;

public class MaterialConfiguration : IEntityTypeConfiguration<Material>
{
  public void Configure(EntityTypeBuilder<Material> builder)
  {
    builder.ToTable("materials");
    builder.HasKey(m => m.Id);
    builder.Property(m => m.Title).IsRequired().HasMaxLength(300);
    builder.Property(m => m.Type).HasMaxLength(100);
    builder.Property(m => m.Url).HasMaxLength(1000);
    builder.Property(m => m.FilePath).HasMaxLength(1000);
    builder.Property(m => m.Description).HasMaxLength(1000);

    builder
      .HasOne(m => m.Lesson)
      .WithMany(l => l.Materials)
      .HasForeignKey(m => m.LessonId)
      .OnDelete(DeleteBehavior.Cascade);

    builder
      .HasOne(m => m.Group)
      .WithMany(g => g.Materials)
      .HasForeignKey(m => m.GroupId)
      .OnDelete(DeleteBehavior.Restrict);

    builder
      .HasOne(m => m.Teacher)
      .WithMany(t => t.Materials)
      .HasForeignKey(m => m.TeacherId)
      .OnDelete(DeleteBehavior.Restrict);
  }
}
