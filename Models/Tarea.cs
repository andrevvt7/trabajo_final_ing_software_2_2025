using Kanban.ViewModels;

namespace Kanban.Models;

public class Tarea{
    int id;
    int idTablero;
    string? nombre;
    string? descripcion;
    string? color;
    EstadoTarea estado;
    int? idUsuarioAsignado;

    public int Id { get => id; set => id = value; }
    public int IdTablero { get => idTablero; set => idTablero = value; }
    public string? Nombre { get => nombre; set => nombre = value; }
    public string? Descripcion { get => descripcion; set => descripcion = value; }
    public string? Color { get => color; set => color = value; }
    public EstadoTarea Estado { get => estado; set => estado = value; }
    public int? IdUsuarioAsignado { get => idUsuarioAsignado; set => idUsuarioAsignado = value; }

    public Tarea(){}

    public Tarea(TareaViewModel tareaViewModel){
        Id = tareaViewModel.Id;
        IdTablero = tareaViewModel.IdTablero;
        Nombre = tareaViewModel.Nombre;
        Descripcion = tareaViewModel.Descripcion;
        Color = tareaViewModel.Color;
        Estado = (EstadoTarea)tareaViewModel.Estado;
        IdUsuarioAsignado = tareaViewModel.IdUsuarioAsignado;
    }
    public Tarea(CrearTareaViewModel tareaViewModel){
        IdTablero = tareaViewModel.IdTablero;
        Nombre = tareaViewModel.Nombre;
        Descripcion = tareaViewModel.Descripcion;
        Color = tareaViewModel.Color;
        Estado = (EstadoTarea)tareaViewModel.Estado;
        IdUsuarioAsignado = tareaViewModel.IdUsuarioAsignado;
    }
    public Tarea(ModificarTareaViewModel tareaViewModel){
        Id = tareaViewModel.Id;
        Nombre = tareaViewModel.Nombre;
        Descripcion = tareaViewModel.Descripcion;
        Color = tareaViewModel.Color;
        Estado = (EstadoTarea)tareaViewModel.Estado;
        IdUsuarioAsignado = tareaViewModel.IdUsuarioAsignado;
    }
}

public enum EstadoTarea{
    Ideas = 1,
    ToDo = 2,
    Doing = 3,
    Review = 4,
    Done = 5
}

public enum ColorTarea{
    fcd2b5 = 1,
    fcc3b5 = 2,
    d8fcb5 = 3,
    b5fced = 4,
    f7fcb5 = 5
}