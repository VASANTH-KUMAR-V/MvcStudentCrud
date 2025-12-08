using Microsoft.AspNetCore.Mvc;
using StudentDb.Models;
using StudentDb.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MvcStudentCrud.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentRepository _repo;

        public StudentController(IStudentRepository repo)
        {
            _repo = repo;
        }

        // GET: /Student
        public IActionResult Index()
        {
            var students = _repo.GetAll();
            return View(students);
        }

        // GET: /Student/Create
        public IActionResult Create()
        {
            ViewBag.States = GetStates();
            return View();
        }

        // POST: /Student/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Student student)
        {
            ViewBag.States = GetStates();

            if (!ModelState.IsValid)
                return View(student);

            // Duplicate check
            if (_repo.IsEmailExists(student.Email))
            {
                ModelState.AddModelError("Email", "This email is already used.");
                return View(student);
            }

            if (_repo.IsMobileExists(student.MobileNumber))
            {
                ModelState.AddModelError("MobileNumber", "This mobile number is already used.");
                return View(student);
            }

            try
            {
                _repo.Add(student);
                TempData["SuccessMessage"] = "Student added successfully!";

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error while adding student: " + ex.Message);
                return View(student);
            }
        }

        // GET: /Student/Edit/5
        public IActionResult Edit(int id)
        {
            var student = _repo.GetById(id);
            if (student == null)
                return NotFound();

            ViewBag.States = GetStates();
            return View(student);
        }

        // POST: /Student/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Student student)
        {
            ViewBag.States = GetStates();

            if (!ModelState.IsValid)
                return View(student);

            // Duplicate check (exclude current record)
            if (_repo.IsEmailExists(student.Email, student.Id))
            {
                ModelState.AddModelError("Email", "This email is already used.");
                return View(student);
            }

            if (_repo.IsMobileExists(student.MobileNumber, student.Id))
            {
                ModelState.AddModelError("MobileNumber", "This mobile number is already used.");
                return View(student);
            }

            try
            {
                _repo.Update(student);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error while updating student: " + ex.Message);
                return View(student);
            }
        }

        // GET: /Student/ConfirmDelete/5
        public IActionResult ConfirmDelete(int id)
        {
            var student = _repo.GetById(id);
            if (student == null)
                return NotFound();

            return View(student);
        }

        // POST: /Student/DeleteConfirmed/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                _repo.Delete(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error deleting student: " + ex.Message;
                return RedirectToAction("Index");
            }
        }
        // GET: /Student/Details/5
        public IActionResult Details(int id)
        {
            var student = _repo.GetById(id);
            if (student == null)
                return NotFound();

            return View(student);
        }

        // Method for State dropdown
        private List<string> GetStates()
        {
            return new List<string> { "Karnataka", "Kerala", "Tamil Nadu", "Maharashtra",    };
        }



        // Dashboard Model
        // GET Dashboard
        
        public IActionResult Dashboard()
        {
            // Get all students
            var students = _repo.GetAll();

            // Calculate counts
            ViewBag.TotalStudents = students.Count();
            ViewBag.ActiveStudents = students.Count(s => s.Status);    // Status = true
            ViewBag.InactiveStudents = students.Count(s => !s.Status); // Status = false

            return View();
        }
        public IActionResult DashboardCounts()
        {
            var students = _repo.GetAll();

            ViewBag.TotalStudents = students.Count();
            ViewBag.ActiveStudents = students.Count(s => s.Status);
            ViewBag.InactiveStudents = students.Count(s => !s.Status);

            return PartialView("_DashboardCounts");
        }


        // Load List in Dashboard (AJAX)
        public IActionResult LoadStudentList()
        {
            var students = _repo.GetAll();
            return PartialView("_StudentList", students);
        }

        // GET: Add/Edit Popup
        [HttpGet]
        public IActionResult SavePopup(int? id)
        {
            ViewBag.States = GetStates();

            if (id == null) // Add
                return PartialView("_Save", new Student());

            // Edit
            var student = _repo.GetById(id.Value);
            if (student == null)
                return NotFound();

            return PartialView("_Save", student);
        }

        // POST: Add/Edit
        [HttpPost]
        public IActionResult SavePopup(Student student)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.States = GetStates();
                return PartialView("_Save", student);
            }

            string message;

            if (student.Id == 0)
            {
                _repo.Add(student);
                message = "Student has been added successfully!";
            }
            else
            {
                _repo.Update(student);
                message = "Student has been updated successfully!";
            }

            return Json(new { success = true, message });
        }


        // Popup Details
        public IActionResult DetailsPopup(int id)
        {
            return PartialView("_Details", _repo.GetById(id));
        }

        // Popup Delete Confirmation
        public IActionResult DeletePopup(int id)
        {
            return PartialView("_Delete", _repo.GetById(id));
        }

        [HttpPost]
        public IActionResult DeleteConfirmedPopup(int id)
        {
            _repo.Delete(id);
            return Json(new { success = true });
        }

        


    }
}
