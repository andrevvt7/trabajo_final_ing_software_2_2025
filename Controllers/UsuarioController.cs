using Microsoft.AspNetCore.Mvc;
using Kanban.Models;
using Kanban.Repositories;
using Kanban.ViewModels;

namespace Kanban.Controllers;

public class UsuarioController : Controller
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly ILogger<UsuarioController> _logger;

    public UsuarioController(IUsuarioRepository usuarioRepository, ILogger<UsuarioController> logger)
    {
        _usuarioRepository = usuarioRepository;
        _logger = logger;
    }

    //En el controlador de usuarios: Listar, Crear, Modificar, y Eliminar Usuarios.

    //Listar
    [RolUsuarioAutorizacion("Administrador")]
    [HttpGet]
    public IActionResult ListarUsuarios()
    {
        try
        {
            return View(_usuarioRepository.ListarUsuarios());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return View("Error", "Home");
        }
    }

    //Crear
    [RolUsuarioAutorizacion("Administrador")]
    [HttpGet]
    public IActionResult CrearUsuario()
    {
        try
        {
            return View();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return View("Error", "Home");
        }
    }

    [RolUsuarioAutorizacion("Administrador")]
    [HttpPost]

    public IActionResult CrearUsuario(CrearUsuarioViewModel usuarioViewModel)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View("Index");
            }
            _usuarioRepository.CrearUsuario(new Usuario(usuarioViewModel));

            _logger.LogInformation("Nuevo usuario: " + @usuarioViewModel.NombreDeUsuario);

            return RedirectToAction("ListarUsuarios");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return View("Error", "Home");
        }
    }

    //Modificar
    [RolUsuarioAutorizacion("Administrador")]
    [HttpGet]

    public IActionResult ModificarUsuario(int idUsuario)
    {
        try
        {
            return View(new ModificarUsuarioViewModel(_usuarioRepository.ObtenerUsuario(idUsuario)));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return View("Error", "Home");
        }
    }

    [RolUsuarioAutorizacion("Administrador")]
    [HttpPost]
    public IActionResult ModificarUsuario(ModificarUsuarioViewModel usuarioModificado)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View("Index");
            }

            Usuario usuarioExistente = _usuarioRepository.ObtenerUsuario(usuarioModificado.Id);

            usuarioExistente.NombreDeUsuario = !string.IsNullOrEmpty(usuarioModificado.NombreDeUsuario) ? usuarioModificado.NombreDeUsuario : string.Empty;
            usuarioExistente.Password = !string.IsNullOrEmpty(usuarioModificado.Password) ? usuarioModificado.Password : string.Empty;

            if (usuarioModificado.RolUsuario != 0)
            {
                usuarioExistente.RolUsuario = (Models.RolUsuario)usuarioModificado.RolUsuario;
            }

            _usuarioRepository.ModificarUsuario(usuarioExistente.Id, usuarioExistente);

            _logger.LogInformation("Usuario modificado: " + @usuarioExistente.NombreDeUsuario);

            return RedirectToAction("ListarUsuarios");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return View("Error", "Home");
        }
    }

    //Eliminar
    [RolUsuarioAutorizacion("Administrador")]
    [HttpPost]
    public IActionResult EliminarUsuario(int idUsuario)
    {
        try
        {
            bool eliminado = _usuarioRepository.EliminarUsuario(idUsuario);

            if (eliminado)
            {
                TempData["TipoEliminacion"] = "exitosa";
                TempData["MensajeEliminacion"] = "El usuario se eliminó correctamente.";
            }
            else
            {
                TempData["TipoEliminacion"] = "fallida";
                TempData["MensajeEliminacion"] = "El usuario no se pudo eliminar porque es propietario de algún tablero o tiene tareas asignadas.";
            }
            return RedirectToAction("ListarUsuarios");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return View("Error", "Home");
        }
    }
}