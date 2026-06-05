using AntecLMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AntecLMS.Infrastructure.Persistence.Configurations;

public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
  public void Configure(EntityTypeBuilder<Student> builder)
  {
    builder.ToTable("students");
    builder.HasKey(s => s.Id);
    builder.Property(s => s.Note).HasMaxLength(500);
    builder.Property(s => s.Status).HasConversion<string>();
  }
}
