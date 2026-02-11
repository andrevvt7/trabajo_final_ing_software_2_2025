using Kanban.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Kanban.ViewModels;
public class CrearTableroViewModel
{
    [Required(ErrorMessage = "Este campo es requerido.")]
    [Display(Name = "Nombre")]
    [DataType(DataType.Text)]
    public string Nombre { get; set; }
    
    [Display(Name = "Descripción")]
    [StringLength(100)]
    [DataType(DataType.Text)]
    public string? Descripcion { get; set; }
    public CrearTableroViewModel(Tablero tablero)
    {
        Nombre = tablero.Nombre;
        Descripcion = tablero.Descripcion;
    }
    public CrearTableroViewModel() { }
}