using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using ACDataStorage.Models;
using System.Reflection.Emit;


namespace ACDataStorage
{
    class AppDbContext:DbContext
    {
        public AppDbContext() : base("DefaultConnection")
        { }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Client> Clients { get; set; }
 
    }
}
