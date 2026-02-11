using Microsoft.Data.Sqlite;
using Kanban.Models;
namespace Kanban.Repositories;

public class TareaRepository : ITareaRepository
{
    private readonly string _cadenaConexion;

    public TareaRepository(string cadenaConexion)
    {
        _cadenaConexion = cadenaConexion;
    }

    //Crear una nueva tarea en un tablero. (recibe un idTablero, devuelve un objeto Tarea)
    public void CrearTarea(int idTablero, Tarea tarea)
    {
        var consulta = $"INSERT INTO Tarea (id_tablero,nombre,estado,descripcion,color,id_usuario_asignado) VALUES (@id_tablero,@nombre,@estado,@descripcion,@color,@id_usuario_asignado)";
        int filasAfectadas;

        using (SqliteConnection conexion = new SqliteConnection(_cadenaConexion))
        {
            conexion.Open();

            var comando = new SqliteCommand(consulta, conexion);

            comando.Parameters.Add(new SqliteParameter("@id_tablero", idTablero));
            comando.Parameters.Add(new SqliteParameter("@nombre", tarea.Nombre));
            comando.Parameters.Add(new SqliteParameter("@estado", tarea.Estado));
            //comando.Parameters.Add(new SqliteParameter("@descripcion", tarea.Descripcion));
            comando.Parameters.Add(new SqliteParameter("@descripcion", (object?)tarea.Descripcion ?? DBNull.Value));

            //comando.Parameters.Add(new SqliteParameter("@color", tarea.Color));
            comando.Parameters.Add(new SqliteParameter("@color", (object?)tarea.Color ?? DBNull.Value));

            //comando.Parameters.Add(new SqliteParameter("@id_usuario_asignado", tarea.IdUsuarioAsignado));
            comando.Parameters.Add(new SqliteParameter("@id_usuario_asignado", (object?)tarea.IdUsuarioAsignado ?? DBNull.Value));


            filasAfectadas = comando.ExecuteNonQuery();
            conexion.Close();
        }

        if (filasAfectadas == 0)
        {
            throw new Exception("Tarea no creada.");
        }
    }

    //Modificar una tarea existente. (recibe un id y un objeto Tarea)
    public void ModificarTarea(int id, Tarea tarea)
    {
        var consulta = $"UPDATE Tarea SET nombre=@nombre,estado=@estado,descripcion=@descripcion,color=@color,id_usuario_asignado=@id_usuario_asignado WHERE id=@id";
        int filasAfectadas;

        using (SqliteConnection conexion = new SqliteConnection(_cadenaConexion))
        {
            conexion.Open();

            var comando = new SqliteCommand(consulta, conexion);

            comando.Parameters.Add(new SqliteParameter("@nombre", tarea.Nombre));
            comando.Parameters.Add(new SqliteParameter("@estado", tarea.Estado));
            comando.Parameters.Add(new SqliteParameter("@descripcion", (object?)tarea.Descripcion ?? DBNull.Value));
            comando.Parameters.Add(new SqliteParameter("@color", (object?)tarea.Color ?? DBNull.Value));
            comando.Parameters.Add(new SqliteParameter("@id_usuario_asignado", (object?)tarea.IdUsuarioAsignado ?? DBNull.Value));
            comando.Parameters.Add(new SqliteParameter("@id", id));

            filasAfectadas = comando.ExecuteNonQuery();
            conexion.Close();
        }

        if (filasAfectadas == 0)
        {
            throw new Exception("Tarea no modificada.");
        }
    }

    //Obtener detalles de una tarea por su ID. (devuelve un objeto Tarea)
    public Tarea ObtenerTarea(int id)
    {
        var consulta = $"SELECT * FROM Tarea WHERE id=@id";
        Tarea tarea = null;

        using (SqliteConnection conexion = new SqliteConnection(_cadenaConexion))
        {
            conexion.Open();

            var comando = new SqliteCommand(consulta, conexion);

            comando.Parameters.Add(new SqliteParameter("@id", id));

            using (SqliteDataReader reader = comando.ExecuteReader())
            {
                while (reader.Read())
                {
                    tarea = new Tarea();
                    tarea.Id = Convert.ToInt32(reader["id"]);
                    tarea.IdTablero = Convert.ToInt32(reader["id_tablero"]);
                    tarea.Nombre = reader["nombre"].ToString();
                    tarea.Estado = (EstadoTarea)Convert.ToInt32(reader["estado"]);
                    tarea.Descripcion = reader["descripcion"] != DBNull.Value ? reader["descripcion"].ToString() : null;
                    tarea.Color = reader["color"] != DBNull.Value ? reader["color"].ToString() : null;
                    tarea.IdUsuarioAsignado = reader["id_usuario_asignado"] != DBNull.Value ? Convert.ToInt32(reader["id_usuario_asignado"]) : null;
                }
            }

            conexion.Close();
        }

        if (tarea == null)
        {
            throw new Exception("Tarea no encontrada.");
        } else {
            return tarea;
        }
    }

