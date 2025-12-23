using APP.Models;
using CORE.APP.Services.MVC;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    public class ArtistController : Controller
    {
        private readonly IService<ArtistRequest, ArtistResponse> _service;

        public ArtistController(IService<ArtistRequest, ArtistResponse> service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            var artists = _service.List();
            return View(artists);
        }

        public IActionResult Details(int id)
        {
            var artist = _service.Item(id);
            if (artist == null)
            {
                return NotFound();
            }
            return View(artist);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ArtistRequest request)
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
        public IActionResult Edit(ArtistRequest request)
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
            var artist = _service.Item(id);
            if (artist == null)
            {
                return NotFound();
            }
            return View(artist);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var response = _service.Delete(id);
            if (!response.IsSuccessful)
            {
                ModelState.AddModelError(string.Empty, response.Message);
                var artist = _service.Item(id);
                return View(artist);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}

