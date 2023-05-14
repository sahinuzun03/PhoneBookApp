using Microsoft.EntityFrameworkCore;
using PersonService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonService.DataAccess.Context
{
    public class PersonDbContext : DbContext
    {
        public PersonDbContext(DbContextOptions<PersonDbContext> options) : base(options)
        {
            
        }

        public DbSet<ContactInfo> ContactInfos { get; set; }
        public DbSet<Person> People { get; set; }
    }
}
