using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataAcess
{
    public class FamilyTreeDbContext : DbContext
    {
        public FamilyTreeDbContext(DbContextOptions<FamilyTreeDbContext> options) : base(options)
        {
        }

        public DbSet<Person> People { get; set; }
        public DbSet<RelationType> RelationTypes { get; set; }
        public DbSet<Relation> Relations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .ConfigurePerson()
                .ConfigureRelation()
                .ConfigureRelationType();
        }
    }
}
