using System.Configuration;
using Microsoft.EntityFrameworkCore;


namespace DogDatabase
{
    public class DbContextDog : DbContext
    {
        public DbSet<Dog> Dogs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)

             => options.UseSqlServer(connectionString: ConfigurationManager.ConnectionStrings["DogContextDB"].ConnectionString);
             

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Dog>()
                .Property(p => p.BreedName)
                .IsRequired();
            modelBuilder.Entity<Dog>()
                .Property(p => p.SubBreed);
            modelBuilder.Entity<Dog>()
                .Property(p => p.Id)
                .UseIdentityColumn();   
        }

    }
}
