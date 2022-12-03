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
    [Authorize(Roles = "admin, user")]
    public class PaymentsController : Controller
    {
        private readonly AutorepairContext _context;

        public PaymentsController(AutorepairContext context)
        {
            _context = context;
        }

        // GET: Payments
        [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 258)]
        public ActionResult Index(SortState sortOrder, string currentFilter1,
            string currentFilter2, string searchProgressReport, string searchMechanicFIO, int? page, int? reset)
        {
            if (searchProgressReport != null || searchMechanicFIO != null || (searchProgressReport != null & searchMechanicFIO != null))
            {
                page = 1;
            }
            else
            {
                searchProgressReport = currentFilter1;
                searchMechanicFIO = currentFilter2;
            }

            IEnumerable<PaymentViewModel> paymentViewModel;
            ViewBag.CurrentFilter1 = searchProgressReport;
            ViewBag.CurrentFilter2 = searchMechanicFIO;
            ICached<Payment> cachedPayments = _context.GetService<ICached<Payment>>();

            if (reset == 1 || !HttpContext.Session.Keys.Contains("payments"))
            {
                paymentViewModel = GetOrder(cachedPayments.GetList());
                HttpContext.Session.SetList("payments", paymentViewModel);
            }
            else
            {
                paymentViewModel = HttpContext.Session.Get<IEnumerable<PaymentViewModel>>("payments");
            }
            paymentViewModel = _SearchProgressReport(_SearchMechanicFIO(paymentViewModel, searchMechanicFIO), searchProgressReport);
            ViewBag.CurrentSort = sortOrder;
            paymentViewModel = _Sort(paymentViewModel, sortOrder);

            if (!HttpContext.Session.Keys.Contains("payments") || searchProgressReport != null || searchMechanicFIO != null)
            {
                HttpContext.Session.SetList("payments", paymentViewModel);
            }

            int pageSize = 20;
            int pageNumber = page ?? 1;

            return View(paymentViewModel.ToPagedList(pageNumber, pageSize));
        }


        // GET: Payments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ICached<Payment> cachedPayments = _context.GetService<ICached<Payment>>();
            List<Payment> payments = (List<Payment>)cachedPayments.GetList("cachedPayments");

            var paymentViewModel = from p in payments
                               join c in _context.Cars
                               on p.CarId equals c.CarId
                               join m in _context.Mechanics
                               on p.MechanicId equals m.MechanicId
                               where p.PaymentId == id 
                               select new PaymentViewModel
                               {
                                   PaymentId = p.PaymentId,
                                   StateNumberCar = c.StateNumber,
                                   MechanicFIO = m.FirstName + " " + m.MiddleName + " " + m.LastName,
                                   Date = p.Date,
                                   Cost = p.Cost,
                                   ProgressReport = p.ProgressReport
                               };
            if (paymentViewModel == null)
            {
                return NotFound();
            }

            return View(paymentViewModel.FirstOrDefault());
        }

        // GET: Payments/Create
        public IActionResult Create()
        {
            ViewData["CarId"] = new SelectList(_context.Cars, "CarId", "StateNumber");
            ViewData["MechanicId"] = new SelectList(_context.Mechanics, "MechanicId", "MiddleName");
            return View();
        }

        // POST: Payments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PaymentId,CarId,Date,Cost,MechanicId,ProgressReport")] Payment payment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(payment);
                await _context.SaveChangesAsync();
                _context.GetService<ICached<Payment>>().AddList("cachedPayments");

                IEnumerable<PaymentViewModel> paymentViewModel = GetOrder(_context.GetService<ICached<Payment>>().GetList());
                HttpContext.Session.SetList("payments", paymentViewModel);

                return RedirectToAction(nameof(Index));
            }
            ViewData["CarId"] = new SelectList(_context.Cars, "CarId", "CarId", payment.CarId);
            ViewData["MechanicId"] = new SelectList(_context.Mechanics, "MechanicId", "MechanicId", payment.MechanicId);
            return View(payment);
        }

        // GET: Payments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.Payments
                .SingleOrDefaultAsync(p => p.PaymentId == id);
             
            if (payment == null)
            {
                return NotFound();
            }
            ViewData["CarId"] = new SelectList(_context.Cars, "CarId", "StateNumber", payment.CarId);
            ViewData["MechanicId"] = new SelectList(_context.Mechanics, "MechanicId", "MiddleName", payment.MechanicId);
            return View(payment);
        }

        // POST: Payments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PaymentId,CarId,Date,Cost,MechanicId,ProgressReport")] Payment payment)
        {
            if (id != payment.PaymentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(payment);
                    await _context.SaveChangesAsync();

                    IEnumerable<PaymentViewModel> paymentViewModel = GetOrder(_context.GetService<ICached<Payment>>().GetList());
                    HttpContext.Session.SetList("payments", paymentViewModel);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PaymentExists(payment.PaymentId))
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
            ViewData["CarId"] = new SelectList(_context.Cars, "CarId", "CarId", payment.CarId);
            ViewData["MechanicId"] = new SelectList(_context.Mechanics, "MechanicId", "MechanicId", payment.MechanicId);
            return View(payment);
        }

        // GET: Payments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ICached<Payment> cachedPayments = _context.GetService<ICached<Payment>>();
            List<Payment> payments = (List<Payment>)cachedPayments.GetList("cachedPayments");

            var paymentViewModel = from p in payments
                                   join c in _context.Cars
                                   on p.CarId equals c.CarId
                                   join m in _context.Mechanics
                                   on p.MechanicId equals m.MechanicId
                                   where p.PaymentId == id
                                   select new PaymentViewModel
                                   {
                                       PaymentId = p.PaymentId,
                                       StateNumberCar = c.StateNumber,
                                       MechanicFIO = m.FirstName + " " + m.MiddleName + " " + m.LastName,
                                       Date = p.Date,
                                       Cost = p.Cost,
                                       ProgressReport = p.ProgressReport
                                   };
            if (paymentViewModel == null)
            {
                return NotFound();
            }
            return View(paymentViewModel.FirstOrDefault());
        }

        // POST: Payments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var payment = await _context.Payments.FindAsync(id);
            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();

            IEnumerable<PaymentViewModel> paymentViewModel = GetOrder(_context.GetService<ICached<Payment>>().GetList());
            HttpContext.Session.SetList("payments", paymentViewModel);

            return RedirectToAction(nameof(Index));
        }

        private IEnumerable<PaymentViewModel> _SearchProgressReport(IEnumerable<PaymentViewModel> payments, string progressReport)
        {
            try
            {
                if (!String.IsNullOrEmpty(progressReport))
                {
                    payments = payments.Where(p => p.ProgressReport.Contains(progressReport));
                }
            }
            catch
            {
            }
            return payments;
        }

        private IEnumerable<PaymentViewModel> _SearchMechanicFIO(IEnumerable<PaymentViewModel> payments, string mechanicFIO)
        {
            if (!String.IsNullOrEmpty(mechanicFIO))
            {
                payments = payments.Where(p => p.MechanicFIO.Contains(mechanicFIO));
            }
            return payments;
        }

        private IEnumerable<PaymentViewModel> _Sort(IEnumerable<PaymentViewModel> payments, SortState sortOrder)
        {
            ViewData["Cost"] = sortOrder == SortState.PaymentCostAsc ? SortState.PaymentCostDesc : SortState.PaymentCostAsc;
            ViewData["Date"] = sortOrder == SortState.PaymentDateAsc ? SortState.PaymentDateDesc : SortState.PaymentDateAsc;
            payments = sortOrder switch
            {
                SortState.PaymentCostAsc => payments.OrderBy(p => p.Cost),
                SortState.PaymentCostDesc => payments.OrderByDescending(p => p.Cost),
                SortState.PaymentDateAsc => payments.OrderBy(p => p.Date),
                SortState.PaymentDateDesc => payments.OrderByDescending(p => p.Date),
                _ => payments.OrderBy(p => p.PaymentId),
            };
            return payments;
        }

        private bool PaymentExists(int id)
        {
            return _context.Payments.Any(e => e.PaymentId == id);
        }

        public IEnumerable<PaymentViewModel> GetOrder(IEnumerable<Payment> payments)
        {
            IEnumerable<PaymentViewModel> paymentViewModel = from p in payments
                                                         join c in _context.Cars
                                                         on p.CarId equals c.CarId
                                                         join m in _context.Mechanics
                                                         on p.MechanicId equals m.MechanicId
                                                         select new PaymentViewModel
                                                         {
                                                             PaymentId = p.PaymentId,
                                                             StateNumberCar = c.StateNumber,
                                                             MechanicFIO = m.FirstName + " " + m.MiddleName + " " + m.LastName,
                                                             Date = p.Date,
                                                             Cost = p.Cost,
                                                             ProgressReport = p.ProgressReport
                                                         };
            return paymentViewModel;
        }
    }
}
