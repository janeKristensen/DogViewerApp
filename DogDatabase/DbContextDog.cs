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
                ,
                options => options.EnableRetryOnFailure(maxRetryCount:0));
        }
        //connectionString: ConfigurationManager.ConnectionStrings["DogContextDB"].ConnectionString
    }
}
