using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.SQLite;
using System.Data;

namespace FlightLib
{
    public class GestionUsuarios
    {
        string fichero; // ruta del fichero de la base de datos
        public GestionUsuarios(string usuarios)
        {
            this.fichero = usuarios;
        }

        //Abrir connexión con la base de datos y devuelve la conexión abierta
        private SQLiteConnection Conectar()
        {
            SQLiteConnection conexion = new SQLiteConnection("Data Source=" + fichero + ";Version=3;");
            conexion.Open();
            return conexion;
        }
        public int Comprobar()
        {
            // SQLite crea un fichero vacío si no existe, por eso lo miramos antes
            if (!File.Exists(fichero))
            {
                //No existe el fichero de usuarios
                return -1;
            }

            int resultado;
            SQLiteConnection conexion = null;
            try
            {
                conexion = Conectar();
                //Selecciona, cuenta y lo hace desde la tabla de usuarios del fichero
                SQLiteCommand consulta = new SQLiteCommand("SELECT * FROM usuarios", conexion);
                SQLiteDataAdapter adaptador = new SQLiteDataAdapter(consulta);
                DataTable tabla = new DataTable();
                adaptador.Fill(tabla);
                resultado = 0;
            }
            catch (SQLiteException)
            {
                //Existe pero esta mal
                resultado = -2;
            }
            if (conexion != null)
            {
                conexion.Close();
            }
            return resultado;
        }

        // Comprueba si el usuario y la contraseña son correctos
        // Devuelve 1 si son correctos, 0 si no, -1 si no existe la base de datos y -2 si hay un error
        public int Login(string usuario, string password)
        {
            if (File.Exists(fichero) == false)
            {
                return -1;
            }

            int resultado;
            SQLiteConnection conexion = null;
            try
            {
                conexion = Conectar();

                string sql = "SELECT * FROM usuarios WHERE nombre = '" + usuario + "' AND password = '" + password + "'";
                SQLiteCommand consulta = new SQLiteCommand(sql, conexion);
                SQLiteDataAdapter adaptador = new SQLiteDataAdapter(consulta);
                DataTable tabla = new DataTable();
                adaptador.Fill(tabla);

                if (tabla.Rows.Count == 1)
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
            if (conexion != null)
            {
                conexion.Close();
            }
            return resultado;
        }

        // Registra un usuario nuevo
        // Devuelve 0 si se ha registrado, 1 si el usuario ya existe, -1 si no existe la base de datos y -2 si hay un error
        public int Registrar(string usuario, string password)
        {
            if (File.Exists(fichero) == false)
            {
                return -1;
            }

            int resultado;
            SQLiteConnection conexion = null;
            try
            {
                conexion = Conectar();

                // Buscamos si ya existe alguien con ese nombre
                string sql = "SELECT * FROM usuarios WHERE nombre = '" + usuario + "'";
                SQLiteCommand buscar = new SQLiteCommand(sql, conexion);
                SQLiteDataAdapter adaptador = new SQLiteDataAdapter(buscar);
                DataTable tabla = new DataTable();
                adaptador.Fill(tabla);

                if (tabla.Rows.Count > 0)
                {
                    resultado = 1;
                }
                else
                {
                    sql = "INSERT INTO usuarios (nombre, password) VALUES ('" + usuario + "', '" + password + "')";
                    SQLiteCommand insertar = new SQLiteCommand(sql, conexion);
                    insertar.ExecuteNonQuery();
                    resultado = 0;
                }
            }
            catch (SQLiteException)
            {
                resultado = -2;
            }
            if (conexion != null)
            {
                conexion.Close();
            }
            return resultado;
        }
    }
}
