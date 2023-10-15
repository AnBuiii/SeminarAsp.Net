using Microsoft.AspNetCore.Mvc;
using Seminar.Models;
using Seminar.Services;

namespace Seminar.Controllers;

public class LoginController : Controller
{
    private readonly UserService _userService;

    private readonly ILogger<HomeController> _logger;


    public LoginController(UserService userService, ILogger<HomeController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    // GET
    public async Task<IActionResult> Index()

    {
        // if(User)
        var logged = HttpContext.Session.GetString("userId");
        if (logged != null)
        {
            return RedirectToAction("Logged", new { id = logged });
        }

        return View();
    }

    public async Task<IActionResult> Logged(String? id)
    {
        var user = await _userService.GetAsync(id);
        return View(user);
    }

    public async Task<IActionResult> LoginPost(User user)
    {
        var logged = await _userService.Login(user.username, user.password);
        if (logged != null)
        {
            TempData["success"] = "Login Success";
            HttpContext.Session.SetString("userId", logged.Id);
        }
        else
        {
            TempData["error"] = "Login Failed";
        }

        return RedirectToAction("Index");
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Remove("userId");
        return RedirectToAction("Index");
    }
}