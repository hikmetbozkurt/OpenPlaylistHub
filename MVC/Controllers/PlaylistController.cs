using APP.Business.Services;
using APP.Models;
using CORE.APP.Services.MVC;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    public class PlaylistController : Controller
    {
        private readonly IService<PlaylistRequest, PlaylistResponse> _service;
        private readonly IService<UserRequest, UserResponse> _userService;
        private readonly IService<TrackRequest, TrackResponse> _trackService;
        private readonly PlaylistService _playlistService;

        public PlaylistController(
            IService<PlaylistRequest, PlaylistResponse> service,
            IService<UserRequest, UserResponse> userService,
            IService<TrackRequest, TrackResponse> trackService,
            PlaylistService playlistService)
        {
            _service = service;
            _userService = userService;
            _trackService = trackService;
            _playlistService = playlistService;
        }

        public IActionResult Index()
        {
            var playlists = _service.GetAll();
            return View(playlists);
        }

        public IActionResult Details(int id)
        {
            var playlist = _service.GetById(id);
            if (playlist == null)
            {
                return NotFound();
            }
            return View(playlist);
        }

        public IActionResult Create()
        {
            ViewBag.Users = _userService.GetAll();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PlaylistRequest request)
        {
            if (ModelState.IsValid)
            {
                request.CreatedDate = DateTime.Now;
                _service.Create(request);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Users = _userService.GetAll();
            return View(request);
        }

        public IActionResult Edit(int id)
        {
            var playlist = _service.GetById(id);
            if (playlist == null)
            {
                return NotFound();
            }
            ViewBag.Users = _userService.GetAll();
            var request = new PlaylistRequest
            {
                Id = playlist.Id,
                Name = playlist.Name,
                Description = playlist.Description,
                IsPublic = playlist.IsPublic,
                CreatedDate = playlist.CreatedDate,
                UserId = playlist.UserId
            };
            return View(request);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(PlaylistRequest request)
        {
            if (ModelState.IsValid)
            {
                _service.Update(request);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Users = _userService.GetAll();
            return View(request);
        }

        public IActionResult Delete(int id)
        {
            var playlist = _service.GetById(id);
            if (playlist == null)
            {
                return NotFound();
            }
            return View(playlist);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _service.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult AddTrack(int id)
        {
            var playlist = _service.GetById(id);
            if (playlist == null)
            {
                return NotFound();
            }
            ViewBag.Playlist = playlist;
            var allTracks = _trackService.GetAll();
            var playlistTrackIds = playlist.Tracks.Select(t => t.Id).ToList();
            ViewBag.Tracks = allTracks.Where(t => !playlistTrackIds.Contains(t.Id)).ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddTrack(int playlistId, int trackId)
        {
            _playlistService.AddTrackToPlaylist(playlistId, trackId);
            return RedirectToAction(nameof(Details), new { id = playlistId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveTrack(int playlistId, int trackId)
        {
            _playlistService.RemoveTrackFromPlaylist(playlistId, trackId);
            return RedirectToAction(nameof(Details), new { id = playlistId });
        }
    }
}

