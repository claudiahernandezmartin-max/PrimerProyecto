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
    public partial class Menu : Form
    {
        FlightPlanList lista = new FlightPlanList();

        // FASE 2.3: valores por defecto si el usuario no los cambia
        double distanciaSeguridad = 15;
        double tiempoCiclo = 2;
        bool reiniciar = true; // false justo después de cargar un fichero, para seguir desde donde se guardó

        public Menu()
        {
            InitializeComponent();
        }

        private void planesDeVueloToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FlightPlansInterface f = new FlightPlansInterface();
            f.SetLista(lista);
            f.ShowDialog();

        }

        private void simulacionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Hacen falta al menos 2 vuelos (pueden ser más, por ejemplo los 8 de prueba)
            if (lista.GetNumber() < 2)
            {
                MessageBox.Show("Primero hay que introducir al menos dos planes de vuelo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Los vuelos empiezan desde su posición inicial, menos si se acaba de cargar un fichero
            if (reiniciar)
            {
                lista.Restart();
            }
            reiniciar = true;

            // Ahora se pasa la lista entera en vez de plan1 y plan2
            Simulation f = new Simulation(lista, distanciaSeguridad, tiempoCiclo);
            f.ShowDialog();
        }

        private void distanciaDeSeguridadYTiempoDeCicloToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ParametrosForm f = new ParametrosForm();
            f.SetDatos(distanciaSeguridad, tiempoCiclo);
            f.ShowDialog();

            if (f.GetAceptado() == true)
            {
                distanciaSeguridad = f.GetDistancia();
                tiempoCiclo = f.GetCiclo();
            }
        }

        // Elige el fichero de vuelos con un OpenFileDialog y lo carga en la lista
        private void cargarPlanesDesdeFicheroToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialogo = new OpenFileDialog();

            if (dialogo.ShowDialog() == DialogResult.OK)
            {
                int resultado = lista.CargarFichero(dialogo.FileName);

                if (resultado == -1)
                {
                    MessageBox.Show("El fichero no existe.");
                }
                else if (resultado == -2)
                {
                    MessageBox.Show("Hay datos mal escritos o que faltan en el fichero.");
                }
                else if (resultado == -3)
                {
                    MessageBox.Show("Alguna velocidad del fichero no es mayor que 0.");
                }
                else if (lista.GetNumber() == 0)
                {
                    MessageBox.Show("El fichero está vacío.");
                }
                else
                {
                    reiniciar = false; // la simulación seguirá desde la posición guardada
                    MessageBox.Show("Se han cargado " + lista.GetNumber() + " vuelos.");
                }
            }
        }
    }
}