using Microsoft.Data.Sqlite;
using Kanban.Models;

namespace Kanban.Repositories;

public class UsuarioRepository: IUsuarioRepository{
    private readonly string _cadenaConexion;

    public UsuarioRepository(string cadenaConexion){
        _cadenaConexion = cadenaConexion;
    }

//Crear un nuevo usuario. (recibe un objeto Usuario)
    public void CrearUsuario(Usuario usuario){
        var consulta = $"INSERT INTO Usuario (nombre_de_usuario,password,rolusuario) VALUES (@nombre_de_usuario,@password,@rolusuario)";

        int filasAfectadas;
        
        using (SqliteConnection conexion = new SqliteConnection(_cadenaConexion)){
            conexion.Open();
            var comando = new SqliteCommand(consulta,conexion);
            
            comando.Parameters.Add(new SqliteParameter("@nombre_de_usuario", usuario.NombreDeUsuario));
            comando.Parameters.Add(new SqliteParameter("@password", usuario.Password));
            comando.Parameters.Add(new SqliteParameter("@rolusuario", usuario.RolUsuario));

            filasAfectadas = comando.ExecuteNonQuery();
            conexion.Close();
        }

        if (filasAfectadas == 0)
        {
            throw new Exception("Usuario no creado.");
        }
    }

//Modificar un usuario existente. (recibe un Id y un objeto Usuario)
    public void ModificarUsuario(int id, Usuario usuario){
        var consulta = $"UPDATE Usuario SET nombre_de_usuario=@nombre_de_usuario, password=@password, rolusuario=@rolusuario WHERE id=@id";

        int filasAfectadas;
        
        using (SqliteConnection conexion = new SqliteConnection(_cadenaConexion)){
            conexion.Open();
            var comando = new SqliteCommand(consulta,conexion);
            
            comando.Parameters.Add(new SqliteParameter("@nombre_de_usuario", usuario.NombreDeUsuario));
            comando.Parameters.Add(new SqliteParameter("@password", usuario.Password));
            comando.Parameters.Add(new SqliteParameter("@rolusuario", usuario.RolUsuario));
            comando.Parameters.Add(new SqliteParameter("@id", id));

            filasAfectadas = comando.ExecuteNonQuery();
            conexion.Close();
        }

        if (filasAfectadas == 0)
        {
            throw new Exception("Usuario no modificado.");
        }
    }

//Listar todos los usuarios registrados. (devuelve un List de Usuarios)
    public List<Usuario> ListarUsuarios(){
        var consulta = @"SELECT * FROM Usuario";
        List<Usuario> usuarios = null;
        
        using (SqliteConnection conexion = new SqliteConnection(_cadenaConexion)){
            conexion.Open();
            var comando = new SqliteCommand(consulta,conexion);

            using(SqliteDataReader reader = comando.ExecuteReader()){
                usuarios = new List<Usuario>();
                while (reader.Read()){
                    var usuario = new Usuario();
                    usuario.Id = Convert.ToInt32(reader["id"]);
                    usuario.NombreDeUsuario = reader["nombre_de_usuario"].ToString();
                    usuario.Password = reader["password"].ToString();
                    usuario.RolUsuario = (RolUsuario)Convert.ToInt32(reader["rolusuario"]);
                    usuarios.Add(usuario);
                }
            }

            conexion.Close();
        }

        if (usuarios == null)
        {
            throw new Exception("Lista de usuarios vacía.");
        } else {
            return usuarios;
        }
    }

//Obtener detalles de un usuario por su ID. (recibe un Id y devuelve un Usuario)
    public Usuario ObtenerUsuario(int id){
        var consulta = $"SELECT * FROM Usuario WHERE id=@id";
        Usuario usuario = null;
        
        using (SqliteConnection conexion = new SqliteConnection(_cadenaConexion)){
            conexion.Open();
            var comando = new SqliteCommand(consulta,conexion);

            comando.Parameters.Add(new SqliteParameter("@id", id));

            using(SqliteDataReader reader = comando.ExecuteReader()){
                while (reader.Read()){
                    usuario = new Usuario();
                    usuario.Id = Convert.ToInt32(reader["id"]);
                    usuario.NombreDeUsuario = reader["nombre_de_usuario"].ToString();
                    usuario.Password = reader["password"].ToString();
                    usuario.RolUsuario = (RolUsuario)Convert.ToInt32(reader["rolusuario"]);
                }
            }

            conexion.Close();
        }

        if (usuario == null)
        {
            throw new Exception("Usuario no encontrado.");
        } else {
            return usuario;
        }
    }

//Obtener usuario por nombre y contraseña
public Usuario ObtenerUsuarioAutenticacion(string nombre, string password){
        var consulta = $"SELECT * FROM Usuario WHERE nombre_de_usuario=@nombre_de_usuario AND password=@password";
        Usuario usuario = null;
        
        using (SqliteConnection conexion = new SqliteConnection(_cadenaConexion)){
            conexion.Open();
            var comando = new SqliteCommand(consulta,conexion);

            comando.Parameters.Add(new SqliteParameter("@nombre_de_usuario", nombre));
            comando.Parameters.Add(new SqliteParameter("@password", password));

            using(SqliteDataReader reader = comando.ExecuteReader()){
                if (reader.Read()){
                    usuario = new Usuario();
                    usuario.Id = Convert.ToInt32(reader["id"]);
                    usuario.NombreDeUsuario = reader["nombre_de_usuario"].ToString();
                    usuario.Password = reader["password"].ToString();
                    usuario.RolUsuario = (RolUsuario)Convert.ToInt32(reader["rolusuario"]);
                }
            }

            conexion.Close();
        }

        if (usuario == null)
        {
            throw new Exception("Usuario no encontrado.");
        } else {
            return usuario;
        }
}

//Eliminar un usuario por ID (solo si el usuario no esta relacionado a ninguna tarea y/o tablero)
    public bool EliminarUsuario(int id){
        var consulta = $"DELETE FROM Usuario WHERE id = @id AND NOT EXISTS (SELECT 1 FROM Tarea WHERE id_usuario_asignado = @id) AND NOT EXISTS (SELECT 1 FROM Tablero WHERE id_usuario_propietario = @id)";
        int filasAfectadas;

        using (SqliteConnection conexion = new SqliteConnection(_cadenaConexion)){
            conexion.Open();
            var comando = new SqliteCommand(consulta,conexion);

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

//Cambio de Password
    public void CambiarPassword(int id, string passwordNueva){
        var consulta = $"UPDATE Usuario SET password=@password WHERE id=@id";
        int filasAfectadas;
        
        using (SqliteConnection conexion = new SqliteConnection(_cadenaConexion)){
            conexion.Open();
            var comando = new SqliteCommand(consulta,conexion);
            
            comando.Parameters.Add(new SqliteParameter("@password", passwordNueva));
            comando.Parameters.Add(new SqliteParameter("@id", id));

            filasAfectadas = comando.ExecuteNonQuery();
            conexion.Close();
        }

        if (filasAfectadas == 0)
        {
            throw new Exception("Contraseña no modificada.");
        }
    }
}