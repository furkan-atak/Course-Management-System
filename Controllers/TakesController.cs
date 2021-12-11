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
    public class TakesController : Controller
    {
        private readonly CourseManagementDBContext _context;

        public TakesController(CourseManagementDBContext context)
        {
            _context = context;
        }

        // GET: Takes
        [HttpGet]
        public async Task<IActionResult> Index(int? StudentIdFk)
        {
            var courseManagementDBContext = StudentIdFk != null ? 
                _context.Takes.Include(t => t.CourseIdFkNavigation).Include(t => t.StudentIdFkNavigation).Where(t => t.StudentIdFk == StudentIdFk):
                 _context.Takes.Include(t => t.CourseIdFkNavigation).Include(t => t.StudentIdFkNavigation);
           
            return View(await courseManagementDBContext.ToListAsync());
        }



        // GET: Takes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var take = await _context.Takes
                .Include(t => t.CourseIdFkNavigation)
                .Include(t => t.StudentIdFkNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (take == null)
            {
                return NotFound();
            }

            return View(take);
        }
        
        // GET: Takes/Create
        public IActionResult Create()
        {
            ViewData["CourseIdFk"] = new SelectList(_context.Courses, "Id", "Name");
            ViewData["StudentIdFk"] = new SelectList(_context.Students, "Id", "FirstName");
            return View();
        }
        
        public IActionResult Create1(int id)
        {
            var student = _context.Students.FirstOrDefault(t => t.Id == id);
            var list = new List<Student>() { student };
            ViewData["CourseIdFk"] = new SelectList(_context.Courses.Where(t => t.DeptIdFk == student.DeptIdFk), "Id", "Name");
            ViewData["StudentIdFk"] = new SelectList(list, "Id", "FullName");
            return View(nameof(Create));
        }


        // POST: Takes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CourseIdFk,StudentIdFk,Semester,Year,Grade")] Take take)
        {
            if (ModelState.IsValid)
            {
                if (_context.Takes.Any(t => t.Semester == take.Semester && t.CourseIdFk == take.CourseIdFk))
                {
                    ViewBag.Message = "The Course Already Taken In The Semester!";
                    return View();
                }
                _context.Add(take);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { StudentIdFk = take.StudentIdFk});
            }
            ViewData["CourseIdFk"] = new SelectList(_context.Courses, "Id", "Name", take.CourseIdFk);
            ViewData["StudentIdFk"] = new SelectList(_context.Students, "Id", "FirstName", take.StudentIdFk);
            return View(take);
        }

        // GET: Takes/Edit/5
        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var take = await _context.Takes.FindAsync(id);
            if (take == null)
            {
                return NotFound();
            }
            ViewData["CourseIdFk"] = new SelectList(_context.Courses, "Id", "Name", take.CourseIdFk);
            ViewData["StudentIdFk"] = new SelectList(_context.Students, "Id", "FirstName", take.StudentIdFk);
            return View(take);
        }

        // POST: Takes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CourseIdFk,StudentIdFk,Semester,Year,Grade")] Take take)
        {
            if (id != take.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(take);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TakeExists(take.Id))
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
            ViewData["CourseIdFk"] = new SelectList(_context.Courses, "Id", "Name", take.CourseIdFk);
            ViewData["StudentIdFk"] = new SelectList(_context.Students, "Id", "FirstName", take.StudentIdFk);
            return View(take);
        }

        
        // GET: Takes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var take = await _context.Takes
                .Include(t => t.CourseIdFkNavigation)
                .Include(t => t.StudentIdFkNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (take == null)
            {
                return NotFound();
            }

            return View(take);
        }

        
        // POST: Takes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var take = await _context.Takes.FindAsync(id);
            _context.Takes.Remove(take);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TakeExists(int id)
        {
            return _context.Takes.Any(e => e.Id == id);
        }
    }
}
