using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookMarket.Controllers
{
    public class DisplayController : Controller
    {
        // GET: DisplayController
        public ActionResult Index()
        {
            return View();
        }

        // GET: DisplayController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DisplayController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DisplayController/Create
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

        // GET: DisplayController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DisplayController/Edit/5
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

        // GET: DisplayController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DisplayController/Delete/5
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
