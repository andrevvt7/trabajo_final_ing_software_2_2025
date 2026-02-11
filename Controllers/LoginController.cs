using Microsoft.AspNetCore.Mvc;
using Kanban.ViewModels;
using Kanban.Autenticacion;

namespace Kanban.Controllers;
public class LoginController : Controller
{
    private readonly IAuthenticationService _authenticationService;
    private readonly ILogger<LoginController> _logger;

    public LoginController(IAuthenticationService authenticationService, ILogger<LoginController> logger)
    {
        _authenticationService = authenticationService;
        _logger = logger;
    }

    public IActionResult Index()
    {
        if (HttpContext.Session.GetString("IsAuthenticated") == "true")
        {
            return RedirectToAction("ListarTableros", "Tablero");
        }

        var model = new LoginViewModel
        {
            IsAuthenticated = HttpContext.Session.GetString("IsAuthenticated") == "false"
        };
        return View(model);
    }

    [HttpPost]
    public IActionResult Login(LoginViewModel model)
    {
        try
        {
            if (_authenticationService.Login(model.NombreDeUsuario, model.Password))
            {
                model.IsAuthenticated = true;
                _logger.LogInformation("El usuario " + HttpContext.Session.GetString("NombreDeUsuarioLogueado") + " ingresó correctamente.");
                return RedirectToAction("ListarTableros", "Tablero");
            }
            _logger.LogError("Intento de ingreso inválido - Usuario: " + model.NombreDeUsuario + " - Clave ingresada: " + model.Password);
            return View("Index", model);
        }
        catch (Exception ex)
        {
            model.ErrorMessage = "Usuario y/o contraseña incorrecta.";
            _logger.LogError("Error al procesar la solicitud - " + ex.Message.ToString());
            return View("Index", model);
        }
    }

    public IActionResult Logout()
    {
        try
        {
            HttpContext.Session.Clear();
            _logger.LogInformation("Cierre de sesión exitosa.");
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            throw new Exception("No se pudo cerrar la sesión - Motivo: " + ex.Message.ToString());
        }
    }
}