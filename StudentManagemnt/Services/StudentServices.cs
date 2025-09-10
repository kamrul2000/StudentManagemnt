using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
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

        [Obsolete]
        public async Task<List<Student>> ImportFromExcelAsync(IFormFile file)
        {
            var students = new List<Student>();
            if (file != null && file.Length > 0)
            {
                using var stream = new MemoryStream();
                await file.CopyToAsync(stream);
                stream.Position = 0;
                using var package = new ExcelPackage(stream);
                var worksheet = package.Workbook.Worksheets[0];
                int rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++)
                {
                    if (worksheet.Cells[row, 1].Value == null)
                        continue;

                    var student = new Student
                    {
                        Name = worksheet.Cells[row, 1].Value.ToString(),
                        Email = worksheet.Cells[row, 2].Value.ToString(),
                        EnrollmentDate = DateTime.Parse(worksheet.Cells[row, 3].Value.ToString())

                    };
                    students.Add(student);


                }
            }

            if (students.Any())
            {
                await AddBulkStudent(students);
            }
            return students;
        }

        public async Task<List<Student>> GetFilteredStudentsAsync(DateTime? fromDate, DateTime? toDate, string emailDomain)
        {
            var query = _context.Students.AsQueryable();

            if (fromDate.HasValue)
                query = query.Where(s => s.EnrollmentDate >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(s => s.EnrollmentDate <= toDate.Value);

            if (!string.IsNullOrEmpty(emailDomain))
            {
                emailDomain = emailDomain.Trim();
                query = query.Where(s => s.Email.ToLower().EndsWith(emailDomain.ToLower()));
            }

            return await query.ToListAsync();
        }

        public async Task<byte[]> ExportToExcelAsync(List<Student> students)
        {
            using var package = new ExcelPackage();
            var ws = package.Workbook.Worksheets.Add("Students");
            ws.Cells[1, 1].Value = "Name";
            ws.Cells[1, 2].Value = "Email";
            ws.Cells[1, 3].Value = "Enrollment Date";
            int row = 2;
            foreach (var s in students)
            {
                ws.Cells[row, 1].Value = s.Name;
                ws.Cells[row, 2].Value = s.Email;
                ws.Cells[row, 3].Value = s.EnrollmentDate.ToShortDateString();
                row++;
            }
            return await package.GetAsByteArrayAsync();
        }

    }
}