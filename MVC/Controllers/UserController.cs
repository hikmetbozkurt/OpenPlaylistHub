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
            var users = _service.GetAll();
            return View(users);
        }

        public IActionResult Details(int id)
        {
            var user = _service.GetById(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        public IActionResult Create()
        {
            ViewBag.Groups = _groupService.GetAll();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(UserRequest request)
        {
            if (ModelState.IsValid)
            {
                _service.Create(request);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Groups = _groupService.GetAll();
            return View(request);
        }

        public IActionResult Edit(int id)
        {
            var user = _service.GetById(id);
            if (user == null)
            {
                return NotFound();
            }
            ViewBag.Groups = _groupService.GetAll();
            var request = new UserRequest
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Password = string.Empty,
                IsActive = user.IsActive,
                BirthDate = user.BirthDate,
                GroupId = user.GroupId
            };
            return View(request);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(UserRequest request)
        {
            if (ModelState.IsValid)
            {
                _service.Update(request);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Groups = _groupService.GetAll();
            return View(request);
        }

        public IActionResult Delete(int id)
        {
            var user = _service.GetById(id);
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
            _service.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}

