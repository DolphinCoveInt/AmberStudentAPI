using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace AmberStudentInterface.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }
        public string StudentName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int ParishId { get; set; }
        public int CourseId { get; set; }
        public int ShirtSizeId { get; set; }
        public string? StudentIdImageFilePath { get; set; } = String.Empty;



        //ForeignKeys
        [ForeignKey("ParishId")]
        public virtual Parish? Parish { get; set; }
        
        [ForeignKey("CourseId")]
        public virtual Course? Course { get; set; }

        [ForeignKey("ShirtSizeId")]
        public virtual ShirtSize? ShirtSize { get; set; }
    }
}
