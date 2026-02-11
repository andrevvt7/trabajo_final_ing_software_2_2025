using Kanban.Models;

namespace Kanban.ViewModels;
public class TableroViewModel{
    public int Id {get ; set;}
    public int IdUsuarioPropietario {get ; set;}
    public string NombreUsuarioPropietario {get ; set;}
    public string Nombre {get ; set;}
    public string? Descripcion {get ; set;}

     public TableroViewModel(Tablero tablero, Usuario usuario){
        Id = tablero.Id;
        IdUsuarioPropietario = tablero.IdUsuarioPropietario;
        NombreUsuarioPropietario = usuario.NombreDeUsuario;
        Nombre = tablero.Nombre;
        Descripcion = tablero.Descripcion;
    }
    public TableroViewModel(){}   
}