using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CourseManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;

namespace CourseManagementSystem.Controllers
{
    public class StudentsController : Controller
    {
        private readonly CourseManagementDBContext _context;

        public StudentsController(CourseManagementDBContext context)
        {
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> Index()
        {
            var courseManagementDBContext = _context.Students.Include(s => s.DeptIdFkNavigation);
            return View(await courseManagementDBContext.ToListAsync());
        }

        public async Task<IActionResult> SearchStudent(string search)
        {
            var students = _context.Students.Include(c => c.DeptIdFkNavigation).Where(t => (t.FirstName.ToLower() + " " +t.LastName.ToLower()).Contains(search.ToLower()));
            return View(nameof(Index), await students.ToListAsync());
        }
        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.DeptIdFkNavigation)
                .Include(s => s.Takes)
                    .ThenInclude(s => s.CourseIdFkNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            var courses = student.Takes.Select(t => t.CourseIdFkNavigation).ToList();
            var totalAkts = 0;
            foreach (Course course in courses) {
                totalAkts += course.Credit;
            }
            ViewBag.totalAkts = totalAkts;
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }
        
        public IActionResult TakeCourse(int id) {
            var student = _context.Students.FirstOrDefault(t => t.Id == id);
            ViewBag.student = student;
            return RedirectToAction("Create", "Takes");
        }
        
        // GET: Students/Create
        public IActionResult Create()
        {
            ViewData["DeptIdFk"] = new SelectList(_context.Departments, "Id", "DeptName");
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,DeptIdFk")] Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DeptIdFk"] = new SelectList(_context.Departments, "Id", "DeptName", student.DeptIdFk);
            return View(student);
        }
        
        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            ViewData["DeptIdFk"] = new SelectList(_context.Departments, "Id", "DeptName", student.DeptIdFk);
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,DeptIdFk")] Student student)
        {
            if (id != student.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["DeptIdFk"] = new SelectList(_context.Departments, "Id", "DeptName", student.DeptIdFk);
            return View(student);
        }

        // GET: Students/Delete/5
        
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.DeptIdFkNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);
            _context.Entry(student).State = EntityState.Deleted;
            int changes = await _context.SaveChangesAsync();
            TempData["deleteResult"] = changes >= 0 ? "Student Is Successfully Deleted!" : "Student Cannot Be Deleted";
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.Id == id);
        }
    }
}
