using System.Configuration;
using Microsoft.EntityFrameworkCore;


namespace DogDatabase
{
    public class DbContextDog : DbContext
    {
        public DbSet<Dog> Dogs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)

             => options.UseSqlServer();
        //connectionString: ConfigurationManager.ConnectionStrings["DogContextDB"].ConnectionString
    }
}
