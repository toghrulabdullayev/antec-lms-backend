using AntecLMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AntecLMS.Infrastructure.Persistence.Configurations;

public class GroupConfiguration : IEntityTypeConfiguration<Group>
{
  public void Configure(EntityTypeBuilder<Group> builder)
  {
    builder.ToTable("groups");
    builder.HasKey(g => g.Id);
    builder.Property(g => g.Name).IsRequired().HasMaxLength(200);
    builder.Property(g => g.Status).HasConversion<string>();

    builder
      .HasOne(g => g.Course)
      .WithMany(c => c.Groups)
      .HasForeignKey(g => g.CourseId)
      .OnDelete(DeleteBehavior.Restrict);

    builder
      .HasOne(g => g.Teacher)
      .WithMany(t => t.Groups)
      .HasForeignKey(g => g.TeacherId)
      .OnDelete(DeleteBehavior.Restrict);
  }
}
