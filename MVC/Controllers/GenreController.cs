using APP.Models;
using CORE.APP.Services.MVC;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    public class GenreController : Controller
    {
        private readonly IService<GenreRequest, GenreResponse> _service;

        public GenreController(IService<GenreRequest, GenreResponse> service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            var genres = _service.List();
            return View(genres);
        }

        public IActionResult Details(int id)
        {
            var genre = _service.Item(id);
            if (genre == null)
            {
                return NotFound();
            }
            return View(genre);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(GenreRequest request)
        {
            if (ModelState.IsValid)
            {
                var response = _service.Create(request);
                if (response.IsSuccessful)
                {
                    return RedirectToAction(nameof(Details), new { id = response.Id });
                }
                ModelState.AddModelError(string.Empty, response.Message);
            }
            return View(request);
        }

        public IActionResult Edit(int id)
        {
            var request = _service.Edit(id);
            if (request == null)
            {
                return NotFound();
            }
            return View(request);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(GenreRequest request)
        {
            if (ModelState.IsValid)
            {
                var response = _service.Update(request);
                if (response.IsSuccessful)
                {
                    return RedirectToAction(nameof(Details), new { id = response.Id });
                }
                ModelState.AddModelError(string.Empty, response.Message);
            }
            return View(request);
        }

        public IActionResult Delete(int id)
        {
            var genre = _service.Item(id);
            if (genre == null)
            {
                return NotFound();
            }
            return View(genre);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var response = _service.Delete(id);
            if (!response.IsSuccessful)
            {
                ModelState.AddModelError(string.Empty, response.Message);
                var genre = _service.Item(id);
                return View(genre);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}

