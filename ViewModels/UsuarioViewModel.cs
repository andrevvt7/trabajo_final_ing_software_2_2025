using Kanban.Models;

namespace Kanban.ViewModels;
public class UsuarioViewModel{
    public int Id { get ; set ; }
    public string NombreDeUsuario { get ; set ; }
    public string Password { get ; set ; }
    public RolUsuario RolUsuario { get ; set ; }

    public UsuarioViewModel(Usuario usuario){
        Id = usuario.Id;
        NombreDeUsuario = usuario.NombreDeUsuario;
        Password = usuario.Password;
        RolUsuario = (RolUsuario)usuario.RolUsuario;
    }
    public UsuarioViewModel(){}  
}

public enum RolUsuario{
    Administrador = 1,
    Operador = 2
}