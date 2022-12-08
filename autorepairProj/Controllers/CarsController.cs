using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using autorepairProj.Data;
using autorepairProj.Models;
using autorepairProj.Services;
using autorepairProj.ViewModels;
using Microsoft.EntityFrameworkCore.Infrastructure;
using X.PagedList;
using autorepairProj.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace autorepairProj.Controllers
{
    [Authorize]
    public class CarsController : Controller
    {
        private readonly AutorepairContext _context;

        public CarsController(AutorepairContext context)
        {
            _context = context;
        }

        // GET: Cars
        [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 258)]
        public ActionResult Index(SortState sortOrder, string currentFilter1,
            string currentFilter2, string searchStateNumber, string searchOwnerFIO, int? page, int? reset)
        {
            if (searchStateNumber != null || searchOwnerFIO != null || (searchStateNumber != null & searchOwnerFIO != null))
            {
                page = 1;
            }
            else
            {
                searchStateNumber = currentFilter1;
                searchOwnerFIO = currentFilter2;
            }

            IEnumerable<CarViewModel> carViewModel;
            ViewBag.CurrentFilter1 = searchStateNumber;
            ViewBag.CurrentFilter2 = searchOwnerFIO;
            ICached<Car> cachedCars = _context.GetService<ICached<Car>>();
            if (reset == 1 || !HttpContext.Session.Keys.Contains("cars"))
            {
                carViewModel = GetOrder(cachedCars.GetList());
                HttpContext.Session.SetList("cars", carViewModel);
            }
            else
            {
                carViewModel = HttpContext.Session.Get<IEnumerable<CarViewModel>>("cars");
            }
            carViewModel = _SearchStateNumber(_SearchOwnerFIO(carViewModel, searchOwnerFIO), searchStateNumber);
            ViewBag.CurrentSort = sortOrder;
            carViewModel = _Sort(carViewModel, sortOrder);

            if (!HttpContext.Session.Keys.Contains("cars") || searchOwnerFIO != null || searchStateNumber != null)
            {
                HttpContext.Session.SetList("cars", carViewModel);
            }
            int pageSize = 20;
            int pageNumber = page ?? 1;
            return View(carViewModel.ToPagedList(pageNumber, pageSize));
        }

        // GET: Cars/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars
                .Include(c => c.Owner)
                .FirstOrDefaultAsync(m => m.CarId == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        // GET: Cars/Create
        public IActionResult Create()
        {
            ViewData["OwnerId"] = new SelectList(_context.Owners, "OwnerId", "DriverLicenseNumber");
            return View();
        }

        // POST: Cars/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CarId,Brand,Power,Color,StateNumber,OwnerId,Year,VIN,EngineNumber,AdmissionDate")] Car car)
        {
            if (ModelState.IsValid)
            {
                _context.Add(car);
                await _context.SaveChangesAsync();
                IEnumerable<CarViewModel> carViewModel = GetOrder(_context.GetService<ICached<Car>>().GetList());
                HttpContext.Session.SetList("cars", carViewModel);
                return RedirectToAction(nameof(Index));
            }
            ViewData["OwnerId"] = new SelectList(_context.Owners, "OwnerId", "DriverLicenseNumber", car.OwnerId);
            return View(car);
        }

        // GET: Cars/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            ViewData["OwnerId"] = new SelectList(_context.Owners, "OwnerId", "DriverLicenseNumber");
            return View(car);
        }

        // POST: Cars/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CarId,Brand,Power,Color,StateNumber,OwnerId,Year,VIN,EngineNumber,AdmissionDate")] Car car)
        {
            if (id != car.CarId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(car);
                    await _context.SaveChangesAsync();
                    IEnumerable<CarViewModel> carViewModel = GetOrder(_context.GetService<ICached<Car>>().GetList());
                    HttpContext.Session.SetList("cars", carViewModel);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarExists(car.CarId))
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
            ViewData["OwnerId"] = new SelectList(_context.Owners, "OwnerId", "DriverLicenseNumber");
            return View(car);
        }

        // GET: Cars/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars
                .Include(c => c.Owner)
                .FirstOrDefaultAsync(m => m.CarId == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        // POST: Cars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();
            IEnumerable<CarViewModel> carViewModel = GetOrder(_context.GetService<ICached<Car>>().GetList());
            HttpContext.Session.SetList("cars", carViewModel);
            return RedirectToAction(nameof(Index));
        }


        private IEnumerable<CarViewModel> _SearchStateNumber(IEnumerable<CarViewModel> cars, string stateNumber)
        {
            try
            {
                if (!String.IsNullOrEmpty(stateNumber))
                {
                    cars = cars.Where(m => m.StateNumber.Contains(stateNumber));
                }
            }
            catch
            {
            }
            return cars;
        }

        private IEnumerable<CarViewModel> _SearchOwnerFIO(IEnumerable<CarViewModel> cars, string ownerFIO)
        {
            if (!String.IsNullOrEmpty(ownerFIO))
            {
                cars = cars.Where(c => c.OwnerFIO.Contains(ownerFIO));
            }
            return cars;
        }

        private IEnumerable<CarViewModel> _Sort(IEnumerable<CarViewModel> cars, SortState sortOrder)
        {
            ViewData["Power"] = sortOrder == SortState.CarPowerAsc ? SortState.CarPowerDesc : SortState.CarPowerAsc;
            ViewData["DateAdmission"] = sortOrder == SortState.CarDateAdmissionAsc ? SortState.CarDateAdmissionDesc : SortState.CarDateAdmissionAsc;
            ViewData["Year"] = sortOrder == SortState.CarDateAsc ? SortState.CarDateDesc : SortState.CarDateAsc;
            cars = sortOrder switch
            {
                SortState.CarPowerAsc => cars.OrderBy(c => c.Power),
                SortState.CarPowerDesc => cars.OrderByDescending(c => c.Power),
                SortState.CarDateAdmissionAsc => cars.OrderBy(c => c.AdmissionDate),
                SortState.CarDateAdmissionDesc => cars.OrderByDescending(c => c.AdmissionDate),
                SortState.CarDateAsc => cars.OrderBy(c => c.Year),
                SortState.CarDateDesc => cars.OrderByDescending(c => c.Year),
                _ => cars.OrderBy(c => c.CarId),
            };
            return cars;
        }

        private bool CarExists(int id)
        {
            return _context.Cars.Any(e => e.CarId == id);
        }

        public IEnumerable<CarViewModel> GetOrder(IEnumerable<Car> cars)
        {
            IEnumerable<CarViewModel> carViewModel = from c in cars
                                                     join owner in _context.Owners
                                                     on c.OwnerId equals owner.OwnerId
                                                     select new CarViewModel
                                                     {
                                                         CarId = c.CarId,
                                                         Brand = c.Brand,
                                                         Color = c.Color,
                                                         Power = c.Power,
                                                         StateNumber = c.StateNumber,
                                                         OwnerFIO = owner.FirstName + " " + owner.MiddleName + " " + owner.LastName,
                                                         Year = c.Year,
                                                         VIN = c.VIN,
                                                         EngineNumber = c.EngineNumber,
                                                         AdmissionDate = c.AdmissionDate
                                                     };
            return carViewModel;
        }
    }
}
