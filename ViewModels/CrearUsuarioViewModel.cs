using Kanban.Models;
using System.ComponentModel.DataAnnotations;

namespace Kanban.ViewModels;
public class CrearUsuarioViewModel{
    
    [Required(ErrorMessage = "Este campo es requerido.")]
    [Display(Name = "Nombre de usuario")]
    public string NombreDeUsuario { get; set ; }
    
    [Required(ErrorMessage = "Este campo es requerido.")]
    [DataType(DataType.Password)]
    [Display(Name = "Contraseña")]
    public string Password { get; set ; }

    [Required(ErrorMessage = "Este campo es requerido.")]
    [Display(Name = "Rol del usuario")]
    public RolUsuario RolUsuario { get; set ; }
    
    public CrearUsuarioViewModel(Usuario usuario){
        NombreDeUsuario = usuario.NombreDeUsuario;
        Password = usuario.Password;
        RolUsuario = (RolUsuario)usuario.RolUsuario;
    }
    public CrearUsuarioViewModel(){}
    
}