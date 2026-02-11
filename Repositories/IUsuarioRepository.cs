using Kanban.Models;

namespace Kanban.Repositories;

public interface IUsuarioRepository{
    public void CrearUsuario(Usuario usuario);
    public void ModificarUsuario(int id, Usuario usuario);
    public List<Usuario> ListarUsuarios();
    public Usuario ObtenerUsuario(int id);
    public Usuario ObtenerUsuarioAutenticacion(string nombre, string password);
    public bool EliminarUsuario(int id);
    public void CambiarPassword(int id, string passwordNueva);
}