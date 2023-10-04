using AmberStudentInterface.Models;
using Microsoft.EntityFrameworkCore;

namespace AmberStudentInterface.Data
{ 
    public class StudentDbContext : DbContext
    {
        public StudentDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<StudentsVM2> Student { get; set; }
        public DbSet<ShirtSizes> ShirtSize { get; set; }
        public DbSet<Parishs> Parish { get; set; }
        public DbSet<Courses> Course { get; set; }

    }
}
