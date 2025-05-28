using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetCareBooking.Data;
using PetCareBooking.Models;
using System.Threading.Tasks;
using System.Linq;

namespace PetCareBooking.Controllers
{
    public class InvoicesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InvoicesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Invoices
        public async Task<IActionResult> Index(string? searchString)
        {
            var invoices = _context.Invoices
                .Include(i => i.Appointment)
                    .ThenInclude(a => a.Customer)
                .Include(i => i.Appointment)
                    .ThenInclude(a => a.Product)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                invoices = invoices.Where(i =>
                    i.Appointment.Customer.Name.Contains(searchString) ||
                    i.Appointment.Product.Name.Contains(searchString) ||
                    i.VoucherCode.Contains(searchString));
            }

            return View(await invoices.ToListAsync());
        }

        // GET: Invoices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices
                .Include(i => i.Appointment)
                    .ThenInclude(a => a.Customer)
                .Include(i => i.Appointment)
                    .ThenInclude(a => a.Product)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // GET: Invoices/Create
        public IActionResult Create()
        {
            ViewData["Appointments"] = _context.Appointments
                .Include(a => a.Customer)
                .Include(a => a.Product)
                .ToList();
            return View();
        }

        // POST: Invoices/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AppointmentId,InvoiceDate,TotalAmount,VoucherCode,DiscountAmount,FinalAmount")] Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                // Apply voucher discount if voucher code is provided and valid
                if (!string.IsNullOrEmpty(invoice.VoucherCode))
                {
                    var voucher = await _context.Vouchers
                        .FirstOrDefaultAsync(v => v.Code == invoice.VoucherCode && v.IsActive && (v.ExpiryDate == null || v.ExpiryDate >= DateTime.Now));
                    if (voucher != null)
                    {
                        invoice.DiscountAmount = voucher.DiscountAmount;
                        invoice.FinalAmount = invoice.TotalAmount - voucher.DiscountAmount;
                    }
                    else
                    {
                        ModelState.AddModelError("VoucherCode", "Invalid or expired voucher code.");
                        ViewData["Appointments"] = _context.Appointments
                            .Include(a => a.Customer)
                            .Include(a => a.Product)
                            .ToList();
                        return View(invoice);
                    }
                }
                else
                {
                    invoice.DiscountAmount = 0;
                    invoice.FinalAmount = invoice.TotalAmount;
                }

                _context.Add(invoice);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Appointments"] = _context.Appointments
                .Include(a => a.Customer)
                .Include(a => a.Product)
                .ToList();
            return View(invoice);
        }

        // GET: Invoices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }
            ViewData["Appointments"] = _context.Appointments
                .Include(a => a.Customer)
                .Include(a => a.Product)
                .ToList();
            return View(invoice);
        }

        // POST: Invoices/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AppointmentId,InvoiceDate,TotalAmount,VoucherCode,DiscountAmount,FinalAmount")] Invoice invoice)
        {
            if (id != invoice.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(invoice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvoiceExists(invoice.Id))
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
            ViewData["Appointments"] = _context.Appointments
                .Include(a => a.Customer)
                .Include(a => a.Product)
                .ToList();
            return View(invoice);
        }

        // GET: Invoices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices
                .Include(i => i.Appointment)
                    .ThenInclude(a => a.Customer)
                .Include(i => i.Appointment)
                    .ThenInclude(a => a.Product)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // POST: Invoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var invoice = await _context.Invoices.FindAsync(id);
            _context.Invoices.Remove(invoice!);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InvoiceExists(int id)
        {
            return _context.Invoices.Any(e => e.Id == id);
        }
    }
}
