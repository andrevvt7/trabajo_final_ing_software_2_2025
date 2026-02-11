using Kanban.Models;
using System.ComponentModel.DataAnnotations;

namespace Kanban.ViewModels;
public class ModificarTableroViewModel{
    public int Id { get; set; }

    [Required(ErrorMessage = "Este campo es requerido.")]
    [Display(Name = "Nombre")]
    [DataType(DataType.Text)]
    public string Nombre { get; set; }

    [Display(Name = "Descripción")]
    [StringLength(100)]
    [DataType(DataType.Text)]
    public string? Descripcion { get; set; }

    public ModificarTableroViewModel(Tablero tablero){
        Id = tablero.Id;
        Nombre = tablero.Nombre;
        Descripcion = tablero.Descripcion;
    }
    public ModificarTableroViewModel(){}   
}