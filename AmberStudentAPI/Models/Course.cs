using System.ComponentModel.DataAnnotations;

namespace AmberStudentInterface.Models
{
    public class Course
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
