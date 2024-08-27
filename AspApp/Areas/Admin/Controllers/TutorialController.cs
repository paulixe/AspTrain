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

    public class TutorialController : Controller
    {
        private readonly AspAppDbContext _context;

        public TutorialController(AspAppDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Tutorial
        public async Task<IActionResult> Index()
        {
            return View(await _context.Tutorials.Include("RequiredTools").ToListAsync());
        }


        // GET: Admin/Tutorial/Create
        public async Task<IActionResult> Create()
        {
            return View(await GetVM(null));
        }

        // POST: Admin/Tutorial/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TutorialVM tutorialVm)
        {
            if (ModelState.IsValid)
            {
                IEnumerable<int> toolsId = tutorialVm.ToolCheckboxes.Where(c => c.IsChecked).Select(c => c.Tool.Id);
                tutorialVm.Tutorial.RequiredTools.AddRange(_context.Tools.Where(t => toolsId.Contains(t.Id)));
                _context.Add(tutorialVm.Tutorial);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tutorialVm);
        }

        // GET: Admin/Tutorial/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TutorialVM tutorialVM = await GetVM(id);
            if (tutorialVM.Tutorial == null)
            {
                return NotFound();
            }
            return View(tutorialVM);
        }

        // POST: Admin/Tutorial/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TutorialVM tutorialVM)
        {
            Tutorial tutorial = tutorialVM.Tutorial;

            if (id != tutorial.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                try
                {
                    Tutorial freshTutorial = _context.Tutorials.Include("RequiredTools").FirstOrDefault(t => t.Id == id);
                    freshTutorial.Name = tutorial.Name;

                    IEnumerable<int> currentsIds = freshTutorial.RequiredTools.Select(t => t.Id);
                    IEnumerable<int> newIds = tutorialVM.ToolCheckboxes.Where(c => c.IsChecked).Select(t => t.Tool.Id);
                    IEnumerable<int> idsToAdd = newIds.Where(i => !currentsIds.Contains(i));
                    IEnumerable<Tool> toolsToAdd = _context.Tools.Where(t => idsToAdd.Contains(t.Id));

                    freshTutorial.RequiredTools.RemoveAll(t => !newIds.Contains(t.Id));
                    freshTutorial.RequiredTools.AddRange(toolsToAdd);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TutorialExists(tutorial.Id))
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

        // GET: Admin/Tutorial/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tutorial = await _context.Tutorials
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tutorial == null)
            {
                return NotFound();
            }

            return View(tutorial);
        }

        // POST: Admin/Tutorial/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tutorial = await _context.Tutorials.FindAsync(id);
            if (tutorial != null)
            {
                _context.Tutorials.Remove(tutorial);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private async Task<TutorialVM> GetVM(int? id)
        {
            Tutorial tutorial = _context.Tutorials.Include("RequiredTools").Where(t => t.Id == id).FirstOrDefault();
            TutorialVM tutorialVM = new TutorialVM()
            {
                Tutorial = tutorial,
                ToolCheckboxes = await _context.Tools.Select(t => new ToolCheckbox()
                {
                    IsChecked = (tutorial == null) ? false : tutorial.RequiredTools.Contains(t),
                    Tool = t
                }).ToListAsync()
            };
            return tutorialVM;
        }
        private bool TutorialExists(int id)
        {
            return _context.Tutorials.Any(e => e.Id == id);
        }
    }
}
