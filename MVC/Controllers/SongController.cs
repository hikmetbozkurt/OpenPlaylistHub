using APP.Business.Services;
using APP.Models;
using CORE.APP.Services.MVC;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    public class SongController : Controller
    {
        private readonly IService<SongRequest, SongResponse> _service;
        private readonly SongService _songService;
        private readonly IService<AlbumRequest, AlbumResponse> _albumService;
        private readonly IService<GenreRequest, GenreResponse> _genreService;
        private readonly IService<ArtistRequest, ArtistResponse> _artistService;

        public SongController(
            IService<SongRequest, SongResponse> service,
            SongService songService,
            IService<AlbumRequest, AlbumResponse> albumService,
            IService<GenreRequest, GenreResponse> genreService,
            IService<ArtistRequest, ArtistResponse> artistService)
        {
            _service = service;
            _songService = songService;
            _albumService = albumService;
            _genreService = genreService;
            _artistService = artistService;
        }

        public IActionResult Index()
        {
            var songs = _service.List();
            return View(songs);
        }

        public IActionResult Details(int id)
        {
            var song = _service.Item(id);
            if (song == null)
            {
                return NotFound();
            }
            return View(song);
        }

        public IActionResult Create()
        {
            ViewBag.Albums = _albumService.List();
            ViewBag.Genres = _genreService.List();
            ViewBag.Artists = _artistService.List();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(SongRequest request)
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
            ViewBag.Albums = _albumService.List();
            ViewBag.Genres = _genreService.List();
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
            ViewBag.Albums = _albumService.List();
            ViewBag.Genres = _genreService.List();
            ViewBag.Artists = _artistService.List();
            return View(request);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(SongRequest request)
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
            ViewBag.Albums = _albumService.List();
            ViewBag.Genres = _genreService.List();
            ViewBag.Artists = _artistService.List();
            return View(request);
        }

        public IActionResult Delete(int id)
        {
            var song = _service.Item(id);
            if (song == null)
            {
                return NotFound();
            }
            return View(song);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var response = _service.Delete(id);
            if (!response.IsSuccessful)
            {
                ModelState.AddModelError(string.Empty, response.Message);
                var song = _service.Item(id);
                return View(song);
            }
            return RedirectToAction(nameof(Index));
        }


    }
}

