using AntecLMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AntecLMS.Infrastructure.Persistence.Configurations;

public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
{
  public void Configure(EntityTypeBuilder<Teacher> builder)
  {
    builder.ToTable("teachers");
    builder.HasKey(t => t.Id);
    builder.Property(t => t.Specialization).HasMaxLength(500);
    builder.Property(t => t.Bio).HasMaxLength(1000);
    builder.Property(t => t.Status).HasConversion<string>();
  }
}
