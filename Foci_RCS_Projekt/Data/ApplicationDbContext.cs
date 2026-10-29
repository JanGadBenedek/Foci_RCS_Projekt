using Microsoft.EntityFrameworkCore;
using Foci_RCS_Projekt.Models;

namespace Foci_RCS_Projekt.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Csapat> Csapatok { get; set; }
    public DbSet<Merkozes> Merkozesek { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Csapat>()
            .HasIndex(c => c.Nev)
            .IsUnique();

        modelBuilder.Entity<Merkozes>()
            .HasOne(m => m.HazaiCsapat)
            .WithMany(c => c.HazaiMerkozesek)
            .HasForeignKey(m => m.HazaiCsapatId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Merkozes>()
            .HasOne(m => m.VendegCsapat)
            .WithMany(c => c.VendegMerkozesek)
            .HasForeignKey(m => m.VendegCsapatId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Merkozes>()
            .HasIndex(m => new
            {
                m.HazaiCsapatId,
                m.VendegCsapatId
            })
            .IsUnique();
    }
}