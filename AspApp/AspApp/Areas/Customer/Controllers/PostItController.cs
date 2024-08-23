using AspApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspApp.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class PostItController : Controller
    {
        private AspAppDbContext db;
        public PostItController(AspAppDbContext dbContext)
        {
            db = dbContext;
        }
        // GET: PostItController
        [Authorize]
        public ActionResult Index()
        {
            return View(db.Posts.ToList());
        }

        // GET: PostItController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PostItController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PostItController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PostItController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PostItController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PostItController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PostItController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
