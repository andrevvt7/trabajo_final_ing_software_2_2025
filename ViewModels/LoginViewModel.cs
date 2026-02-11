using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace Kanban.ViewModels;
public class LoginViewModel
{
    [Required(ErrorMessage = "Este campo es requerido.")]
    [Display(Name = "Nombre de usuario")]
    public string NombreDeUsuario { get; set; }

    [Required(ErrorMessage = "Este campo es requerido.")]
    [DataType(DataType.Password)]
    [Display(Name = "Contraseña")]
    public string Password { get; set; }

    public string? ErrorMessage { get; set; }
    public bool IsAuthenticated { get; set; } 
}
