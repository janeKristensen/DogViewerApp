using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


namespace DogDatabase
{
    public class DbContextDog : DbContext
    {
        public DbSet<Dog> Dogs { get; set; }
        private readonly IConfiguration _configuration;
        
        public DbContextDog(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_configuration != null)
            {
                optionsBuilder.UseSqlServer(
                    _configuration.GetConnectionString("DogContextDB"),
                    options => options.EnableRetryOnFailure(maxRetryCount: 0));
            }
        }
    }
}
