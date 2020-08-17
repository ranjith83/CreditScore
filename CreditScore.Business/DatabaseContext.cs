using System;
using CreditScore.Models;
using Microsoft.EntityFrameworkCore;

namespace CreditScore.Business
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            
        }

        public DbSet<User> User { get; set; }

        public DbSet<Company> Company { get; set; }
        
        public DbSet<Customer> Customer { get; set; }
       
    }
}
