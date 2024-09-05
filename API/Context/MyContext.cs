using API.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace API.Context

{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options)
        {}
        public DbSet<Department> Departments {get; set;}
        public DbSet<Roles> Roles { get; set; }
        
    }
}

