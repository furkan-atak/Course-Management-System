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
    public class CoursesController : Controller
    {
        private readonly CourseManagementDBContext _context;


        public CoursesController(CourseManagementDBContext context)
        {
            _context = context;
        }

        // GET: Courses
        public async Task<IActionResult> Index()
        {
            var courseManagementDBContext = _context.Courses.Include(c => c.DeptIdFkNavigation);
            return View(await courseManagementDBContext.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> SearchCourse(string search)
        {
            var courseManagementDBContext = _context.Courses.Include(c => c.DeptIdFkNavigation).Where(t => t.Name.ToLower().Contains(search.ToLower()));
            return View(nameof(Index), await courseManagementDBContext.ToListAsync());
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var course = await _context.Courses
                .Include(c => c.DeptIdFkNavigation)
                .Include(c => c.Takes)
                    .ThenInclude(c => c.StudentIdFkNavigation)
                .Include(c => c.Teaches)
                    .ThenInclude(c => c.TeacherIdFkNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // GET: Courses/Create
        
        public IActionResult Create()
        {

            ViewData["DeptIdFk"] = new SelectList(_context.Departments, "Id", "DeptName");
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,DeptIdFk,Date,Credit,Place")] Course course)
        {
            
            
            if (ModelState.IsValid)
            {
                var courseTime = course.Date;
                if (_context.Courses.Any(t => t.Place == course.Place && t.Date == courseTime))
                {
                    ViewBag.Message = "A Course With The Same Date And Place Exists!";
                    return View();
                }
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            ViewData["DeptIdFk"] = new SelectList(_context.Departments, "Id", "DeptName", course.DeptIdFk);
            return View(course);
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            ViewData["DeptIdFk"] = new SelectList(_context.Departments, "Id", "DeptName", course.DeptIdFk);
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,DeptIdFk,Date,Credit,Place")] Course course)
        {
            if (id != course.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.Id))
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
            ViewData["DeptIdFk"] = new SelectList(_context.Departments, "Id", "DeptName", course.DeptIdFk);
            return View(course);
        }
        
        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .Include(c => c.DeptIdFkNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.Id == id);
        }
    }
}
