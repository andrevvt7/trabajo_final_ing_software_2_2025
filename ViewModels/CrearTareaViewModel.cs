using Kanban.Models;
using System.ComponentModel.DataAnnotations;

namespace Kanban.ViewModels;
public class CrearTareaViewModel{
    public int IdTablero { get ; set ; }
    
    [Required(ErrorMessage = "Este campo es requerido.")]
    [Display(Name = "Nombre")]
    [DataType(DataType.Text)]
    public string? Nombre { get ; set ; }

    [Display(Name = "Descripción")]
    [StringLength(100)]
    [DataType(DataType.Text)]
    public string? Descripcion { get ; set ; }
    
    [Display(Name = "Color")]
    public string? Color { get; set ; }
    
    [Required(ErrorMessage = "Este campo es requerido.")]
    [Display(Name = "Estado de la tarea")]
    public EstadoTarea Estado { get ; set ; }
    
    [Display(Name = "Usuario asignado")]
    public int? IdUsuarioAsignado { get ; set ; }

    public List<Usuario>? Usuarios { get ; set ; }
    public CrearTareaViewModel(Tarea tareaNueva, List<Usuario> usuarios)
    {
        IdTablero = tareaNueva.IdTablero;
        Nombre = tareaNueva.Nombre;   
        Estado = (EstadoTarea)tareaNueva.Estado;   
        Descripcion = tareaNueva.Descripcion;   
        Color = tareaNueva.Color;   
        IdUsuarioAsignado = tareaNueva.IdUsuarioAsignado; 
        Usuarios = usuarios;
    }

    public CrearTareaViewModel(int idTablero)
    {
        IdTablero = idTablero;
    }

    public CrearTareaViewModel(){
    }
}