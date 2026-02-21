using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ScanEventWorker.Data;

public class ScanDbContextFactory : IDesignTimeDbContextFactory<ScanDbContext>
{
    public ScanDbContext CreateDbContext(string[] args)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<ScanDbContext>();
        optionsBuilder.UseSqlServer(
            config.GetConnectionString("DefaultConnection"));

        return new ScanDbContext(optionsBuilder.Options);
    }
}