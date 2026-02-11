using Kanban.ViewModels;
namespace Kanban.Models;
public class Usuario{
    int id;
    string nombreDeUsuario;
    string password;
    RolUsuario rolUsuario;

    public int Id { get => id; set => id = value; }
    public string NombreDeUsuario { get => nombreDeUsuario; set => nombreDeUsuario = value; }
    public string Password { get => password; set => password = value; }
    public RolUsuario RolUsuario { get => rolUsuario; set => rolUsuario = value; }

    public Usuario(){}

    public Usuario(UsuarioViewModel usuarioViewModel){
        Id = usuarioViewModel.Id;
        NombreDeUsuario = usuarioViewModel.NombreDeUsuario;
        Password = usuarioViewModel.Password;
        RolUsuario = (RolUsuario)usuarioViewModel.RolUsuario;
    }
    public Usuario(CrearUsuarioViewModel usuarioViewModel){
        NombreDeUsuario = usuarioViewModel.NombreDeUsuario;
        Password = usuarioViewModel.Password;
        RolUsuario = (RolUsuario)usuarioViewModel.RolUsuario;
    }
    public Usuario(ModificarUsuarioViewModel usuarioViewModel){
        Id = usuarioViewModel.Id;
        NombreDeUsuario = usuarioViewModel.NombreDeUsuario;
        RolUsuario = (RolUsuario)usuarioViewModel.RolUsuario;
    }
}

public enum RolUsuario{
    Administrador = 1,
    Operador = 2
}