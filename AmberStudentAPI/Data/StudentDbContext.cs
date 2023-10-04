using AmberStudentInterface.Models;
using Microsoft.EntityFrameworkCore;

namespace AmberStudentInterface.Data
{
    public class StudentDbContext : DbContext
    {
        public StudentDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Student> Student { get; set; }
        public DbSet<ShirtSize> ShirtSize { get; set; }
        public DbSet<Parish> Parish { get; set; }
        internal DbSet<Course> Course { get; set; }

    }
}
