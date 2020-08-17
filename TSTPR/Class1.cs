using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSTPR
{
    public class DatabaseContext1 : Microsoft.EntityFrameworkCore.DbContext
    {
        public DatabaseContext1(Microsoft.EntityFrameworkCore.DbContextOptions<DatabaseContext1> options) : base(options)
        {

        }

        public Microsoft.EntityFrameworkCore.DbSet<User> Users { get; set; }
        //public DbSet<UsersInRoles> UsersInRoles { get; set; }
        //public DbSet<PaymentDetails> PaymentDetails { get; set; }
    }
}
