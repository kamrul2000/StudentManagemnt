using StudentManagemnt.Models;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Models;


namespace StudentManagement.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext>options) : base(options) 
        
        {               
        }
        public DbSet<Student> Students { get; set; }
    }
}
