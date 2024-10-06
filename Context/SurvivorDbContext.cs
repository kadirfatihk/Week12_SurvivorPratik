using Microsoft.EntityFrameworkCore;
using Week12_SurvivorPratik.Entities;

namespace Week12_SurvivorPratik.Context
{
    public class SurvivorDbContext : DbContext
    {
        // BİRİNCİ BÜYÜK
        public SurvivorDbContext(DbContextOptions<SurvivorDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CategoryEntity>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<CompetitorEntity>().HasQueryFilter(x => !x.IsDeleted);

            base.OnModelCreating(modelBuilder);
        }

       public DbSet<CompetitorEntity> Competitors => Set<CompetitorEntity>();
       public DbSet<CategoryEntity> Categories => Set<CategoryEntity>();
    }
}
