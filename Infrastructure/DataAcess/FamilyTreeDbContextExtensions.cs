using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Infrastructure.DataAcess
{
    public static class FamilyTreeDbContextExtensions
    {
        public static ModelBuilder ConfigureRelation(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Relation>()
                .HasOne(r => r.PersonFrom)
                .WithMany(r => r.OutgoingRelations)
                .HasForeignKey(r => r.PersonFromId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Relation>()
                .HasOne(r => r.PersonTo)
                .WithMany(r => r.IncomingRelations)
                .HasForeignKey(r => r.PersonToId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Relation>()
                .HasOne(r => r.RelationType)
                .WithMany(r => r.Relations)
                .HasForeignKey(r => r.RelationTypeId);

            return modelBuilder;
        }

        public static ModelBuilder ConfigureRelationType(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RelationType>()
                .Property(rt => rt.Id)
                .ValueGeneratedNever();

            var data = Enum.GetValues(typeof(RelationTypeEnum))
                .OfType<RelationTypeEnum>()
                .Select(rte => new RelationType() { Id = rte, Name = rte.ToString() })
                .ToArray();

            modelBuilder.Entity<RelationType>()
                .HasData(data);

            return modelBuilder;
        }

        public static ModelBuilder ConfigurePerson(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>()
                .Property(p => p.Name)
                .IsRequired();

            modelBuilder.Entity<Person>()
                .Property(p => p.Surname)
                .IsRequired();

            return modelBuilder;
        }
    }
}
