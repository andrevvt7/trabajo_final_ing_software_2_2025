using Kanban.ViewModels;

namespace Kanban.Models;

public class Tablero{
    int id;
    int idUsuarioPropietario;
    string nombre;
    string? descripcion;

    public int Id { get => id; set => id = value; }
    public int IdUsuarioPropietario { get => idUsuarioPropietario; set => idUsuarioPropietario = value; }
    public string Nombre { get => nombre; set => nombre = value; }
    public string? Descripcion { get => descripcion; set => descripcion = value; }

    public Tablero(){}

    public Tablero(TableroViewModel tableroViewModel){
        Id = tableroViewModel.Id;
        IdUsuarioPropietario = tableroViewModel.IdUsuarioPropietario;
        Nombre = tableroViewModel.Nombre;
        Descripcion = tableroViewModel.Descripcion;
    }
    public Tablero(CrearTableroViewModel tableroViewModel){
        Nombre = tableroViewModel.Nombre;
        Descripcion = tableroViewModel.Descripcion;
    }
    public Tablero(ModificarTableroViewModel tableroViewModel){
        Id = tableroViewModel.Id;
        Nombre = tableroViewModel.Nombre;
        Descripcion = tableroViewModel.Descripcion;
    }
}