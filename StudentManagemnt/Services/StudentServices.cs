using Microsoft.EntityFrameworkCore;
using StudentManagement.Data;
using StudentManagement.Interface;
using StudentManagement.Models;

namespace StudentManagement.Services
{
    public class StudentServices : IStudentService
    {
        private readonly AppDbContext _context;

        public StudentServices(AppDbContext context)
        {
            _context = context;

        }

        public async Task<List<Student>> GetAllStudentsAsync()
        {

            return await _context.Students.ToListAsync();

        }
        public async Task<Student> GetStudentByIdAsync(int id)
        {

            return await _context.Students.FindAsync(id);

        }

        public async Task AddStudent(Student student)
        { 
             _context.Students.AddAsync(student);

            await _context.SaveChangesAsync();
        }

        public async Task AddBulkStudent(List<Student> students)
        {
            _context.Students.AddRangeAsync(students);

            await _context.SaveChangesAsync();
        }
        public async Task UpdateStudentAsync(Student student)
        {
            _context.Students.Update(student);
            await _context.SaveChangesAsync();

        }

        public async Task DeleteStudentAsync(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student != null)
            {
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();

            }
        }
    }
}