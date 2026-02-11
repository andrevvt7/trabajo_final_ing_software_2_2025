using Microsoft.Data.Sqlite;
using Kanban.Models;
namespace Kanban.Repositories;

public class TableroRepository : ITableroRepository
{
    private readonly string _cadenaConexion;

    public TableroRepository(string cadenaConexion)
    {
        _cadenaConexion = cadenaConexion;
    }

    //Crear un nuevo tablero (devuelve un objeto Tablero)
    public void CrearTablero(Tablero tablero)
    {
        var consulta = $"INSERT INTO Tablero (id_usuario_propietario,nombre,descripcion) VALUES (@id_usuario_propietario,@nombre,@descripcion)";
        int filasAfectadas;

        using (SqliteConnection conexion = new SqliteConnection(_cadenaConexion))
        {
            conexion.Open();

            var comando = new SqliteCommand(consulta, conexion);

            comando.Parameters.Add(new SqliteParameter("@id_usuario_propietario", tablero.IdUsuarioPropietario));
            comando.Parameters.Add(new SqliteParameter("@nombre", tablero.Nombre));
            comando.Parameters.Add(new SqliteParameter("@descripcion", (object?)tablero.Descripcion ?? DBNull.Value));

            filasAfectadas = comando.ExecuteNonQuery();
            conexion.Close();
        }

        if (filasAfectadas == 0)
        {
            throw new Exception("Tablero no creado.");
        }
    }
    //Modificar un tablero existente (recibe un id y un objeto Tablero)
    public void ModificarTablero(int id, Tablero tablero)
    {
        var consulta = $"UPDATE Tablero SET nombre=@nombre,descripcion=@descripcion WHERE id=@id";
        int filasAfectadas;

        using (SqliteConnection conexion = new SqliteConnection(_cadenaConexion))
        {
            conexion.Open();

            var comando = new SqliteCommand(consulta, conexion);

            comando.Parameters.Add(new SqliteParameter("@nombre", tablero.Nombre));
            comando.Parameters.Add(new SqliteParameter("@descripcion", (object?)tablero.Descripcion ?? DBNull.Value));
            comando.Parameters.Add(new SqliteParameter("@id", id));

            filasAfectadas = comando.ExecuteNonQuery();
            conexion.Close();
        }

        if (filasAfectadas == 0)
        {
            throw new Exception("Tablero no modificado.");
        }
    }

    //Obtener detalles de un tablero por su ID. (recibe un id y devuelve un Tablero)
    public Tablero ObtenerTablero(int id)
    {
        var consulta = $"SELECT * FROM Tablero WHERE id=@id";
        Tablero tablero = null;

        using (SqliteConnection conexion = new SqliteConnection(_cadenaConexion))
        {
            conexion.Open();

            var comando = new SqliteCommand(consulta, conexion);

            comando.Parameters.Add(new SqliteParameter("@id", id));

            using (SqliteDataReader reader = comando.ExecuteReader())
            {
                while (reader.Read())
                {
                    tablero = new Tablero();
                    tablero.Id = Convert.ToInt32(reader["id"]);
                    tablero.IdUsuarioPropietario = Convert.ToInt32(reader["id_usuario_propietario"]);
                    tablero.Nombre = reader["nombre"].ToString();
                    tablero.Descripcion = reader["descripcion"] is DBNull ? null : reader["descripcion"].ToString();
                }
            }

            conexion.Close();
        }
        
        if (tablero == null)
        {
            throw new Exception("Tablero no encontrado.");
        } else {
            return tablero;
        }
    }

    //Listar todos los tableros existentes (devuelve un list de tableros)
    public List<Tablero> ListarTableros()
    {
        var consulta = @"SELECT * FROM Tablero";
        List<Tablero> tableros = null;

        using (SqliteConnection conexion = new SqliteConnection(_cadenaConexion))
        {
            conexion.Open();

            var comando = new SqliteCommand(consulta, conexion);

            using (SqliteDataReader reader = comando.ExecuteReader())
            {
                tableros = new List<Tablero>();
                while (reader.Read())
                {
                    var tablero = new Tablero();
                    tablero.Id = Convert.ToInt32(reader["id"]);
                    tablero.IdUsuarioPropietario = Convert.ToInt32(reader["id_usuario_propietario"]);
                    tablero.Nombre = reader["nombre"].ToString();
                    tablero.Descripcion = reader["descripcion"] is DBNull ? null : reader["descripcion"].ToString();
                    tableros.Add(tablero);
                }
            }

            conexion.Close();
        }
        
        if (tableros == null)
        {
            throw new Exception("Lista de tableros vacía.");
        } else {
            return tableros;
        }
    }

    //Listar todos los tableros de un usuario específico. (recibe un IdUsuario, devuelve un list de tableros)
    public List<Tablero> ListarTablerosDeUnUsuario(int idUsuario)
    {
        var consulta = @"SELECT DISTINCT Tablero.* FROM Tablero WHERE id_usuario_propietario = @idUsuario UNION SELECT DISTINCT Tablero.* FROM Tablero INNER JOIN Tarea ON Tablero.id = Tarea.id_tablero WHERE Tarea.id_usuario_asignado = @idUsuario";
        List<Tablero> tableros = null;

        using (SqliteConnection conexion = new SqliteConnection(_cadenaConexion))
        {
            conexion.Open();

            var comando = new SqliteCommand(consulta, conexion);

            comando.Parameters.Add(new SqliteParameter("@idUsuario", idUsuario));

            using (SqliteDataReader reader = comando.ExecuteReader())
            {
                tableros = new List<Tablero>();
                while (reader.Read())
                {
                    var tablero = new Tablero();
                    tablero.Id = Convert.ToInt32(reader["id"]);
                    tablero.IdUsuarioPropietario = Convert.ToInt32(reader["id_usuario_propietario"]);
                    tablero.Nombre = reader["nombre"].ToString();
                    tablero.Descripcion = reader["descripcion"] is DBNull ? null : reader["descripcion"].ToString();
                    tableros.Add(tablero);
                }
            }

            conexion.Close();
        }
        
        if (tableros == null)
        {
            throw new Exception("Lista de tableros vacía.");
        } else {
            return tableros;
        }
    }

    //Eliminar un tablero por ID (solo si no tiene tareas cargadas)
    public bool EliminarTablero(int id)
    {
        var consulta = $"DELETE FROM Tablero WHERE id = @id AND NOT EXISTS (SELECT 1 FROM Tarea WHERE id_tablero = Tablero.id)";
        int filasAfectadas;

        using (SqliteConnection conexion = new SqliteConnection(_cadenaConexion))
        {
            conexion.Open();
            var comando = new SqliteCommand(consulta, conexion);

            comando.Parameters.Add(new SqliteParameter("@id", id));

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
}