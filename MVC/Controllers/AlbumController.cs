using APP.Models;
using CORE.APP.Services.MVC;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    public class AlbumController : Controller
    {
        private readonly IService<AlbumRequest, AlbumResponse> _service;
        private readonly IService<ArtistRequest, ArtistResponse> _artistService;

        public AlbumController(
            IService<AlbumRequest, AlbumResponse> service,
            IService<ArtistRequest, ArtistResponse> artistService)
        {
            _service = service;
            _artistService = artistService;
        }

        public IActionResult Index()
        {
            var albums = _service.List();
            return View(albums);
        }

        public IActionResult Details(int id)
        {
            var album = _service.Item(id);
            if (album == null)
            {
                return NotFound();
            }
            return View(album);
        }

        public IActionResult Create()
        {
            ViewBag.Artists = _artistService.List();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AlbumRequest request)
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
            ViewBag.Artists = _artistService.List();
            return View(request);
        }

        public IActionResult Edit(int id)
        {
            var request = _service.Edit(id);
            if (request == null)
            {
                return NotFound();
            }
            ViewBag.Artists = _artistService.List();
            return View(request);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(AlbumRequest request)
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
            ViewBag.Artists = _artistService.List();
            return View(request);
        }

        public IActionResult Delete(int id)
        {
            var album = _service.Item(id);
            if (album == null)
            {
                return NotFound();
            }
            return View(album);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var response = _service.Delete(id);
            if (!response.IsSuccessful)
            {
                ModelState.AddModelError(string.Empty, response.Message);
                var album = _service.Item(id);
                return View(album);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}

