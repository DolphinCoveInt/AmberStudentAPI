using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AmberStudentInterface.Models
{
    public class ShirtSizes
    {
        public int Id { get; set; }

        [DisplayName("Size")]
        public string Name { get; set; }

        [DisplayName("Size Description")]
        public string Description { get; set; }
    }
}
