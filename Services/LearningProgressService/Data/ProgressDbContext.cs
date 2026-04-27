using LearningProgressService.Models;
using Microsoft.EntityFrameworkCore;
namespace LearningProgressService.Data;
public class ProgressDbContext : DbContext
{
    public ProgressDbContext(DbContextOptions<ProgressDbContext> options) : base(options) { }
    public DbSet<UserProgress> UserProgresses => Set<UserProgress>();
    protected override void OnModelCreating(ModelBuilder modelBuilder) { base.OnModelCreating(modelBuilder); }
}
