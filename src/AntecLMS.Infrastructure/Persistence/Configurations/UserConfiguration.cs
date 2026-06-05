using AntecLMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AntecLMS.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
  public void Configure(EntityTypeBuilder<User> builder)
  {
    builder.ToTable("users");
    builder.HasKey(u => u.Id);

    builder.Property(u => u.Name).IsRequired().HasMaxLength(100);
    builder.Property(u => u.Surname).IsRequired().HasMaxLength(100);
    builder.Property(u => u.Email).IsRequired().HasMaxLength(200);
    builder.Property(u => u.Password).IsRequired();
    builder.Property(u => u.Phone).HasMaxLength(20);
    builder.Property(u => u.Role).HasConversion<string>();
    builder.Property(u => u.Status).HasConversion<string>();

    builder.HasIndex(u => u.Email).IsUnique();

    builder
      .HasOne(u => u.TeacherProfile)
      .WithOne(t => t.User)
      .HasForeignKey<Teacher>(t => t.UserId);

    builder
      .HasOne(u => u.StudentProfile)
      .WithOne(s => s.User)
      .HasForeignKey<Student>(s => s.UserId);
  }
}
