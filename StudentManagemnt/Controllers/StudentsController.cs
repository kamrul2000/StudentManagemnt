using Microsoft.AspNetCore.Mvc;
using StudentManagement.Interface;
using StudentManagement.Services;
using StudentManagement.Models;
using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Authorization;

namespace StudentManagement.Controllers
{
    [Authorize]
    public class StudentsController : Controller
    {
        private readonly IStudentService _studentService;
        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }

       public async Task<IActionResult> Index()
        {
            var students = await _studentService.GetAllStudentsAsync();
            return View(students);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Student student)
        {
            if (ModelState.IsValid)
            {
                await _studentService.AddStudent(student);
                return RedirectToAction("Index");
            }
            return View(student);
        }
        public IActionResult BulkInsert()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> BulkInsert(List<Student> students)
        {
            if (ModelState.IsValid)
            {
                await _studentService.AddBulkStudent(students);
                return RedirectToAction("Index");
            }
            return View(students);
        }

        [HttpGet]

        public async Task<IActionResult>Edit(int id)
        {
            var student = await _studentService.GetStudentByIdAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        [HttpPost]
        public async Task<IActionResult>Edit(Student student)
        {
            if (ModelState.IsValid)
            {
                await _studentService.UpdateStudentAsync(student);
                return RedirectToAction("Index");
            }
            return View(student);
        }
        public async Task<IActionResult> Delete(int id)
        {
            await _studentService.DeleteStudentAsync(id);
            return RedirectToAction("Index");
        }

        //csv file upload

        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Upload(IFormFile file) 
        
        {
        if(file == null || file.Length==0)
            {
                ModelState.AddModelError("", "Please upload a valid Excel/CSV file.");
                return View();
            }

            var students = await _studentService.ImportFromExcelAsync(file);
            if (!students.Any())
            {
                ModelState.AddModelError("", "No valid student records found in the file.");
                return View();
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Report(DateTime? fromDate, DateTime? toDate, string emailDomain)
        {
            var students = await _studentService.GetFilteredStudentsAsync(fromDate, toDate, emailDomain);
            ViewBag.FromDate = fromDate?.ToString("yyyy-MM-dd");
            ViewBag.ToDate = toDate?.ToString("yyyy-MM-dd");
            ViewBag.EmailDomain = emailDomain;
            return View(students);
        }
        public async Task<IActionResult> Export(DateTime? fromDate, DateTime? toDate, string emailDomain)
        {
            var students = await _studentService.GetFilteredStudentsAsync(fromDate, toDate, emailDomain);
            var fileContent = await _studentService.ExportToExcelAsync(students);
            var fileName = "StudentsReport.xlsx";
            return File(fileContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
    }
}
