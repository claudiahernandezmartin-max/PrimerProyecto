using System;
using System.Data;
using System.Windows.Forms;
using FlightLib;

namespace SimulatorForms
{
    public partial class FlightDataForm : Form
    {
        FlightPlan plan1;
        FlightPlanList lista;

        public FlightDataForm(FlightPlanList lista)
        {
            InitializeComponent();

            this.lista = lista;

            CargarDatos();
        }

        public FlightDataForm()
        {
            InitializeComponent();
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