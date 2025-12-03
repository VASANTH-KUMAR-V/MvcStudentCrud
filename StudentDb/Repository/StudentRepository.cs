using Dapper;
using StudentDb.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace StudentDb.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private readonly IDbConnection _db;

        public StudentRepository(IDbConnection db)
        {
            _db = db;
        }

        public IEnumerable<Student> GetAll()
        {
            return _db.Query<Student>("sp_GetAllStudents", commandType: CommandType.StoredProcedure);
        }

        public Student GetById(int id)
        {
            return _db.Query<Student>(
                "sp_GetStudentById",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            ).FirstOrDefault();
        }

        public void Add(Student student)
        {
            _db.Execute(
                "sp_AddStudent",
                new
                {
                    student.Name,
                    student.Email,
                    student.MobileNumber,
                    student.State,
                    student.Gender,
                    student.Dob
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public void Update(Student student)
        {
            _db.Execute(
                "sp_UpdateStudent",
                new
                {
                    student.Id,
                    student.Name,
                    student.Email,
                    student.MobileNumber,
                    student.State,
                    student.Gender,
                    student.Dob
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public void Delete(int id)
        {
            _db.Execute(
                "sp_DeleteStudent",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );
        }

        public bool IsEmailExists(string email, int? id = null)
        {
            return _db.ExecuteScalar<int>(
                "sp_IsEmailExists",
                new { Email = email, Id = id },
                commandType: CommandType.StoredProcedure
            ) > 0;
        }

        public bool IsMobileExists(string mobile, int? id = null)
        {
            return _db.ExecuteScalar<int>(
                "sp_IsMobileExists",
                new { MobileNumber = mobile, Id = id },
                commandType: CommandType.StoredProcedure
            ) > 0;
        }
    }
}
