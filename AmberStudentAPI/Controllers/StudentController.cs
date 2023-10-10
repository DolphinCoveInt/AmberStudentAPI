using AmberStudentInterface.Data;
using Newtonsoft.Json;
using AmberStudentInterface.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AmberStudentAPI.Models;

namespace AmberStudentInterface.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        //Create context connection to Database
        private readonly StudentDbContext _ctx;

        ////Constructor for Dependency Injection to connect to DB
        public StudentController(StudentDbContext context) => _ctx = context;

        //Student Endpoint
        //Get All Student
        [HttpGet]
        public IActionResult GetStudents()
        {
            var students = _ctx.Student.Include(s => s.ShirtSize).Include(s => s.Course).Include(s => s.Parish).ToList();
            if (students == null)
            {
                return BadRequest();
            }
            var baseUrl = "https://localhost:7141/images/";
            foreach (var student in students)
            {
                student.StudentIdImageFilePath = baseUrl + student.StudentIdImageFilePath;
            }

            return Ok(students);
        }

        [HttpGet("{id}")]
        public IActionResult GetStudentById(int id)
        {
            var student = _ctx.Student
                .Include(s => s.Parish)
                .Include(s => s.ShirtSize)
                .Include(s => s.Course)
                .FirstOrDefault(s => s.Id == id);

            if (student == null)
            {
                return NotFound();
            }

            var baseUrl = "https://localhost:7141/images/";
            student.StudentIdImageFilePath = baseUrl + student.StudentIdImageFilePath;

            return Ok(student);
        }


        //[HttpPost]
        //public async Task<ActionResult<Student>> PostStudent (Student student)
        //{
        //    if (_ctx.Student == null) 
        //    {
        //        return Problem("Entity Set 'StudentDbContext' is null");
        //    }
        //    _ctx.Student.Add(student);
        //    await _ctx.SaveChangesAsync();

        //    return Ok(student);

        //}


        [HttpPost]
        public async Task<IActionResult> Create([FromForm] StudentCreateDTO model)
        {
            if (ModelState.IsValid)
            {
                // Retrieve the file from the DTO
                var studentImageFile = model.StudentIdImageFile;

                if (studentImageFile != null && studentImageFile.Length > 0)
                {
                    // Generate a unique file name 

                    // 50137a25-920c-469a-b202-4ce1e2a4712c_person-02.jpg
                    var uniqueFileName = Guid.NewGuid() + "_" + studentImageFile.FileName;

                    // Define the final file path on the API server
                    var apiFilePath = Path.Combine("uploads", $"{uniqueFileName}");

                    // Save the file to the server
                    using (var stream = new FileStream(apiFilePath, FileMode.Create))
                    {
                        await studentImageFile.CopyToAsync(stream);
                    }

                    // Store the file path in the database along with other student details
                    var student = new Student
                    {
                        StudentName = model.StudentName,
                        CourseId = model.CourseId,
                        StudentIdImageFilePath = apiFilePath != String.Empty ? apiFilePath : "",
                        ShirtSizeId = model.ShirtSizeId,
                        ParishId = model.ParishId,
                        Email = model.Email,
                        PhoneNumber = model.PhoneNumber,

                    };

                    // Save the student to the database using your data access logic
                    _ctx.Student.Add(student);
                    await _ctx.SaveChangesAsync();

                    return CreatedAtAction(nameof(GetStudentById), new { id = student.Id }, student);
                    //return Ok(bill); // Return a JSON response, indicating success, or the newly created bill
                }
            }
            // If ModelState is not valid or the file is missing, return a validation error response
            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            // Find the student to delete by ID
            var student = await _ctx.Student.FindAsync(id);

            if (student == null)
            {
                return NotFound(); // Return a 404 Not Found response if the student is not found
            }

            // Remove the student from the database
            _ctx.Student.Remove(student);
            await _ctx.SaveChangesAsync();

            return NoContent(); // Return a 204 No Content response to indicate successful deletion
        }



        //Retrieve a link to the uploaded file
        [HttpGet("files/{fileName}")]
        public IActionResult GetFile(string fileName)
        {
            // Construct the full path to the file based on the provided 'fileName'
            string filePath = Path.Combine("api", "server", "uploads", fileName);

            // Check if the file exists
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound(); // Or handle the case where the file doesn't exist
            }
            // Determine the content type based on the file's extension
            string contentType = GetContentType(fileName);

            // Return the image file as a FileStreamResult with the appropriate content type
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            return new FileStreamResult(fileStream, contentType); // Adjust the content type as needed

        }
        private string GetContentType(string fileName)
        {
            // Determine the content type based on the file's extension
            string ext = Path.GetExtension(fileName).ToLowerInvariant();
            switch (ext)
            {
                case ".jpg":
                case ".jpeg":
                    return "image/jpeg";
                case ".png":
                    return "image/png";
                case ".pdf":
                    return "application/pdf";
                default:
                    return "application/octet-stream"; // Default to binary data
            }
        }



        //[HttpPost]
        //[FromBody] tells the program to expects a product that's coming from the body of the request
        //public IActionResult CreateStudent([FromBody] Student student)
        //{
        //    _ctx.Student.Add(student);
        //    _ctx.SaveChanges();

        //    Fetches the most recent created record and adds and Id then display the info
        //    return CreatedAtAction(nameof(GetStudentById), new { id = student.Id }, student);
        //}

        [HttpPut("{id}")]
        public IActionResult UpdateStudent(int id, [FromBody] Student student)
        {
            if (id != student.Id)
            {
                return BadRequest();
            }

            _ctx.Student.Update(student);
            _ctx.SaveChanges();

            return CreatedAtAction(nameof(GetStudentById), new { id = student.Id }, student);
        }



        #region Course Endpoint
        // Get all Course 
        [HttpGet]
        [Route("Course")]
        public IActionResult GetCourse()
        {
            var aCourse = _ctx.Course.ToList();
            return Ok(aCourse);
        }

        // Get a specific Course size by ID
        [HttpGet]
        [Route("Course/{id}")]
        public IActionResult GetCourseById(int id)
        {
            var aCourse = _ctx.Course.FirstOrDefault(x => x.Id == id);
            if (aCourse == null)
            {
                return NotFound();
            }

            return Ok(aCourse);
        }

        // Create a new Course size
        [HttpPost]
        [Route("Course")]
        public IActionResult CreateCourse([FromBody] Course item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            _ctx.Course.Add(item);
            _ctx.SaveChanges();

            return CreatedAtAction(nameof(GetCourseById), new { id = item.Id }, item);
        }

        // Update a Course Course by ID
        [HttpPut]
        [Route("Course/{id}")]
        public IActionResult UpdateCourse(int id, [FromBody] Course item)
        {
            if (item == null || id != item.Id)
            {
                return BadRequest();
            }

            var aCourse = _ctx.Course.FirstOrDefault(x => x.Id == id);
            if (aCourse == null)
            {
                return NotFound();
            }

            _ctx.Entry(aCourse).CurrentValues.SetValues(item);
            _ctx.SaveChanges();

            return Ok(item);
        }

        [HttpDelete("Course/{id}")]
        public IActionResult DeleteCourse(int id)
        {
            var course = _ctx.Course.FirstOrDefault(x => x.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            _ctx.Course.Remove(course);
            _ctx.SaveChanges();

            return NoContent();
        }


        #endregion


        #region ShirtSize Endpoint
        // Get all shirt sizes
        [HttpGet]
        [Route("ShirtSize")]
        public IActionResult GetShirtSize()
        {
            var shirtSizes = _ctx.ShirtSize.ToList();
            return Ok(shirtSizes);
        }

        // Get a specific shirt size by ID
        [HttpGet]
        [Route("ShirtSize/{id}")]
        public IActionResult GetShirtSizeById(int id)
        {
            var shirtSize = _ctx.ShirtSize.FirstOrDefault(x => x.Id == id);
            if (shirtSize == null)
            {
                return NotFound();
            }

            return Ok(shirtSize);
        }

        // Create a new shirt size
        [HttpPost]
        [Route("ShirtSize")]
        public IActionResult CreateShirtSize([FromBody] ShirtSize item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            _ctx.ShirtSize.Add(item);
            _ctx.SaveChanges();

            return CreatedAtAction(nameof(GetShirtSizeById), new { id = item.Id }, item);
        }

        // Update a shirt size by ID
        [HttpPut]
        [Route("ShirtSize/{id}")]
        public IActionResult UpdateShirtSize(int id, [FromBody] ShirtSize item)
        {
            if (item == null || id != item.Id)
            {
                return BadRequest();
            }

            var existingShirtSize = _ctx.ShirtSize.FirstOrDefault(x => x.Id == id);
            if (existingShirtSize == null)
            {
                return NotFound();
            }

            _ctx.Entry(existingShirtSize).CurrentValues.SetValues(item);
            _ctx.SaveChanges();

            return Ok(item);
        }
        [HttpDelete("ShirtSize/{id}")]
        public IActionResult DeleteShirtSize(int id)
        {
            var aShirtSize = _ctx.ShirtSize.FirstOrDefault(x => x.Id == id);
            if (aShirtSize == null)
            {
                return NotFound();
            }

            _ctx.ShirtSize.Remove(aShirtSize);
            _ctx.SaveChanges();

            return NoContent();
        }
        #endregion



        #region Parish Endpoint
        // Get all Parish sizes
        [HttpGet]
        [Route("Parish")]
        public IActionResult GetParish()
        {
            var aParish = _ctx.Parish.ToList();
            return Ok(aParish);
        }

        // Get a specific Parish size by ID
        [HttpGet]
        [Route("Parish/{id}")]
        public IActionResult GetParishById(int id)
        {
            var aParish = _ctx.Parish.FirstOrDefault(x => x.Id == id);
            if (aParish == null)
            {
                return NotFound();
            }

            return Ok(aParish);
        }

        // Create a new Parish size
        [HttpPost]
        [Route("Parish")]
        public IActionResult CreateParish([FromBody] Parish item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            _ctx.Parish.Add(item);
            _ctx.SaveChanges();

            return CreatedAtAction(nameof(GetParishById), new { id = item.Id }, item);
        }

        // Update a Parish size by ID
        [HttpPut]
        [Route("Parish/{id}")]
        public IActionResult UpdateParish(int id, [FromBody] Parish item)
        {
            if (item == null || id != item.Id)
            {
                return BadRequest();
            }

            var aParish = _ctx.Parish.FirstOrDefault(x => x.Id == id);
            if (aParish == null)
            {
                return NotFound();
            }

            _ctx.Entry(aParish).CurrentValues.SetValues(item);
            _ctx.SaveChanges();

            return Ok(item);
        }
        [HttpDelete("Parish/{id}")]
        public IActionResult DeleteParish(int id)
        {
            var aParish = _ctx.Parish.FirstOrDefault(x => x.Id == id);
            if (aParish == null)
            {
                return NotFound();
            }

            _ctx.Parish.Remove(aParish);
            _ctx.SaveChanges();

            return NoContent();
        }
        #endregion
        [HttpPost]
        [Route("upload")]
        public IActionResult Upload(IFormFile file)
        {
            return Ok();
        }
    }
}
