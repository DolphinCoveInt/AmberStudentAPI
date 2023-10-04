using AmberStudentInterface.Data;
using Newtonsoft.Json;
using AmberStudentInterface.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


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
            var students = _ctx.Student
                .Include(s => s.ShirtSize)
                .Include(s => s.Course)
                .Include(s => s.Parish)
                .ToList();

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
        //[FromBody] tells the program to expects a product that's coming from the body of the request
        public IActionResult CreateStudent([FromBody] Student student)
        {
            _ctx.Student.Add(student);
            _ctx.SaveChanges();

            //Fetches the most recent created record and adds and Id then display the info
            return CreatedAtAction(nameof(GetStudentById), new { id = student.Id }, student);
        }

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
