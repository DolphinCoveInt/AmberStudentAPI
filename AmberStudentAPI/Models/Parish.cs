using System.ComponentModel.DataAnnotations;

namespace AmberStudentInterface.Models
{
    public class Parish
    {
        [Key]
        public int Id { get; set; }
       
        public string ParishName { get; set; }
    }
}
