using Kanban.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Kanban.ViewModels;
public class ModificarTareaViewModel{
    public int Id { get ; set ; }
    
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
    public int IdUsuarioPropietario { get ; set ; }

    public ModificarTareaViewModel(Tarea tarea, List<Usuario> usuarios, Tablero tablero){
        Id = tarea.Id;
        Nombre = tarea.Nombre;
        Descripcion = tarea.Descripcion;
        Color = tarea.Color;
        Estado = (EstadoTarea)tarea.Estado;
        IdUsuarioAsignado = tarea.IdUsuarioAsignado;
        Usuarios = usuarios;
        IdUsuarioPropietario = tablero.IdUsuarioPropietario;
    }
    public ModificarTareaViewModel(){}   
}
