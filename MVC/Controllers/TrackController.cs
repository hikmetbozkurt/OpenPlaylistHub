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
            var tracks = _service.GetAll();
            return View(tracks);
        }

        public IActionResult Details(int id)
        {
            var track = _service.GetById(id);
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
                _service.Create(request);
                return RedirectToAction(nameof(Index));
            }
            return View(request);
        }

        public IActionResult Edit(int id)
        {
            var track = _service.GetById(id);
            if (track == null)
            {
                return NotFound();
            }
            var request = new TrackRequest
            {
                Id = track.Id,
                Title = track.Title,
                Album = track.Album,
                Duration = track.Duration,
                Rating = track.Rating,
                ReleaseDate = track.ReleaseDate,
                IsFavorite = track.IsFavorite,
                Genre = track.Genre
            };
            return View(request);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TrackRequest request)
        {
            if (ModelState.IsValid)
            {
                _service.Update(request);
                return RedirectToAction(nameof(Index));
            }
            return View(request);
        }

        public IActionResult Delete(int id)
        {
            var track = _service.GetById(id);
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
            _service.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}

