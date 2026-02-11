using Kanban.Models;

namespace Kanban.ViewModels;
public class TareaViewModel{
    public int Id { get ; set ; }
    public int IdTablero { get ; set ; }
    public string? Nombre { get ; set ; }
    public string? Descripcion { get ; set ; }
    public string? Color { get; set ; }
    public EstadoTarea Estado { get ; set ; }
    public int? IdUsuarioAsignado { get ; set ; }
    public string? NombreDeUsuario { get ; set ; }
    public int IdUsuarioPropietarioDelTablero { get ; set ; }

    public TareaViewModel(Tarea tarea, Usuario usuario, Tablero tablero){
        Id = tarea.Id;
        IdTablero = tarea.IdTablero;
        Nombre = tarea.Nombre;
        Descripcion = tarea.Descripcion;
        Color = tarea.Color;
        Estado = (EstadoTarea)tarea.Estado;
        IdUsuarioAsignado = tarea.IdUsuarioAsignado;
        NombreDeUsuario = usuario.NombreDeUsuario;
        IdUsuarioPropietarioDelTablero = tablero.IdUsuarioPropietario;
    }
    public TareaViewModel(){}   
}

public enum EstadoTarea{
    Ideas = 1,
    ToDo = 2,
    Doing = 3,
    Review = 4,
    Done = 5
}

public enum Colores{
    fcd2b5 = 1,
    fcc3b5 = 2,
    d8fcb5 = 3,
    b5fced = 4,
    f7fcb5 = 5
}