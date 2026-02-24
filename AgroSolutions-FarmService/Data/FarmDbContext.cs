using AgroSolutions_FarmService.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace AgroSolutions.FarmService.Data;

public class FarmDbContext : DbContext
{
    public FarmDbContext(DbContextOptions<FarmDbContext> options) : base(options)
    {
    }
    public DbSet<Propriedade> Propriedades { get; set; }
    public DbSet<Talhao> Talhoes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Propriedade>()
            .HasMany(p => p.Talhoes)
            .WithOne(t => t.Propriedade)
            .HasForeignKey(t => t.PropriedadeId);
    }
}