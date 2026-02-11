using Kanban.Models;
namespace Kanban.Repositories;

public interface ITareaRepository{
    public void CrearTarea(int idTablero, Tarea tarea);
    public void ModificarTarea(int id, Tarea tarea);
    public Tarea ObtenerTarea(int id);
    public List<Tarea> ListarTareasDeUnUsuario(int idUsuario);
    public List<Tarea> ListarTareasDeUnTablero(int idTablero);
    public bool EliminarTarea(int id);
    public void AsignarUsuarioATarea(int idUsuario, int idTarea);
}