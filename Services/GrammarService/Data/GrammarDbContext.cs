using GrammarService.Models;
using Microsoft.EntityFrameworkCore;
namespace GrammarService.Data;
public class GrammarDbContext : DbContext
{
    public GrammarDbContext(DbContextOptions<GrammarDbContext> options) : base(options) { }
    public DbSet<GrammarTopic> GrammarTopics => Set<GrammarTopic>();
    protected override void OnModelCreating(ModelBuilder modelBuilder) { base.OnModelCreating(modelBuilder); }
}
