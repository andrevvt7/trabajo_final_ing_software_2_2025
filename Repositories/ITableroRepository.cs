using Kanban.Models;
namespace Kanban.Repositories;

public interface ITableroRepository{
    public void CrearTablero(Tablero tablero);
    public void ModificarTablero(int id, Tablero tablero);
    public Tablero ObtenerTablero(int id);
    public List<Tablero> ListarTableros();
    public List<Tablero> ListarTablerosDeUnUsuario(int idUsuario);
    public bool EliminarTablero(int id);
}