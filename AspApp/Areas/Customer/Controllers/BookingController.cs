using AspApp.Data;
using AspApp.Models;
using AspApp.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Claims;

namespace AspApp.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class BookingController : Controller
    {
        private readonly AspAppDbContext _context;

        public BookingController(AspAppDbContext context)
        {
            _context = context;
        }

        // GET: Customer/Booking
        public async Task<IActionResult> Index()
        {
            return View(await _context.Workshops.Include("ToolsAvailable").ToListAsync());
        }

        // GET: Customer/Booking/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            WorkshopVM workshopVM = await GetVM(id);

            if (workshopVM.Workshop == null)
            {
                return NotFound();
            }

            return View(workshopVM);
        }

        // GET: Customer/Booking/MyBookings
        [Authorize]
        public IActionResult MyBookings()
        {
            return View();
        }
        [HttpGet]
        [Authorize]
        public JsonResult GetMyBookings()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            List<Booking> bookings = _context.Bookings.Include("Workshop").Include("ItemsBooked").Where(b => b.CustomerId == userId).ToList();
            if (bookings == null)
            {
                return Json(new { StatusCode = HttpStatusCode.NotFound, Message = "Id null" });
            }
            var myJson = bookings.Select(b => new { b.ReservationDate, WorkshopName = b.Workshop.Name, ItemsBooked = b.ItemsBooked.Select(t => t.Name) });

            return Json(myJson);
        }


        // POST: Customer/Booking/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reserve(WorkshopVM workshopVM)
        {

            if (ModelState.IsValid)
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

                Booking booking = new Booking()
                {
                    ReservationDate = workshopVM.BookingDate,
                    CustomerId = userId,
                    WorkshopId = workshopVM.Workshop.Id
                };
                var toolsId = workshopVM.ToolCheckboxes.Where(c => c.IsChecked).Select(c => c.Tool.Id).ToList();
                booking.ItemsBooked.AddRange(_context.Tools.Include("Bookings").Where(t => toolsId.Contains(t.Id)).ToList());

                _context.Add(booking);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction("Details", workshopVM.Workshop.Id);
        }

        //Notused atm
        // GET: Customer/Booking/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.Customer)
                .Include(b => b.Workshop)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        //Notused atm
        // POST: Customer/Booking/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize]
        public JsonResult GetToolsAvailable(int? id)
        {
            if (id == null)
            {
                return Json(new { StatusCode = HttpStatusCode.BadRequest, Message = "Id null" });
            }
            Workshop workshop = _context.Workshops.Include("ToolsAvailable.Bookings").Where(t => t.Id == id).FirstOrDefault();

            if (workshop == null)
            {
                return Json(new { StatusCode = HttpStatusCode.NotFound, Message = "Id not found in db" });
            }
            var tools = workshop.ToolsAvailable.Select(t => new { t.Id, Bookings = t.Bookings.Where(b => b.WorkshopId == workshop.Id).Select(b => b.ReservationDate) });
            return Json(tools);
        }

        private async Task<WorkshopVM> GetVM(int? id)
        {
            Workshop workshop = _context.Workshops.Include("ToolsAvailable.Bookings").Where(t => t.Id == id).FirstOrDefault();

            var json = workshop.ToolsAvailable.Select(t => new { t.Id, Bookings = t.Bookings.Where(b => b.WorkshopId == workshop.Id).Select(b => b.ReservationDate) });
            WorkshopVM workshopVM = new WorkshopVM()
            {
                Workshop = workshop,
                ToolCheckboxes = workshop.ToolsAvailable.Select(t => new ToolCheckbox()
                {
                    IsChecked = false,
                    Tool = t
                }).ToList()
            };
            return workshopVM;
        }
        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.Id == id);
        }
    }
}
