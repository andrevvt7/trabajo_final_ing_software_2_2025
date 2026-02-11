using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Kanban.Models;

namespace Kanban.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        if (HttpContext.Session.GetString("IsAuthenticated") != "true" || HttpContext.Session.GetString("IsAuthenticated") != "false")
        {
            return RedirectToAction("Error", "Home");
        }
        var nombreDeUsuarioLogueado = HttpContext.Session.GetString("NombreDeUsuarioLogueado");
        return View("Index", nombreDeUsuarioLogueado);
    }

    public IActionResult Privacy()
    {
        if (HttpContext.Session.GetString("IsAuthenticated") != "true" || HttpContext.Session.GetString("IsAuthenticated") != "false")
        {
            return RedirectToAction("Error", "Home");
        }
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
