using System.Collections.Generic;
using StudentDb.Models;

namespace StudentDb.Repository
{
    public interface IStudentRepository
    {
        IEnumerable<Student> GetAll();
        Student GetById(int id);
        void Add(Student student);
        void Update(Student student);
        void Delete(int id);
        bool IsEmailExists(string email, int? id = null);
        bool IsMobileExists(string mobileNumber, int? id = null);
    }
}
