namespace AmberStudentInterface.Models

{
    public class StudentCreateDTO
    {
        public string StudentName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int ParishId { get; set; }
        public int CourseId { get; set; }
        public int ShirtSizeId { get; set; }
        public IFormFile? StudentIdImageFile { get; set; }
    }
}

