using Microsoft.AspNetCore.Mvc;
using StudentDb.Models;
using StudentDb.Repository;
using System;
using System.Collections.Generic;

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
    }
}
