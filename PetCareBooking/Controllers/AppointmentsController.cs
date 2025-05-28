using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetCareBooking.Data;
using PetCareBooking.Models;
using System.Threading.Tasks;
using System.Linq;

namespace PetCareBooking.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AppointmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Appointments
        public async Task<IActionResult> Index(string? searchString)
        {
            var appointments = _context.Appointments
                .Include(a => a.Customer)
                .Include(a => a.Product)
                .Include(a => a.CreatedByUser)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                appointments = appointments.Where(a =>
                    a.Customer.Name.Contains(searchString) ||
                    a.Product.Name.Contains(searchString) ||
                    a.Status.Contains(searchString));
            }

            return View(await appointments.ToListAsync());
        }

        // GET: Appointments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments
                .Include(a => a.Customer)
                .Include(a => a.Product)
                .Include(a => a.CreatedByUser)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // GET: Appointments/Create
        public IActionResult Create()
        {
            ViewData["Customers"] = _context.Customers.ToList();
            ViewData["Products"] = _context.Products.ToList();
            ViewData["Users"] = _context.Users.ToList();
            return View();
        }

        // POST: Appointments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CustomerId,ProductId,CreatedByUserId,AppointmentDate,Status")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(appointment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Customers"] = _context.Customers.ToList();
            ViewData["Products"] = _context.Products.ToList();
            ViewData["Users"] = _context.Users.ToList();
            return View(appointment);
        }

        // GET: Appointments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }
            ViewData["Customers"] = _context.Customers.ToList();
            ViewData["Products"] = _context.Products.ToList();
            ViewData["Users"] = _context.Users.ToList();
            return View(appointment);
        }

        // POST: Appointments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CustomerId,ProductId,CreatedByUserId,AppointmentDate,Status")] Appointment appointment)
        {
            if (id != appointment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appointment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentExists(appointment.Id))
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
            ViewData["Customers"] = _context.Customers.ToList();
            ViewData["Products"] = _context.Products.ToList();
            ViewData["Users"] = _context.Users.ToList();
            return View(appointment);
        }

        // GET: Appointments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments
                .Include(a => a.Customer)
                .Include(a => a.Product)
                .Include(a => a.CreatedByUser)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            _context.Appointments.Remove(appointment!);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppointmentExists(int id)
        {
            return _context.Appointments.Any(e => e.Id == id);
        }
    }
}
