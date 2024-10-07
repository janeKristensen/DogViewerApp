


using Microsoft.EntityFrameworkCore;

namespace DogDatabase
{
    internal class DbContextDog : DbContext
    {
        public DbContextDog() 
        {
            
        }

        public DbSet<Dog> Dogs { get; set; }
    }
}
