using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Storage;


namespace DogDatabase
{
    public class DbContextDog : DbContext
    {
        public DbSet<Dog> Dogs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                connectionString: ConfigurationManager.ConnectionStrings["DogContextDB"].ConnectionString,
                options => options.EnableRetryOnFailure(maxRetryCount:0));
        }
        //
    }
}