    //Listar todas las tareas asignadas a un usuario específico.(recibe un idUsuario, devuelve un list de tareas)
    public List<Tarea> ListarTareasDeUnUsuario(int idUsuario)
    {
        var consulta = @"SELECT * FROM Tarea WHERE id_usuario_asignado = @idUsuario";
        List<Tarea> tareas = null;

        using (SqliteConnection conexion = new SqliteConnection(_cadenaConexion))
        {
            conexion.Open();

            var comando = new SqliteCommand(consulta, conexion);

            comando.Parameters.Add(new SqliteParameter("@idUsuario", idUsuario));

            using (SqliteDataReader reader = comando.ExecuteReader())
            {
                tareas = new List<Tarea>();
                while (reader.Read())
                {
                    var tarea = new Tarea();
                    tarea.Id = Convert.ToInt32(reader["id"]);
                    tarea.IdTablero = Convert.ToInt32(reader["id_tablero"]);
                    tarea.Nombre = reader["nombre"].ToString();
                    tarea.Estado = (EstadoTarea)Convert.ToInt32(reader["estado"]);
                    tarea.Descripcion = reader["descripcion"] != DBNull.Value ? reader["descripcion"].ToString() : null;
                    tarea.Color = reader["color"] != DBNull.Value ? reader["color"].ToString() : null;
                    tarea.IdUsuarioAsignado = reader["id_usuario_asignado"] != DBNull.Value ? Convert.ToInt32(reader["id_usuario_asignado"]) : null;
                    tareas.Add(tarea);
                }
            }

            conexion.Close();
        }
        
        if (tareas == null)
        {
            throw new Exception("Lista de tareas vacía.");
        } else {
            return tareas;
        }
    }

    //Listar todas las tareas de un tablero específico. (recibe un idTablero, devuelve un list de tareas)
    public List<Tarea> ListarTareasDeUnTablero(int idTablero)
    {
        var consulta = @"SELECT * FROM Tarea WHERE id_tablero = @idTablero";
        List<Tarea> tareas = null;

        using (SqliteConnection conexion = new SqliteConnection(_cadenaConexion))
        {
            conexion.Open();

            var comando = new SqliteCommand(consulta, conexion);

            comando.Parameters.Add(new SqliteParameter("@idTablero", idTablero));

            using (SqliteDataReader reader = comando.ExecuteReader())
            {
                tareas = new List<Tarea>();
                while (reader.Read())
                {
                    var tarea = new Tarea();
                    tarea.Id = Convert.ToInt32(reader["id"]);
                    tarea.IdTablero = Convert.ToInt32(reader["id_tablero"]);
                    tarea.Nombre = reader["nombre"].ToString();
                    tarea.Estado = (EstadoTarea)Convert.ToInt32(reader["estado"]);
                    tarea.Descripcion = reader["descripcion"] != DBNull.Value ? reader["descripcion"].ToString() : null;
                    tarea.Color = reader["color"] != DBNull.Value ? reader["color"].ToString() : null;
                    tarea.IdUsuarioAsignado = reader["id_usuario_asignado"] != DBNull.Value ? Convert.ToInt32(reader["id_usuario_asignado"]) : null;
                    tareas.Add(tarea);
                }
            }

            conexion.Close();
        }
        
        if (tareas == null)
        {
            throw new Exception("Lista de tareas vacía.");
        } else {
            return tareas;
        }
    }

    //Eliminar una tarea (recibe un IdTarea)
    public bool EliminarTarea(int idTarea)
    {
        var consulta = $"DELETE FROM Tarea WHERE id = @idTarea";
        int filasAfectadas;

        using (SqliteConnection conexion = new SqliteConnection(_cadenaConexion))
        {
            conexion.Open();
            var comando = new SqliteCommand(consulta, conexion);

            comando.Parameters.Add(new SqliteParameter("@idTarea", idTarea));

            filasAfectadas = comando.ExecuteNonQuery();
            conexion.Close();
        }

        if (filasAfectadas == 0)
        {
            return false;
        } else {
            return true;
        }
    }

    //Asignar Usuario a Tarea (recibe idUsuario y un idTarea)
    public void AsignarUsuarioATarea(int idUsuario, int idTarea)
    {
        var consulta = $"UPDATE Tarea SET id_usuario_asignado=@id_usuario_asignado WHERE id=@idTarea";
        int filasAfectadas;

        using (SqliteConnection conexion = new SqliteConnection(_cadenaConexion))
        {
            conexion.Open();

            var comando = new SqliteCommand(consulta, conexion);

            comando.Parameters.Add(new SqliteParameter("@id_usuario_asignado", idUsuario));
            comando.Parameters.Add(new SqliteParameter("@idTarea", idTarea));

            filasAfectadas = comando.ExecuteNonQuery();
            conexion.Close();
        }

        if (filasAfectadas == 0)
        {
            throw new Exception("Usuario no asignado.");
        }
    }
}