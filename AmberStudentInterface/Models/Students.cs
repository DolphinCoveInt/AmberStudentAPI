using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace AmberStudentInterface.Models
{
    public class StudentsVM2
    {

        public int Id { get; set; }
        public string StudentName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int ParishId { get; set; }
        public int CourseId { get; set; }
        public int ShirtSizeId { get; set; }

        public string? StudentIdImageFilePath { get; set; } = String.Empty;

        public Parishs Parish { get; set; }
        public Courses Course { get; set;}
        public ShirtSizes ShirtSize { get; set; }


    }
}
