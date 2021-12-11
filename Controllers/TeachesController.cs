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
    public class TeachesController : Controller
    {
        private readonly CourseManagementDBContext _context;

        public TeachesController(CourseManagementDBContext context)
        {
            _context = context;
        }

        // GET: Teaches
        public async Task<IActionResult> Index()
        {
            var courseManagementDBContext = _context.Teaches.Include(t => t.CourseIdFkkNavigation).Include(t => t.TeacherIdFkNavigation);
            return View(await courseManagementDBContext.ToListAsync());
        }

        // GET: Teaches/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teach = await _context.Teaches
                .Include(t => t.CourseIdFkkNavigation)
                .Include(t => t.TeacherIdFkNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teach == null)
            {
                return NotFound();
            }

            return View(teach);
        }

        // GET: Teaches/Create
        
        public IActionResult Create()
        {
            ViewData["CourseIdFkk"] = new SelectList(_context.Courses, "Id", "Name");
            ViewData["TeacherIdFk"] = new SelectList(_context.Teachers, "Id", "Name");
            return View();
        }

        
        public IActionResult Create1(int id)
        {
            var teacher = _context.Teachers.FirstOrDefault(t => t.Id == id);
            var list = new List<Teacher>() { teacher };
            ViewData["CourseIdFkk"] = new SelectList(_context.Courses.Where(t => t.DeptIdFk == teacher.DeptId), "Id", "Name");
            ViewData["TeacherIdFk"] = new SelectList(list, "Id", "Name");
            return View(nameof(Create));
        }

        // POST: Teaches/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CourseIdFkk,TeacherIdFk,Semester,Year")] Teach teach)
        {
            
            
            if (ModelState.IsValid)
            {
                var coursesInSemester = _context.Teaches
                                        .Include(t => t.TeacherIdFkNavigation)
                                        .Select(t => t.TeacherIdFkNavigation.Teaches
                                        .Where(t => t.Semester == teach.Semester && t.Year == teach.Year));
                if (_context.Teaches.Any(t =>  t.CourseIdFkk == teach.CourseIdFkk && t.Semester == teach.Semester && t.Year == teach.Year ))
                {
                    ViewBag.Message = "A Teacher Already Gives The Course In The Semester!";
                    return View();
                }else if(coursesInSemester.Count() > 6) 
                {
                    ViewBag.Message = "A Teacher Cannot Have More Than 6 Courses In A Semester!";
                    return View();
                }
                _context.Add(teach);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseIdFkk"] = new SelectList(_context.Courses, "Id", "Name", teach.CourseIdFkk);
            ViewData["TeacherIdFk"] = new SelectList(_context.Teachers, "Id", "Id", teach.TeacherIdFk);
            return View(teach);
        }

        
        // GET: Teaches/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teach = await _context.Teaches.FindAsync(id);
            if (teach == null)
            {
                return NotFound();
            }
            ViewData["CourseIdFkk"] = new SelectList(_context.Courses, "Id", "Name", teach.CourseIdFkk);
            ViewData["TeacherIdFk"] = new SelectList(_context.Teachers, "Id", "Id", teach.TeacherIdFk);
            return View(teach);
        }

        // POST: Teaches/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CourseIdFkk,TeacherIdFk,Semester,Year")] Teach teach)
        {
            if (id != teach.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(teach);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeachExists(teach.Id))
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
            ViewData["CourseIdFkk"] = new SelectList(_context.Courses, "Id", "Name", teach.CourseIdFkk);
            ViewData["TeacherIdFk"] = new SelectList(_context.Teachers, "Id", "Id", teach.TeacherIdFk);
            return View(teach);
        }

        // GET: Teaches/Delete/5
        
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teach = await _context.Teaches
                .Include(t => t.CourseIdFkkNavigation)
                .Include(t => t.TeacherIdFkNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teach == null)
            {
                return NotFound();
            }

            return View(teach);
        }

        // POST: Teaches/Delete/5
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teach = await _context.Teaches.FindAsync(id);
            _context.Teaches.Remove(teach);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeachExists(int id)
        {
            return _context.Teaches.Any(e => e.Id == id);
        }
    }
}
