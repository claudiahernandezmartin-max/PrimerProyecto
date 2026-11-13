using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using FlightLib;

namespace SimulatorForms
{
    public partial class FlightDataForm : Form
    {
        FlightPlanList lista;
        Estilo estilo = new Estilo();

        public FlightDataForm(FlightPlanList lista)
        {
            InitializeComponent();
            AplicarEstilo();

            this.lista = lista;

            CargarDatos();
        }

        public FlightDataForm()
        {
            InitializeComponent();
            AplicarEstilo();
        }

        // Aspecto de la ventana: cabecera y tabla con los colores del programa
        private void AplicarEstilo()
        {
            estilo.PonerVentana(this, "Datos de vuelo");
            this.ClientSize = new Size(780, 440);
            estilo.PonerTabla(dataGridViewVuelos);
            estilo.PonerCabecera(this, "Datos de vuelo", "Pulsa un vuelo para ver sus distancias", false);
        }

        private void CargarDatos()
        {
            DataTable tabla = new DataTable();
            tabla.Columns.Add("Identificador");
            tabla.Columns.Add("Compañía");
            tabla.Columns.Add("Velocidad");
            tabla.Columns.Add("Posición Actual");
            tabla.Columns.Add("Ha llegado");

            int i = 0;
            while (i < lista.GetNumber())
            {
                FlightPlan plan = lista.GetFlightPlan(i);
                string llegado = "No";
                if (plan.HasArrived())
                {
                    llegado = "Sí";
                }
                tabla.Rows.Add(
                    plan.GetId(),
                    plan.GetCompania(),
                    plan.GetVelocidad().ToString("F2"),
                    string.Format("({0:F2}, {1:F2})", plan.GetCurrentPosition().GetX(), plan.GetCurrentPosition().GetY()),
                    llegado
                );
                i++;
            }
            //Hem afegit el metode GetCompania per mostrar la companya a la taula de vols

            dataGridViewVuelos.DataSource = tabla;
            dataGridViewVuelos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewVuelos.ReadOnly = true;
            dataGridViewVuelos.AllowUserToAddRows = false;

            dataGridViewVuelos.CellClick += DataGridViewVuelos_CellClick;
        }

        private void DataGridViewVuelos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < lista.GetNumber())
            {
                DistanceForm df = new DistanceForm(lista, e.RowIndex);
                df.ShowDialog();
            }
        }

        // Mètode afegit per satisfer l'esdeveniment creat pel dissenyador
        private void dataGridViewVols_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }
    }
}