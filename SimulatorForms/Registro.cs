using FlightLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimulatorForms
{
    // Formulario para crear un usuario nuevo en la base de datos
    public partial class Registro : Form
    {
        GestiónUsuarios gestor;   // el mismo gestor que usa el login
        
        // Constructor. Recibe el gestor de usuarios del login
        bool registrado = false;
        Estilo estilo = new Estilo();
        public Registro(GestiónUsuarios gestor)
        {
            InitializeComponent();
            this.gestor = gestor;

            //Aspecto de la ventana
            estilo.PonerFormulario(this);
            estilo.PonerEtiqueta(labelUsuario);
            estilo.PonerEtiqueta(labelPassword);
            estilo.PonerEtiqueta(labelRepetir);
            estilo.PonerCaja(textBoxUsuario);
            estilo.PonerCaja(textBoxPassword);
            estilo.PonerCaja(textBoxRepetir);
            estilo.PonerBotonPrincipal(buttonCrear);
            estilo.PonerBotonSecundario(buttonCancelar);
            this.Text = "Deconflicting Tool · Crear cuenta";
            this.AcceptButton = buttonCrear;
        }
        // Devuelve true si se ha creado el usuario correctamente
        public bool GetRegistrado()
        {
            return registrado;
        }

        // Botón Crear cuenta: comprueba los datos y guarda el usuario nuevo
        private void Registro_Load(object sender, EventArgs e)
        {

        }
        // Botón Crear cuenta: comprueba los datos y guarda el usuario nuevo
        private void buttonCrear_Click(object sender, EventArgs e)
        {
            if (textBoxUsuario.Text == "" || textBoxPassword.Text == "")
            {
                MessageBox.Show("Escribe el usuario y la contraseña.");
                return;
            }
            if (textBoxPassword.Text != textBoxRepetir.Text)
            {
                MessageBox.Show("Las dos contraseñas no son iguales.");
                return;
            }

            int resultado = gestor.Registrar(textBoxUsuario.Text, textBoxPassword.Text);
            if (resultado == 0)
            {
                MessageBox.Show("Usuario registrado. ¡Bienvenido!");
                registrado = true;
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
        private void buttonCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void panelCabecera_Paint(object sender, PaintEventArgs e)
        {
            estilo.DibujarCabecera(e.Graphics, panelCabecera.Width, panelCabecera.Height, "Crear cuenta", "Únete a la torre de control");
        }
    }
}
