using AntecLMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AntecLMS.Infrastructure.Persistence.Configurations;

public class GroupScheduleConfiguration : IEntityTypeConfiguration<GroupSchedule>
{
  public void Configure(EntityTypeBuilder<GroupSchedule> builder)
  {
    builder.ToTable("group_schedules");
    builder.HasKey(s => s.Id);

    builder
      .HasOne(s => s.Group)
      .WithMany(g => g.Schedules)
      .HasForeignKey(s => s.GroupId)
      .OnDelete(DeleteBehavior.Cascade);
  }
}
