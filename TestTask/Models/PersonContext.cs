using Microsoft.EntityFrameworkCore;
using System.Security.Policy;

namespace TestTask.Models
{
    public class PersonContext : DbContext
    {
        public PersonContext(DbContextOptions<PersonContext> options)
            : base(options) { }

        public DbSet<Person> Persons { get; set; }
        public DbSet<Face> Faces { get; set; }
    }
}
