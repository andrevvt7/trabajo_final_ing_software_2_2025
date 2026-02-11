using Microsoft.AspNetCore.Mvc;
using Kanban.Models;
using Kanban.Repositories;
using Kanban.ViewModels;
namespace Kanban.Controllers;

public class TableroController : Controller
{
    private readonly ITableroRepository _tableroRepository;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly ILogger<TableroController> _logger;

    public TableroController(ITableroRepository tableroRepository, IUsuarioRepository usuarioRepository, ILogger<TableroController> logger)
    {
        _tableroRepository = tableroRepository;
        _usuarioRepository = usuarioRepository;
        _logger = logger;
    }

    //En el controlador de tableros: Listar, Crear, Modificar y Eliminar Tableros, tenga en cuenta que usuario logueado es el dueño del tablero

    //Listar
    [RolUsuarioAutorizacion("Administrador","Operador")]
    [HttpGet]
    public IActionResult ListarTableros()
    {
        try
        {
            int idUsuarioLogueado = Convert.ToInt32(HttpContext.Session.GetString("IdUsuarioLogueado"));
            string rolUsuarioLogueado = HttpContext.Session.GetString("RolUsuarioLogueado");

            List<Tablero> tableros;
            Usuario usuario;
            List<TableroViewModel> tablerosViewModels = new List<TableroViewModel>();

            tableros = rolUsuarioLogueado == "Administrador" ? _tableroRepository.ListarTableros() : _tableroRepository.ListarTablerosDeUnUsuario(idUsuarioLogueado);

            //TableroViewModel(Tablero tablero, Usuario usuario)
            foreach (var tablero in tableros)
            {
                usuario = _usuarioRepository.ObtenerUsuario(tablero.IdUsuarioPropietario);
                tablerosViewModels.Add(new TableroViewModel(tablero, usuario));
            }

            return View(tablerosViewModels);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return View("Error", "Home");
        }
    }

    //Crear
    [RolUsuarioAutorizacion("Administrador","Operador")]
    [HttpGet]
    public IActionResult CrearTablero()
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

    [RolUsuarioAutorizacion("Administrador","Operador")]
    [HttpPost]
    public IActionResult CrearTablero(CrearTableroViewModel tableroViewModel)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View("Index");
            }
            Tablero tablero = new Tablero(tableroViewModel);
            tablero.IdUsuarioPropietario = Convert.ToInt32(HttpContext.Session.GetString("IdUsuarioLogueado"));
            _tableroRepository.CrearTablero(tablero);
            return RedirectToAction("ListarTableros");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return View("Error", "Home");
        }
    }

    //Modificar
    [HttpGet]
    public IActionResult ModificarTablero(int idTablero)
    {
        try
        {
            int idUsuarioPropietario = _tableroRepository.ObtenerTablero(idTablero).IdUsuarioPropietario;

            if (HttpContext.Session.GetString("IsAuthenticated") != "true" || (Convert.ToInt32(HttpContext.Session.GetString("IdUsuarioLogueado")) != idUsuarioPropietario && HttpContext.Session.GetString("RolUsuarioLogueado") != "Administrador"))
            {
                return RedirectToAction("Error", "Home");
            }

            return View(new ModificarTableroViewModel(_tableroRepository.ObtenerTablero(idTablero)));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return View("Error", "Home");
        }
    }

    [RolUsuarioAutorizacion("Administrador","Operador")]
    [HttpPost]
    public IActionResult ModificarTablero(ModificarTableroViewModel tableroModificadoViewModel)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View("Index");
            }

            Tablero tablero = new Tablero(tableroModificadoViewModel);
            Tablero tableroExistente = _tableroRepository.ObtenerTablero(tablero.Id);

            tableroExistente.Nombre = !string.IsNullOrEmpty(tablero.Nombre) ? tablero.Nombre : string.Empty;
            tableroExistente.Descripcion = tablero.Descripcion;

            _tableroRepository.ModificarTablero(tablero.Id, tableroExistente);

            return RedirectToAction("ListarTableros");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return View("Error", "Home");
        }
    }

    //Eliminar
    [RolUsuarioAutorizacion("Administrador","Operador")]
    [HttpPost]
    public IActionResult EliminarTablero(int idTablero)
    {
        try
        {
            int idUsuarioPropietario = _tableroRepository.ObtenerTablero(idTablero).IdUsuarioPropietario;

            bool eliminado = _tableroRepository.EliminarTablero(idTablero);

            if (eliminado)
            {
                TempData["TipoEliminacion"] = "exitosa";
                TempData["MensajeEliminacion"] = "El tablero se eliminó correctamente.";
            }
            else
            {
                TempData["TipoEliminacion"] = "fallida";
                TempData["MensajeEliminacion"] = "El tablero no se pudo eliminar porque contiene tareas.";
            }

            return RedirectToAction("ListarTableros");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return View("Error", "Home");
        }
    }
}