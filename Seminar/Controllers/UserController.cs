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
            if (ModelState.IsValid)
            {
                await _userService.CreateAsync(user);
                TempData["success"] = "Category created successfully";
                return RedirectToAction("Index");
            }
            return NotFound();   
        }


        // [HttpPost]
        // public async Task<IActionResult> Post(User newBook)
        // {
        //     await _userService.CreateAsync(newBook);
        //
        //     return CreatedAtAction(nameof(Get), new { id = newBook.Id }, newBook);
        // }

        /*[HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, User updatedUser)
        {
            var user = await _userService.GetAsync(id);

            if (user is null)
            {
                return NotFound();
            }

            updatedUser.Id = user.Id;

            await _userService.UpdateAsync(id, updatedUser);

            return NoContent();
        }*/

        // [HttpGet]
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

        public IActionResult Edit()
        {
            throw new NotImplementedException();
        }
    }
}