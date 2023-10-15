using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Seminar.Models;
using Seminar.Services;

namespace Seminar.Controllers
{
    public class UserController : Controller
    {
        private readonly UserService _userService;

        public UserController(UserService userService) =>
            _userService = userService;

        public async Task<IActionResult> Index()

        {
            // if(User)
            var logged = HttpContext.Session.GetString("userId");
            if (logged == null)
            {
                TempData["error"] = "Login first";
                return RedirectToAction("Index", "Login");;
            }
            var tasks = await _userService.GetAsync();
            return View(tasks);
        }

        public IActionResult Create()
        {
            return View();
        }

        public async Task<IActionResult> CreatePOST(User user)
        {
            if (user.username == "admin")
            {
                ModelState.AddModelError("name", "Name not admin");
            }

            if (!ModelState.IsValid) return NotFound();
            await _userService.CreateAsync(user);
            TempData["success"] = "Category created successfully";
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(string? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var user = await _userService.GetAsync(id: id);

            if (user is null)
            {
                return NotFound();
            }

            return View(user);
        }

        public async Task<IActionResult> DeletePOST(string? id)
        {
            var user = await _userService.GetAsync(id);
            if (user is null)
            {
                return NotFound();
            }

            await _userService.RemoveAsync(id);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(string? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var user = await _userService.GetAsync(id: id);

            if (user is null)
            {
                return NotFound();
            }

            return View(user);
        }
        
        public async Task<IActionResult> EditPOST(String id, User? user)
        {
            await _userService.UpdateAsync(id, user);
            return RedirectToAction("Index");
        }
    }
}