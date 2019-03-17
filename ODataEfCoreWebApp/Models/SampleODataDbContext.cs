using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODataEfCoreWebApp.Models
{
    public class SampleODataDbContext : DbContext
    {
        public SampleODataDbContext(DbContextOptions options) : base(options)
        {
        }

        protected SampleODataDbContext()
        {
        }

        public DbSet<Person> Persons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>().HasData(
                new List<Person>
                {
                    new Person()
                    {
                        Id = 1,
                        Name = "Pye Dubois",
                        Age = 42
                    },
                    new Person()
                    {
                        Id = 2,
                        Name = "Max Webster",
                        Age = 23
                    },
                    new Person()
                    {
                        Id = 3,
                        Name = "Kim Mitchell",
                        Age = 26
                    },
                    new Person()
                    {
                        Id = 4,
                        Name = "Terry Watkinson",
                        Age = 26
                    },
                }
            );
        }
    }
}
