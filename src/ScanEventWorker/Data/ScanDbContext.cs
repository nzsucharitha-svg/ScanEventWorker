using Microsoft.EntityFrameworkCore;
using ScanEventWorker.Data.Entities;

namespace ScanEventWorker.Data;

public class ScanDbContext : DbContext
{
    public ScanDbContext(DbContextOptions<ScanDbContext> options) : base(options) { }

    public DbSet<ParcelState> ParcelStates => Set<ParcelState>();
    public DbSet<WorkerState> WorkerStates => Set<WorkerState>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WorkerState>().HasKey(x => x.Id);
        modelBuilder.Entity<WorkerState>()
        .ToTable("WorkerStates");
        modelBuilder.Entity<ParcelState>().HasKey(x => x.ParcelId);
        modelBuilder.Entity<ParcelState>()
        .ToTable("ParcelStates");
    }
}