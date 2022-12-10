using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using autorepairProj.Data;
using autorepairProj.Models;
using X.PagedList;
using autorepairProj.Services;
using Microsoft.EntityFrameworkCore.Infrastructure;
using autorepairProj.ViewModels;
using autorepairProj.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace autorepairProj.Controllers
{
    [Authorize]
    public class MechanicsController : Controller
    {
        private readonly AutorepairContext _context;

        public MechanicsController(AutorepairContext context)
        {
            _context = context;
        }

        // GET: Mechanics
        [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 258)]
        public ActionResult Index(SortState sortOrder, string currentFilter1,
            string currentFilter2, string searchQualificationName, string searchExperience, int? page)
        {
            if (searchQualificationName != null || searchExperience != null || (searchQualificationName != null & searchExperience != null))
            {
                page = 1;
            }
            else
            {
                searchQualificationName = currentFilter1;
                searchExperience = currentFilter2;
            }

            IEnumerable<MechanicViewModel> mechanicViewModel;
            ViewBag.CurrentFilter1 = searchQualificationName;
            ViewBag.CurrentFilter2 = searchExperience;
            ICached<Mechanic> cachedMechanics = _context.GetService<ICached<Mechanic>>();

            if (HttpContext.Session.Keys.Contains("mechanics"))
            {
                mechanicViewModel = HttpContext.Session.Get<IEnumerable<MechanicViewModel>>("mechanics");
            }
            else
            {
                List<Mechanic> mechanics = (List<Mechanic>)cachedMechanics.GetList("cachedMechanics");
                mechanicViewModel = from m in mechanics
                                      join qual in _context.Qualifications
                                      on m.QualificationType equals qual.QualificationId                                      
                                      select new MechanicViewModel
                                      {
                                          MechanicId = m.MechanicId,
                                          FirstName = m.FirstName,
                                          MiddleName = m.MiddleName,
                                          LastName = m.LastName,
                                          QualificationName = qual.Name,
                                          Experience = (int)m.Experience
                                      };
            }
            mechanicViewModel = _SearchExperience(_SearchQualificationName(mechanicViewModel, searchQualificationName), searchExperience);
            ViewBag.CurrentSort = sortOrder;
            mechanicViewModel = _Sort(mechanicViewModel, sortOrder);
            int pageSize = 20;
            int pageNumber = page ?? 1;
            return View(mechanicViewModel.ToPagedList(pageNumber, pageSize));
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
            ViewData["QualificationType"] = new SelectList(_context.Qualifications, "QualificationId", "Name");
            return View();
        }

        // POST: Mechanics/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("MechanicId,FirstName,MiddleName,LastName,QualificationType,Experience")] Mechanic mechanic)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mechanic);
                await _context.SaveChangesAsync();
                _context.GetService<ICached<Mechanic>>().AddList("cachedMechanics");
                return RedirectToAction("Index");
            }
            ViewData["QualificationType"] = new SelectList(_context.Qualifications, "QualificationId", "Name", mechanic.QualificationType);
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
            ViewData["QualificationType"] = new SelectList(_context.Qualifications, "QualificationId", "Name", mechanic.QualificationType);
            return View(mechanic);
        }

        // POST: Mechanics/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, [Bind("MechanicId,FirstName,MiddleName,LastName,QualificationType,Experience")] Mechanic mechanic)
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
                    _context.GetService<ICached<Mechanic>>().AddList("cachedMechanics");
                    HttpContext.Session.Clear();
                }
                catch (DbUpdateConcurrencyException)
                {
                }
                return RedirectToAction("Index");
            }
            ViewData["QualificationType"] = new SelectList(_context.Qualifications, "QualificationId", "QualificationId", mechanic.QualificationType);
            return View(mechanic);
        }

        // GET: Mechanics/Delete/5
        public async Task<ActionResult> Delete(int? id)
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
            _context.GetService<ICached<Mechanic>>().AddList("cachedMechanics");
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        private IEnumerable<MechanicViewModel> _SearchExperience(IEnumerable<MechanicViewModel> mechanics, string searchExperience)
        {
            if (!String.IsNullOrEmpty(searchExperience))
            {
                try
                {
                    if (Int32.Parse(searchExperience) > 0)
                    {
                        mechanics = mechanics.Where(m => m.Experience >= Int32.Parse(searchExperience));
                    }
                }
                catch
                {
                    return mechanics;
                }
            }
            return mechanics;
        }

        private IEnumerable<MechanicViewModel> _SearchQualificationName(IEnumerable<MechanicViewModel> mechanics, string QualificationName)
        {
            if (!String.IsNullOrEmpty(QualificationName))
            {
                mechanics = mechanics.Where(m => m.QualificationName.Contains(QualificationName));
            }
            return mechanics;
        }

        private IEnumerable<MechanicViewModel> _Sort(IEnumerable<MechanicViewModel> mechanics, SortState sortOrder)
        {
            ViewData["Qualification"] = sortOrder == SortState.QualificationMechanicAsc ? SortState.QualificationMechanicDesc : SortState.QualificationMechanicAsc;
            ViewData["Experience"] = sortOrder == SortState.ExperienceMechanicAsc ? SortState.ExperienceMechanicDesc : SortState.ExperienceMechanicAsc;
            mechanics = sortOrder switch
            {
                SortState.QualificationMechanicAsc => mechanics.OrderBy(m => m.QualificationName),
                SortState.QualificationMechanicDesc => mechanics.OrderByDescending(m => m.QualificationName),
                SortState.ExperienceMechanicAsc => mechanics.OrderBy(m => m.Experience),
                SortState.ExperienceMechanicDesc => mechanics.OrderByDescending(m => m.Experience),
                _ => mechanics.OrderBy(m => m.MechanicId),
            };
            return mechanics;
        }

        private bool MechanicExists(int id)
        {
            return _context.Mechanics.Any(e => e.MechanicId == id);
        }
    }
}
