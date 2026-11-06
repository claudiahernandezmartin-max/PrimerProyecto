using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FlightLib;

namespace SimulatorForms
{
    public partial class LoginForm : Form
    {
        GestiónUsuarios gestor = new GestiónUsuarios("usuarios.db");
        bool aceptado = false;
        bool modoRegistro = false; // true cuando el usuario ha pulsado Registrarse
        public LoginForm()
        {
            InitializeComponent();
        }
        public bool GetAceptado()
        {
            return aceptado;
        }

        // Comprueba que se han escrito el usuario y la contraseña
        // Devuelve true si están los dos escritos
        private bool DatosEscritos()
        {
            if (textBoxUsuario.Text == "" || textBoxPassword.Text == "")
            {
                MessageBox.Show("Escribe el usuario y la contraseña.");
                return false;
            }
            return true;
        }

        // Al abrir el formulario comprueba que la base de datos existe y es correcta
        // Si no lo está, avisa y cierra el login (el programa termina)
        private void LoginForm_Load(object sender, EventArgs e)
        {
            int estado = gestor.Comprobar();
            if (estado == -1)
            {
                MessageBox.Show("No se encuentra el fichero usuarios.db. Se ha borrado o cambiado de nombre.");
                Close();
            }
            else if (estado == -2)
            {
                MessageBox.Show("El fichero usuarios.db está dañado o no tiene la tabla de usuarios.");
                Close();
            }
        }
        // Si está en modo registro, crea la cuenta y entra. Si no, comprueba usuario y contraseña
        private void buttonEntrar_Click(object sender, EventArgs e)
        {
            if (DatosEscritos() == false)
            {
                return;
            }

            if (modoRegistro == true)
            {
                int resultado = gestor.Registrar(textBoxUsuario.Text, textBoxPassword.Text);
                if (resultado == 0)
                {
                    MessageBox.Show("Usuario registrado. ¡Bienvenido!");
                    aceptado = true;
                    Close();
                }
                else if (resultado == 1)
                {
                    MessageBox.Show("Ese usuario ya existe. Elige otro nombre.");
                }
                else
                {
                    MessageBox.Show("Error con la base de datos.");
                }
            }
            else
            {
                int resultado = gestor.Login(textBoxUsuario.Text, textBoxPassword.Text);
                if (resultado == 1)
                {
                    aceptado = true;
                    Close();
                }
                else if (resultado == 0)
                {
                    MessageBox.Show("Usuario o contraseña incorrectos.");
                }
                else
                {
                    MessageBox.Show("Error con la base de datos.");
                }
            }
        }

        // Registra un usuario nuevo en la base de datos
        private void buttonRegistrar_Click(object sender, EventArgs e)
        {
            if (modoRegistro == false)
            {
                modoRegistro = true;
                buttonEntrar.Text = "Crear cuenta y entrar";
                buttonRegistrar.Text = "Volver";
            }
            else
            {
                modoRegistro = false;
                buttonEntrar.Text = "Entrar";
                buttonRegistrar.Text = "Registrarse";
            }
        }
    }
}
