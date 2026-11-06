using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.SQLite;

namespace FlightLib
{
    public class GestiónUsuarios
    {
        string usuarios;
        public GestiónUsuarios(string usuarios)
        {
            this.usuarios = usuarios;
        }
        //Abrir connexión con la base de datos
        private SQLiteConnection Conectar()
        {
            SQLiteConnection conexion = new SQLiteConnection("Data Source=" + usuarios + ";Version=3;");
            conexion.Open();
            return conexion;
        }
        public int Comprobar()
        {
            // SQLite crea un fichero vacío si no existe, por eso lo miramos antes
            if (!File.Exists(usuarios))
            {
                //No existe el fichero de usuarios
                return -1;
            }

            int resultado;
            SQLiteConnection conexión = null;
            try
            {
                conexión = Conectar();
                //Selecciona, cuenta y lo hace desde la tabla de usuarios. 
                SQLiteCommand consulta = new SQLiteCommand("SELECT COUNT(*) FROM usuarios", conexión);
                consulta.ExecuteScalar();
                resultado = 0;
            }
            catch (SQLiteException)
            {
                //Existe pero esta mal
                resultado = -2;
            }
            if (conexión != null)
            {
                conexión.Close();
            }
            return resultado;
        }

        // Comprueba si el usuario y la contraseña son correctos
        // Devuelve 1 si son correctos, 0 si no, -1 si no existe la base de datos, -2 si hay un error
        public int Login(string usuario, string password)
        {
            if (!File.Exists(usuarios))
            {
                return -1;
            }

            int resultado;
            SQLiteConnection conexión = null;
            try
            {
                conexión = Conectar();
                SQLiteCommand consulta = new SQLiteCommand("SELECT COUNT(*) FROM usuarios WHERE nombre = @nombre AND password = @password", conexión);
                consulta.Parameters.AddWithValue("@nombre", usuario);
                consulta.Parameters.AddWithValue("@password", password);

                if (Convert.ToInt32(consulta.ExecuteScalar()) == 1)
                {
                    resultado = 1;
                }
                else
                {
                    resultado = 0;
                }
            }
            catch (SQLiteException)
            {
                resultado = -2;
            }
            if (conexión != null)
            {
                conexión.Close();
            }
            return resultado;
        }

        // Registra un usuario nuevo
        // Devuelve 0 si se ha registrado, 1 si el usuario ya existe, -1 si no existe la base de datos, -2 si hay un error
        public int Registrar(string usuario, string password)
        {
            if (!File.Exists(usuarios))
            {
                return -1;
            }

            int resultado;
            SQLiteConnection conexión = null;
            try
            {
                conexión = Conectar();

                // Miramos si ya existe alguien con ese nombre
                SQLiteCommand buscar = new SQLiteCommand("SELECT COUNT(*) FROM usuarios WHERE nombre = @nombre", conexión);
                buscar.Parameters.AddWithValue("@nombre", usuario);

                if (Convert.ToInt32(buscar.ExecuteScalar()) > 0)
                {
                    resultado = 1;
                }
                else
                {
                    SQLiteCommand insertar = new SQLiteCommand("INSERT INTO usuarios (nombre, password) VALUES (@nombre, @password)", conexión);
                    insertar.Parameters.AddWithValue("@nombre", usuario);
                    insertar.Parameters.AddWithValue("@password", password);
                    insertar.ExecuteNonQuery();
                    resultado = 0;
                }
            }
            catch (SQLiteException)
            {
                resultado = -2;
            }
            if (conexión != null)
            {
                conexión.Close();
            }
            return resultado;
        }
    }
}
