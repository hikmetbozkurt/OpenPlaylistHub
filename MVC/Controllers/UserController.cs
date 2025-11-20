using APP.Business.Services;
using APP.Models;
using CORE.APP.Services.MVC;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    public class UserController : Controller
    {
        private readonly IService<UserRequest, UserResponse> _service;
        private readonly IService<GroupRequest, GroupResponse> _groupService;

        public UserController(IService<UserRequest, UserResponse> service, IService<GroupRequest, GroupResponse> groupService)
        {
            _service = service;
            _groupService = groupService;
        }

        public IActionResult Index()
        {
            var users = _service.List();
            return View(users);
        }

        public IActionResult Details(int id)
        {
            var user = _service.Item(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        public IActionResult Create()
        {
            ViewBag.Groups = _groupService.List();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(UserRequest request)
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
            ViewBag.Groups = _groupService.List();
            return View(request);
        }

        public IActionResult Edit(int id)
        {
            var request = _service.Edit(id);
            if (request == null)
            {
                return NotFound();
            }
            ViewBag.Groups = _groupService.List();
            return View(request);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(UserRequest request)
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
            ViewBag.Groups = _groupService.List();
            return View(request);
        }

        public IActionResult Delete(int id)
        {
            var user = _service.Item(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var response = _service.Delete(id);
            if (!response.IsSuccessful)
            {
                ModelState.AddModelError(string.Empty, response.Message);
                var user = _service.Item(id);
                return View(user);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
