using System;
using System.Windows.Forms;
using FlightLib;

namespace SimulatorForms
{
    // Formulario para crear un usuario nuevo en la base de datos
    public partial class RegistroForm : Form
    {
        GestiónUsuarios gestor;   // el mismo gestor que usa el login
        bool registrado = false;

        // Constructor. Recibe el gestor de usuarios del login
        public RegistroForm(GestiónUsuarios gestor)
        {
            InitializeComponent();
            this.gestor = gestor;
        }

        // Devuelve true si se ha creado el usuario correctamente
        public bool GetRegistrado()
        {
            return registrado;
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

        // Botón Cancelar: vuelve al login sin registrar a nadie
        private void buttonCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
