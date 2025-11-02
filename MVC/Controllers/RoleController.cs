using APP.Models;
using CORE.APP.Services.MVC;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    public class RoleController : Controller
    {
        private readonly IService<RoleRequest, RoleResponse> _service;

        public RoleController(IService<RoleRequest, RoleResponse> service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            var roles = _service.GetAll();
            return View(roles);
        }

        public IActionResult Details(int id)
        {
            var role = _service.GetById(id);
            if (role == null)
            {
                return NotFound();
            }
            return View(role);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(RoleRequest request)
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
            var role = _service.GetById(id);
            if (role == null)
            {
                return NotFound();
            }
            var request = new RoleRequest
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description
            };
            return View(request);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(RoleRequest request)
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
            var role = _service.GetById(id);
            if (role == null)
            {
                return NotFound();
            }
            return View(role);
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

