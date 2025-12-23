using APP.Business.Services;
using APP.Models;
using CORE.APP.Services.MVC;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MVC.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IService<UserRequest, UserResponse> _service;

        public UserController(IService<UserRequest, UserResponse> service)
        {
            _service = service;
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

        [AllowAnonymous]
        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public IActionResult Create(UserRequest request)
        {
            if (ModelState.IsValid)
            {
                var response = _service.Create(request);
                if (response.IsSuccessful)
                {
                    // If creating account anonymously, redirect to Login
                    if (User.Identity?.IsAuthenticated != true)
                    {
                        return RedirectToAction(nameof(Login));
                    }
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

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserRequest request)
        {
            var service = _service as UserService;
            var user = service?.Login(request.UserName, request.Password);

            if (user != null)
            {
                string role = "User"; // Default or manage via UserRoles logic if needed now that Group is gone

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role, role),
                    new Claim("Id", user.Id.ToString())
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Invalid user name or password.");
            return View();
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        private bool IsOwnAccount(int id)
        {
            var claimId = User.Claims.FirstOrDefault(c => c.Type == "Id");
            return claimId != null && claimId.Value == id.ToString();
        }
    }
}
