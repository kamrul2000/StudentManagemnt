using StudentManagement.Models;
using StudentManagemnt.Models;

namespace StudentManagement.Interface
{
    public interface IStudentService
    {
        Task<List<Student>> GetAllStudentsAsync();
        Task<Student> GetStudentByIdAsync(int id);
        Task AddStudent(Student student);
        Task UpdateStudentAsync(Student student);
        Task DeleteStudentAsync(int id);
        Task AddBulkStudent(List<Student> students);


        Task<List<Student>> ImportFromExcelAsync(IFormFile file);

        Task<List<Student>> GetFilteredStudentsAsync(DateTime? fromDate, DateTime? toDate, string emailDomain);
        Task<byte[]>ExportToExcelAsync(List<Student> students);




    }
}
