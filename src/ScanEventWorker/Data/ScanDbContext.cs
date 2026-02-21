using Microsoft.EntityFrameworkCore;
using ScanEventWorker.Data.Entities;

public class ScanDbContext : DbContext
{
    public ScanDbContext(DbContextOptions<ScanDbContext> options) : base(options) { }

    public DbSet<ParcelState> Parcels => Set<ParcelState>();
    public DbSet<WorkerState> WorkerStates => Set<WorkerState>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WorkerState>().HasKey(x => x.Id);
        modelBuilder.Entity<ParcelState>().HasKey(x => x.ParcelId);
    }
}