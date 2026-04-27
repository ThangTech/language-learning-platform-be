using Microsoft.EntityFrameworkCore;
using VocabularyService.Models;
namespace VocabularyService.Data;
public class VocabularyDbContext : DbContext
{
    public VocabularyDbContext(DbContextOptions<VocabularyDbContext> options) : base(options) { }
    public DbSet<Word> Words => Set<Word>();
    protected override void OnModelCreating(ModelBuilder modelBuilder) { base.OnModelCreating(modelBuilder); }
}
