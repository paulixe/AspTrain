using AspApp.Data;
using AspApp.Models;
using AspApp.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class WorkshopController : Controller
    {
        private readonly AspAppDbContext _context;

        public WorkshopController(AspAppDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Workshop
        public async Task<IActionResult> Index()
        {
            return View(await _context.Workshops.Include("ToolsAvailable").ToListAsync());
        }

        // GET: Admin/Workshop/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workshop = await _context.Workshops
                .FirstOrDefaultAsync(m => m.Id == id);
            if (workshop == null)
            {
                return NotFound();
            }

            return View(workshop);
        }

        // GET: Admin/Workshop/Create
        public async Task<IActionResult> Create()
        {
            return View(await GetVM(null));
        }

        // POST: Admin/Workshop/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WorkshopVM workshopVM)
        {
            if (ModelState.IsValid)
            {
                IEnumerable<int> toolsId = workshopVM.ToolCheckboxes.Where(c => c.IsChecked).Select(c => c.Tool.Id);
                workshopVM.Workshop.ToolsAvailable.AddRange(_context.Tools.Where(t => toolsId.Contains(t.Id)));
                _context.Add(workshopVM.Workshop);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(workshopVM);
        }

        // GET: Admin/Workshop/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workshopVm = await GetVM(id);
            if (workshopVm.Workshop == null)
            {
                return NotFound();
            }
            return View(workshopVm);
        }

        // POST: Admin/Workshop/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, WorkshopVM workshopVM)
        {
            Workshop workshop = workshopVM.Workshop;
            ModelState.ClearValidationState("BookingDate");
            ModelState.MarkFieldValid("BookingDate");
            if (id != workshop.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Workshop? freshWorkshop = _context.Workshops.Include("ToolsAvailable").FirstOrDefault(t => t.Id == id);
                    freshWorkshop.Name = workshop.Name;
                    IEnumerable<int> currentsIds = freshWorkshop.ToolsAvailable.Select(t => t.Id);
                    IEnumerable<int> newIds = workshopVM.ToolCheckboxes.Where(c => c.IsChecked).Select(t => t.Tool.Id);
                    IEnumerable<int> idsToAdd = newIds.Where(i => !currentsIds.Contains(i));
                    IEnumerable<Tool> toolsToAdd = _context.Tools.Where(t => idsToAdd.Contains(t.Id));

                    freshWorkshop.ToolsAvailable.RemoveAll(t => !newIds.Contains(t.Id));
                    freshWorkshop.ToolsAvailable.AddRange(toolsToAdd);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkshopExists(workshop.Id))
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
            return View(await GetVM(id));
        }

        // GET: Admin/Workshop/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workshop = await _context.Workshops
                .FirstOrDefaultAsync(m => m.Id == id);
            if (workshop == null)
            {
                return NotFound();
            }

            return View(workshop);
        }

        // POST: Admin/Workshop/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var workshop = await _context.Workshops.FindAsync(id);
            if (workshop != null)
            {
                _context.Workshops.Remove(workshop);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private async Task<WorkshopVM> GetVM(int? id)
        {
            Workshop workshop = _context.Workshops.Include("ToolsAvailable").Where(t => t.Id == id).FirstOrDefault();
            WorkshopVM workshopVM = new WorkshopVM()
            {
                Workshop = workshop,
                ToolCheckboxes = await _context.Tools.Select(t => new ToolCheckbox()
                {
                    IsChecked = (workshop == null) ? false : workshop.ToolsAvailable.Contains(t),
                    Tool = t
                }).ToListAsync()
            };
            return workshopVM;
        }
        private bool WorkshopExists(int id)
        {
            return _context.Workshops.Any(e => e.Id == id);
        }
    }
}
