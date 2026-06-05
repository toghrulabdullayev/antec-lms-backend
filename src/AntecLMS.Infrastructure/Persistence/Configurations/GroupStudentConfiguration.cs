using AntecLMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AntecLMS.Infrastructure.Persistence.Configurations;

public class GroupStudentConfiguration : IEntityTypeConfiguration<GroupStudent>
{
  public void Configure(EntityTypeBuilder<GroupStudent> builder)
  {
    builder.ToTable("group_students");
    builder.HasKey(gs => new { gs.GroupId, gs.StudentId });

    builder.HasOne(gs => gs.Group).WithMany(g => g.GroupStudents).HasForeignKey(gs => gs.GroupId);

    builder
      .HasOne(gs => gs.Student)
      .WithMany(s => s.GroupStudents)
      .HasForeignKey(gs => gs.StudentId);
  }
}
