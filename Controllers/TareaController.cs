using Microsoft.AspNetCore.Mvc;
using Kanban.Models;
using Kanban.Repositories;
using Kanban.ViewModels;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Kanban.Controllers;

public class TareaController : Controller
{
    private readonly ITareaRepository _tareaRepository;
    private readonly ITableroRepository _tableroRepository;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly ILogger<TareaController> _logger;

    public TareaController(ITareaRepository tareaRepository, ITableroRepository tableroRepository, IUsuarioRepository usuarioRepository, ILogger<TareaController> logger)
    {
        _tareaRepository = tareaRepository;
        _tableroRepository = tableroRepository;
        _usuarioRepository = usuarioRepository;
        _logger = logger;
    }

    //En el controlador de tareas: Listar, Crear, Modificar y Eliminar Tareas. (Por el momento asuma que el tablero al que pertenece la tarea es siempre la misma, y que no posee usuario asignado)

    //Listar
    [RolUsuarioAutorizacion("Administrador", "Operador")]
    [HttpGet]
    public IActionResult ListarTareas(int idTablero)
    {
        try
        {
            TempData["IdTablero"] = idTablero; // guardo el id de ese tablero que no tiene tareas para usarlo cuando quiera agregar una tarea

            if (!_tareaRepository.ListarTareasDeUnTablero(idTablero).Any()) //que pasa si el tablero no tiene tareas??
            {
                TempData["MensajeListaTareasVacia"] = "No hay tareas en este tablero."; // guardo un mensaje para mostrar en el model junto a la opcion de agregar tarea
                return RedirectToAction("ListarTableros", "Tablero"); //redirecciono a la lista de tableros donde se deberá activar el model con las opciones
            }

            List<Tarea> tareas = _tareaRepository.ListarTareasDeUnTablero(idTablero);

            Usuario usuario; //para mostrar los nombres de los usuarios asignados
            List<TareaViewModel> tareasViewModels = new List<TareaViewModel>();

            //public TareaViewModel(Tarea tarea, Usuario usuario, Tablero tablero)
            foreach (var tarea in tareas)
            {
                usuario = tarea.IdUsuarioAsignado != null ? _usuarioRepository.ObtenerUsuario((int)tarea.IdUsuarioAsignado) : new Usuario();
                tareasViewModels.Add(new TareaViewModel(tarea, usuario, _tableroRepository.ObtenerTablero(idTablero)));
            }

            return View(tareasViewModels);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return View("Error", "Home");
        }
    }

    //Crear
    [RolUsuarioAutorizacion("Administrador", "Operador")]
    [HttpGet]
    public IActionResult CrearTarea(int idTablero)
    {
        try
        {
            CrearTareaViewModel tareaViewModel = new CrearTareaViewModel(idTablero);

            tareaViewModel.Usuarios = _usuarioRepository.ListarUsuarios();

            return View(tareaViewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return View("Error", "Home");
        }
    }

    [RolUsuarioAutorizacion("Administrador", "Operador")]
    [HttpPost]
    public IActionResult CrearTarea(CrearTareaViewModel tareaViewModel)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View(tareaViewModel);
            }

            _tareaRepository.CrearTarea(tareaViewModel.IdTablero, new Tarea(tareaViewModel));

            return RedirectToAction("ListarTareas", new { idTablero = tareaViewModel.IdTablero });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return View("Error", "Home");
        }
    }


    //Modificar
    [RolUsuarioAutorizacion("Administrador", "Operador")]
    [HttpGet]
    public IActionResult ModificarTarea(int idTarea)
    {
        try
        {
            Tarea tarea = _tareaRepository.ObtenerTarea(idTarea);
            int idUsuarioPropietario = _tableroRepository.ObtenerTablero(tarea.IdTablero).IdUsuarioPropietario;

            if (Convert.ToInt32(HttpContext.Session.GetString("IdUsuarioLogueado")) != idUsuarioPropietario && HttpContext.Session.GetString("RolUsuarioLogueado") != "Administrador")
            {
                if (Convert.ToInt32(HttpContext.Session.GetString("IdUsuarioLogueado")) != tarea.IdUsuarioAsignado)
                {

                    return RedirectToAction("Error", "Home");
                }
            }

            //public ModificarViewModel(Tarea tarea, Usuario usuario, List<ColorViewModel>, Tablero tablero)
            return View(new ModificarTareaViewModel(tarea, _usuarioRepository.ListarUsuarios(), _tableroRepository.ObtenerTablero(tarea.IdTablero)));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return View("Error", "Home");
        }
    }

    [RolUsuarioAutorizacion("Administrador", "Operador")]
    [HttpPost]
    public IActionResult ModificarTarea(ModificarTareaViewModel tareaModificada)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View("ListarTareas");
            }

            Tarea tareaExistente = _tareaRepository.ObtenerTarea(tareaModificada.Id);
            tareaExistente.Nombre = !string.IsNullOrEmpty(tareaModificada.Nombre) ? tareaModificada.Nombre : string.Empty;
            tareaExistente.Descripcion = tareaModificada.Descripcion;
            tareaExistente.Color = tareaModificada.Color;
            tareaExistente.IdUsuarioAsignado = tareaModificada.IdUsuarioAsignado;

            if (tareaModificada.Estado != 0)
            {
                tareaExistente.Estado = (Models.EstadoTarea)tareaModificada.Estado;
            }

            _tareaRepository.ModificarTarea(tareaExistente.Id, tareaExistente);

            return RedirectToAction("ListarTareas", new { idTablero = tareaExistente.IdTablero });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return View("Error", "Home");
        }
    }

    //Eliminar
    [RolUsuarioAutorizacion("Administrador", "Operador")]
    [HttpPost]
    public IActionResult EliminarTarea(int idTarea)
    {
        try
        {
            int IdTablero = _tareaRepository.ObtenerTarea(idTarea).IdTablero;
            bool eliminado = _tareaRepository.EliminarTarea(idTarea);

            if (eliminado)
            {
                TempData["TipoEliminacion"] = "exitosa";
                TempData["MensajeEliminacion"] = "La tarea se eliminó correctamente.";
            }
            else
            {
                TempData["TipoEliminacion"] = "fallida";
                TempData["MensajeEliminacion"] = "La tarea no se pudo eliminar.";
            }
            return RedirectToAction("ListarTareas", new { idTablero = IdTablero });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return View("Error", "Home");
        }
    }
}