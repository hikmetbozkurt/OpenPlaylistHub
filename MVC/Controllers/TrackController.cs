using APP.Models;
using CORE.APP.Services.MVC;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    public class TrackController : Controller
    {
        private readonly IService<TrackRequest, TrackResponse> _service;

        public TrackController(IService<TrackRequest, TrackResponse> service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            var tracks = _service.List();
            return View(tracks);
        }

        public IActionResult Details(int id)
        {
            var track = _service.Item(id);
            if (track == null)
            {
                return NotFound();
            }
            return View(track);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TrackRequest request)
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
        public IActionResult Edit(TrackRequest request)
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
            var track = _service.Item(id);
            if (track == null)
            {
                return NotFound();
            }
            return View(track);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var response = _service.Delete(id);
            if (!response.IsSuccessful)
            {
                ModelState.AddModelError(string.Empty, response.Message);
                var track = _service.Item(id);
                return View(track);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
