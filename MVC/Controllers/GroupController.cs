using APP.Models;
using CORE.APP.Services.MVC;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    public class GroupController : Controller
    {
        private readonly IService<GroupRequest, GroupResponse> _service;

        public GroupController(IService<GroupRequest, GroupResponse> service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            var groups = _service.GetAll();
            return View(groups);
        }

        public IActionResult Details(int id)
        {
            var group = _service.GetById(id);
            if (group == null)
            {
                return NotFound();
            }
            return View(group);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(GroupRequest request)
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
            var group = _service.GetById(id);
            if (group == null)
            {
                return NotFound();
            }
            var request = new GroupRequest
            {
                Id = group.Id,
                Name = group.Name,
                Description = group.Description
            };
            return View(request);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(GroupRequest request)
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
            var group = _service.GetById(id);
            if (group == null)
            {
                return NotFound();
            }
            return View(group);
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

