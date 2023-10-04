using Microsoft.AspNetCore.Mvc.Rendering;
using AmberStudentInterface.Models;
using System.ComponentModel;
using AmberStudentAPI.Models;
using AmberStudentInterface.Data;
using AmberStudentInterface.Controllers;

namespace AmberStudentInterface.Models.ViewModel
{
    public class StudentVM2
    {
        public int Id { get; set; }
        public string StudentName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public IFormFile? StudentIdImageFile { get; set; }

        //Foreign Keys
        public int CourseId { get; set; }
        public int ShirtSizeId { get; set; }
        public int ParishId { get; set; }

        //Selected Values
        public List<SelectListItem> CourseSelectList { get; set; }
        public List<SelectListItem> ShirtSizeSelectList { get; set; }
        public List<SelectListItem> ParishSelectList { get; set; }

        public int SelectedCourseId { get; set; }
        public int SelectedShirtSizeId { get; set; }
        public int SelectedParishId { get; set; }
    }
}
