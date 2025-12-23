using System.Linq;
using APP.Business.Services;
using APP.Models;
using CORE.APP.Services.MVC;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    [Authorize]
    public class PlaylistController : Controller
    {
        private readonly IService<PlaylistRequest, PlaylistResponse> _service;
        private readonly IService<UserRequest, UserResponse> _userService;
        private readonly IService<SongRequest, SongResponse> _songService;
        private readonly PlaylistService _playlistService;

        public PlaylistController(
            IService<PlaylistRequest, PlaylistResponse> service,
            IService<UserRequest, UserResponse> userService,
            IService<SongRequest, SongResponse> songService,
            PlaylistService playlistService)
        {
            _service = service;
            _userService = userService;
            _songService = songService;
            _playlistService = playlistService;
        }

        public IActionResult Index()
        {
            var playlists = _service.List();
            return View(playlists);
        }

        public IActionResult Details(int id)
        {
            var playlist = _service.Item(id);
            if (playlist == null)
            {
                return NotFound();
            }
            return View(playlist);
        }

        public IActionResult Create()
        {
            ViewBag.Users = _userService.List();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PlaylistRequest request)
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
            ViewBag.Users = _userService.List();
            return View(request);
        }

        public IActionResult Edit(int id)
        {
            var request = _service.Edit(id);
            if (request == null)
            {
                return NotFound();
            }
            ViewBag.Users = _userService.List();
            return View(request);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(PlaylistRequest request)
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
            ViewBag.Users = _userService.List();
            return View(request);
        }

        public IActionResult Delete(int id)
        {
            var playlist = _service.Item(id);
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
            var response = _service.Delete(id);
            if (!response.IsSuccessful)
            {
                ModelState.AddModelError(string.Empty, response.Message);
                var playlist = _service.Item(id);
                return View(playlist);
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult AddSong(int id)
        {
            var playlist = _service.Item(id);
            if (playlist == null)
            {
                return NotFound();
            }
            ViewBag.Playlist = playlist;
            var allSongs = _songService.List();
            var playlistSongIds = playlist.Songs.Select(t => t.Id).ToList();
            ViewBag.Songs = allSongs.Where(t => !playlistSongIds.Contains(t.Id)).ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddSong(int playlistId, int songId)
        {
            var response = _playlistService.AddSongToPlaylist(playlistId, songId);
            if (!response.IsSuccessful)
            {
                TempData["Message"] = response.Message;
            }
            return RedirectToAction(nameof(Details), new { id = playlistId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveSong(int playlistId, int songId)
        {
            var response = _playlistService.RemoveSongFromPlaylist(playlistId, songId);
            if (!response.IsSuccessful)
            {
                TempData["Message"] = response.Message;
            }
            return RedirectToAction(nameof(Details), new { id = playlistId });
        }
    }
}
