using Microsoft.AspNetCore.Mvc;
using StudentManagement.Interface;
using StudentManagement.Services;
using StudentManagement.Models;
using System.Reflection.Metadata.Ecma335;

namespace StudentManagement.Controllers
{
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

    }
}
