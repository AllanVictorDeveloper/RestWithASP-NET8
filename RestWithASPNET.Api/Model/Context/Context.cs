using Microsoft.EntityFrameworkCore;

namespace RestWithASPNET.Api.Model.Context
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
        }

        public DbSet<Person> Persons { get; set; }
    }
}