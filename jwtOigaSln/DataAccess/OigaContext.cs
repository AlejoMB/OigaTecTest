using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class OigaContext : DbContext
    {
        public OigaContext(DbContextOptions options) : base(options) 
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User() { Id = 1, FirstName = "Alejandro", LastName = "Moncada", UserName = "Alejo0921" },
                new User() { Id = 2, FirstName = "Carolina", LastName = "Gahona", UserName = "CGahona" },
                new User() { Id = 3, FirstName = "Santiago", LastName = "Castaño", UserName = "alejoS" },
                new User() { Id = 4, FirstName = "Alejo", LastName = "Monsalve", UserName = "Monsa13" },
                new User() { Id = 5, FirstName = "David", LastName = "Alejandro", UserName = "Cada123" },
                new User() { Id = 6, FirstName = "Alejo1", LastName = "absd", UserName = "absd" },
                new User() { Id = 7, FirstName = "Alejo2", LastName = "absd", UserName = "absd" },
                new User() { Id = 8, FirstName = "absd", LastName = "Alejo3", UserName = "absd" },
                new User() { Id = 9, FirstName = "absd", LastName = "absd", UserName = "Alejo4" },
                new User() { Id = 10, FirstName = "Alejo5", LastName = "absd", UserName = "absd" },
                new User() { Id = 11, FirstName = "absd", LastName = "Alejo6", UserName = "absd" },
                new User() { Id = 12, FirstName = "absd", LastName = "absd", UserName = "Alejo7" },
                new User() { Id = 13, FirstName = "Alejo8", LastName = "absd", UserName = "absd" },
                new User() { Id = 14, FirstName = "absd", LastName = "Alejo9", UserName = "absd" }
            );
        }

        public DbSet<User> User
        {
            get;
            set;
        }
    }
}
