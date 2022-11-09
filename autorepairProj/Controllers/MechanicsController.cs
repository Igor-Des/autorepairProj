using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using autorepairProj.Data;
using autorepairProj.Models;

namespace autorepairProj.Controllers
{
    public class MechanicsController : Controller
    {
        private readonly AutorepairContext _context;

        public MechanicsController(AutorepairContext context)
        {
            _context = context;
        }

        // GET: Mechanics
        public async Task<IActionResult> Index()
        {
            var autorepairContext = _context.Mechanics.Include(m => m.Qualification);
            return View(await autorepairContext.ToListAsync());
        }

        // GET: Mechanics/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mechanic = await _context.Mechanics
                .Include(m => m.Qualification)
                .FirstOrDefaultAsync(m => m.MechanicId == id);
            if (mechanic == null)
            {
                return NotFound();
            }

            return View(mechanic);
        }

        // GET: Mechanics/Create
        public IActionResult Create()
        {
            ViewData["QualificationType"] = new SelectList(_context.Qualifications, "QualificationId", "QualificationId");
            return View();
        }

        // POST: Mechanics/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MechanicId,FirstName,MiddleName,LastName,QualificationType,Experience")] Mechanic mechanic)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mechanic);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["QualificationType"] = new SelectList(_context.Qualifications, "QualificationId", "QualificationId", mechanic.QualificationType);
            return View(mechanic);
        }

        // GET: Mechanics/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mechanic = await _context.Mechanics.FindAsync(id);
            if (mechanic == null)
            {
                return NotFound();
            }
            ViewData["QualificationType"] = new SelectList(_context.Qualifications, "QualificationId", "QualificationId", mechanic.QualificationType);
            return View(mechanic);
        }

        // POST: Mechanics/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MechanicId,FirstName,MiddleName,LastName,QualificationType,Experience")] Mechanic mechanic)
        {
            if (id != mechanic.MechanicId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mechanic);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MechanicExists(mechanic.MechanicId))
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
            ViewData["QualificationType"] = new SelectList(_context.Qualifications, "QualificationId", "QualificationId", mechanic.QualificationType);
            return View(mechanic);
        }

        // GET: Mechanics/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mechanic = await _context.Mechanics
                .Include(m => m.Qualification)
                .FirstOrDefaultAsync(m => m.MechanicId == id);
            if (mechanic == null)
            {
                return NotFound();
            }

            return View(mechanic);
        }

        // POST: Mechanics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mechanic = await _context.Mechanics.FindAsync(id);
            _context.Mechanics.Remove(mechanic);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MechanicExists(int id)
        {
            return _context.Mechanics.Any(e => e.MechanicId == id);
        }
    }
}
