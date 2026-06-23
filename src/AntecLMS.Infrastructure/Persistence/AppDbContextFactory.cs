using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace AntecLMS.Infrastructure.Persistence;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
  public AppDbContext CreateDbContext(string[] args)
  {
    var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
    optionsBuilder
      .UseNpgsql(
        "Host=localhost;Port=5432;Database=anteclms;Username=antec;Password=Antec123!",
        o => o.MigrationsHistoryTable("__EFMigrationsHistory", "public")
      )
      .UseSnakeCaseNamingConvention();
    return new AppDbContext(optionsBuilder.Options);
  }
}
