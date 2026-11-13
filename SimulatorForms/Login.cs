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
    public partial class Login : Form
    {
        GestiónUsuarios gestor = new GestiónUsuarios("Todos los documentos\\usuarios.db");
        bool aceptado = false;
        Estilo estilo = new Estilo();
        public Login()
        {
            InitializeComponent();
            estilo.PonerFormulario(this);
            estilo.PonerEtiqueta(labelUsuario);
            estilo.PonerEtiqueta(labelPassword);
            estilo.PonerCaja(textBoxUsuario);
            estilo.PonerCaja(textBoxPassword);
            estilo.PonerBotonPrincipal(buttonEntrar);
            estilo.PonerBotonSecundario(buttonRegistrar);
            this.Text = "Deconflicting Tool · Iniciar sesión";
            this.AcceptButton = buttonEntrar;   // con la tecla Enter se pulsa Entrar
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

        // Al abrir el formulario comprueba que la base de datos existe y es correct. Si no lo está, avisa y cierra el login (el programa termina)
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
        // Botón Entrar: comprueba el usuario y la contraseña en la base de datos
        private void buttonEntrar_Click(object sender, EventArgs e)
        {
            if (DatosEscritos() == false)
            {
                return;
            }

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

        // Si el usuario se registra bien, entra directamente al Menu.
        private void buttonRegistrar_Click(object sender, EventArgs e)
        {
            Registro registro = new Registro(gestor);
            registro.ShowDialog();

            if (registro.GetRegistrado() == true)
            {
                aceptado = true;
                Close();
            }
        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            estilo.DibujarCabecera(e.Graphics, panelCabecera.Width, panelCabecera.Height, "Deconflicting Tool", "Inicia sesión para entrar en la torre de control");
        }
    }
}
