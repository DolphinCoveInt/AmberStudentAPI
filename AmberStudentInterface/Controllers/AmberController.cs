using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using AmberStudentInterface.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http;
using AmberStudentInterface.Models.ViewModel;
using AmberStudentAPI.Models;
using StudentCreateDTO = AmberStudentInterface.Models.StudentCreateDTO;
using System.Net.Http.Headers;
using Microsoft.EntityFrameworkCore;

namespace AmberStudentInterface.Controllers
{
    public class AmberController : Controller
    {
        //const string BASE_URL = "";
        //const string COURSE_ENDPOINT = "Course";
        //const string SHIRTSIZE_ENDPOINT = "ShirtSize";
        //const string PARISH_ENDPOINT = "Parish";
        Uri baseAddress = new Uri("https://localhost:7141/api/Student/");
        const string COURSE_ENDPOINT = "Course";
        const string SHIRTSIZE_ENDPOINT = "ShirtSize";
        const string PARISH_ENDPOINT = "Parish";

        public IActionResult Index()
        {
            var studentList = new List<StudentsVM2>();
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = baseAddress;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync(baseAddress).Result;
                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    studentList = JsonConvert.DeserializeObject<List<StudentsVM2>>(data);
                }
            }
            return View(studentList);
        }


        [HttpGet]
        //[Route("/Student/Detail/{id}")]
        public IActionResult Detail(int id)
        {
            //Implement Logic to Link
            StudentsVM2 student = new StudentsVM2();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{baseAddress}{id}");

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync($"{baseAddress}{id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    student = JsonConvert.DeserializeObject<StudentsVM2>(data)!;
                }
            }
            return View(student);

        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            List<Courses> corList = new List<Courses>();
            List<ShirtSizes> sizeList = new List<ShirtSizes>();
            List<Parishs> parishList = new List<Parishs>();
            StudentsVM2 student = new StudentsVM2();//Global variable to the view function

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{baseAddress}");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage corResponse = client.GetAsync($"{baseAddress}{COURSE_ENDPOINT}").Result;
                if (corResponse.IsSuccessStatusCode)
                {
                    var corData = corResponse.Content.ReadAsStringAsync().Result;
                    corList = JsonConvert.DeserializeObject<List<Courses>>(corData)!;
                }
                //
                HttpResponseMessage szResponse = client.GetAsync($"{baseAddress}{SHIRTSIZE_ENDPOINT}").Result;
                if (szResponse.IsSuccessStatusCode)
                {
                    var sizeData = szResponse.Content.ReadAsStringAsync().Result;
                    //productList = JsonConvert.DeserializeObject<List<Products>>(data);
                    sizeList = JsonConvert.DeserializeObject<List<ShirtSizes>>(sizeData)!;
                }
                //
                HttpResponseMessage psResponse = client.GetAsync($"{baseAddress}{PARISH_ENDPOINT}").Result;
                if (szResponse.IsSuccessStatusCode)
                {
                    var parishData = psResponse.Content.ReadAsStringAsync().Result;
                    parishList = JsonConvert.DeserializeObject<List<Parishs>>(parishData)!;
                }
            }
            var viewModel = new StudentVM2
            {
                Id = student.Id,
                StudentName = student.StudentName,
                ShirtSizeId = student.ShirtSizeId,
                CourseId = student.CourseId,
                ParishId = student.ParishId,
                PhoneNumber = student.PhoneNumber,
                Email = student.Email,

                CourseSelectList = corList.Select(cor => new SelectListItem
                {
                    Text = cor.Name,
                    Value = cor.Id.ToString()
                }).ToList(),

                ShirtSizeSelectList = sizeList.Select(size => new SelectListItem
                {
                    Text = size.Name,
                    Value = size.Id.ToString()
                }).ToList(),

                ParishSelectList = parishList.Select(par => new SelectListItem
                {
                    Text = par.ParishName,
                    Value = par.Id.ToString()
                }).ToList()

            };
            return View(viewModel);
        }



        [HttpPost]
        public IActionResult Edit(int id, StudentVM2 studentVM2)
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri($"{baseAddress}");
            client.DefaultRequestHeaders.Accept.Clear();

            var stud = new StudentsVM2
            {
                Id = id,
                StudentName = studentVM2.StudentName,
                ShirtSizeId = studentVM2.SelectedShirtSizeId,
                CourseId = studentVM2.SelectedCourseId,
                ParishId = studentVM2.SelectedParishId,
                PhoneNumber = studentVM2.PhoneNumber,
                Email = studentVM2.Email,

            };
            var json = JsonConvert.SerializeObject(stud);
            var data = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage studResponse = client.PutAsync($"{baseAddress}{id}", data).Result;
            if (studResponse.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Unable to update Product");
                return View(studentVM2);
            }
        }



        //CREATIMG FUNCTION
        [HttpGet]
        public IActionResult Create()
        {
            List<Courses> corList = new List<Courses>();
            List<ShirtSizes> sizeList = new List<ShirtSizes>();
            List<Parishs> parishList = new List<Parishs>();
            //Student student = new Student();//Global variable to the view function

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{baseAddress}");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage corResponse = client.GetAsync($"{baseAddress}{COURSE_ENDPOINT}").Result;
                if (corResponse.IsSuccessStatusCode)
                {
                    var corData = corResponse.Content.ReadAsStringAsync().Result;
                    corList = JsonConvert.DeserializeObject<List<Courses>>(corData)!;
                }
                //
                HttpResponseMessage szResponse = client.GetAsync($"{baseAddress}{SHIRTSIZE_ENDPOINT}").Result;
                if (szResponse.IsSuccessStatusCode)
                {
                    var sizeData = szResponse.Content.ReadAsStringAsync().Result;
                    //productList = JsonConvert.DeserializeObject<List<Products>>(data);
                    sizeList = JsonConvert.DeserializeObject<List<ShirtSizes>>(sizeData)!;
                }
                //
                HttpResponseMessage psResponse = client.GetAsync($"{baseAddress}{PARISH_ENDPOINT}").Result;
                if (szResponse.IsSuccessStatusCode)
                {
                    var parishData = psResponse.Content.ReadAsStringAsync().Result;
                    parishList = JsonConvert.DeserializeObject<List<Parishs>>(parishData)!;
                }

            }
            var viewModel = new StudentVM2
            {

                CourseSelectList = corList.Select(cor => new SelectListItem
                {
                    Text = cor.Name,
                    Value = cor.Id.ToString()
                }).ToList(),

                ShirtSizeSelectList = sizeList.Select(size => new SelectListItem
                {
                    Text = size.Name,
                    Value = size.Id.ToString()
                }).ToList(),

                ParishSelectList = parishList.Select(parish => new SelectListItem
                {
                    Text = parish.ParishName,
                    Value = parish.Id.ToString()
                }).ToList()
            };
            return View(viewModel);
        }




        //POSTING CREATION FUNCTION
        [HttpPost]

        public async Task<IActionResult> Create(StudentVM2 studentVM2)
        {
            if (ModelState.IsValid) return View(studentVM2);

            var student = new StudentCreateDTO
            {
                StudentName = studentVM2.StudentName,
                ShirtSizeId = studentVM2.SelectedShirtSizeId,
                CourseId = studentVM2.SelectedCourseId,
                ParishId = studentVM2.SelectedParishId,
                PhoneNumber = studentVM2.PhoneNumber,
                Email = studentVM2.Email,
                StudentIdImageFile = studentVM2.StudentIdImageFile,
            };
            var response = await SendStudentAPI(student);
            {
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Bill creation failed");
                    return View(student);
                }
            }//End Create Method
        }

        private async Task<HttpResponseMessage> SendStudentAPI(StudentCreateDTO studentDto)
        {
            using (var httpClient = new HttpClient())
            {
                using (var formData = new MultipartFormDataContent())
                {
                    // Set the Content-Type header to "multipart/form-data"
                    formData.Headers.ContentType.MediaType = "multipart/form-data";

                    // Add bill data to the request
                    formData.Add(new StringContent(studentDto.StudentName), "StudentName");
                    formData.Add(new StringContent(studentDto.CourseId.ToString()), "Course");
                    formData.Add(new StringContent(studentDto.ShirtSizeId.ToString()), "ShirtSize");
                    formData.Add(new StringContent(studentDto.ParishId.ToString()), "Parish");
                    formData.Add(new StringContent(studentDto.PhoneNumber.ToString()), "PhoneNumber");
                    formData.Add(new StringContent(studentDto.Email.ToString()), "Email");

                    // Add the file to the request
                    if (studentDto.StudentIdImageFile != null && studentDto.StudentIdImageFile.Length > 0)
                    {
                        formData.Add(new StreamContent(studentDto.StudentIdImageFile.OpenReadStream())
                        {
                            Headers = { ContentLength = studentDto.StudentIdImageFile.Length,
                                ContentType = new MediaTypeHeaderValue(
                                    studentDto.StudentIdImageFile.ContentType)
                            }
                            //Headers = { ContentLength = billDto.CustomerIdImageFile.Length }
                        }, "StudentIdImageFile", studentDto.StudentIdImageFile.FileName);
                    }

                    //Send to API Code
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("multipart/form-data"));

                    // Send the data to the API
                    return await httpClient.PostAsync(baseAddress, formData);
                }

            }
        }







                //[HttpPost]

                //public IActionResult Create(int id, StudentVM2 studentVM2)
                //{
                //    HttpClient client = new HttpClient();
                //    client.BaseAddress = new Uri($"{baseAddress}");
                //    client.DefaultRequestHeaders.Accept.Clear();

                //    var stud = new StudentsVM2
                //    {
                //        Id = id,
                //        StudentName = studentVM2.StudentName,
                //        ShirtSizeId = studentVM2.SelectedShirtSizeId,
                //        CourseId = studentVM2.SelectedCourseId,
                //        ParishId = studentVM2.SelectedParishId,
                //        PhoneNumber = studentVM2.PhoneNumber,
                //        Email = studentVM2.Email, 
                //    };

                //    var json = JsonConvert.SerializeObject(stud);
                //    var data = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                //    HttpResponseMessage studResponse = client.PostAsync($"{baseAddress}", data).Result;
                //    if (studResponse.IsSuccessStatusCode)
                //    {
                //        return RedirectToAction("Index");
                //    }
                //    else
                //    {
                //        ModelState.AddModelError(string.Empty, "UNABLE TO CREATE STUDENT");
                //    }
                //    return View(studentVM2);
                //}









            


            }
        } 


