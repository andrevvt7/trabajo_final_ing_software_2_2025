using Kanban.Models;
using System.ComponentModel.DataAnnotations;

namespace Kanban.ViewModels;
public class ModificarUsuarioViewModel{
    public int Id { get ; set ; }
    
    [Required(ErrorMessage = "Este campo es requerido.")]
    [Display(Name = "Nombre")]
    public string NombreDeUsuario { get ; set ; }
    
    [Required(ErrorMessage = "Este campo es requerido.")]
    [Display(Name = "Contraseña")]
    public string Password { get ; set ; }
    
    [Required(ErrorMessage = "Este campo es requerido.")]
    [Display(Name = "Rol de usuario")]
    public RolUsuario RolUsuario { get ; set ; }

    public ModificarUsuarioViewModel(Usuario usuario){
        Id = usuario.Id;
        NombreDeUsuario = usuario.NombreDeUsuario;
        Password = usuario.Password;
        RolUsuario = (RolUsuario)usuario.RolUsuario;
    }
    public ModificarUsuarioViewModel(){}
    
}