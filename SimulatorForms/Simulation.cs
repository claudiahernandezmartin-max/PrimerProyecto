using System;
using System.Data;
using System.Windows.Forms;
using FlightLib;

namespace SimulatorForms
{
    public partial class FlightDataForm : Form
    {
        // Atributs per guardar els dos plans de vol
        FlightPlan plan1;
        FlightPlan plan2;

        // Constructor principal que rep els dos vols com a arguments
        public FlightDataForm(FlightPlan plan1, FlightPlan plan2)
        {
            InitializeComponent();

            this.plan1 = plan1;
            this.plan2 = plan2;

            CargarDatos();
        }

        // Constructor secundari buit per evitar conflictes amb el dissenyador de Visual Studio
        public FlightDataForm()
        {
            InitializeComponent();
        }

        private void CargarDatos()
        {
            // Creem una taula per estructurar les dades dels vols
            DataTable tabla = new DataTable();
            tabla.Columns.Add("Identificador");
            tabla.Columns.Add("Velocidad");
            tabla.Columns.Add("Posición Actual");
            tabla.Columns.Add("Ha llegado");

            if (plan1 != null)
            {
                tabla.Rows.Add(
                    plan1.GetId(),
                    plan1.GetVelocidad().ToString("F2"),
                    string.Format("({0:F2}, {1:F2})", plan1.GetCurrentPosition().GetX(), plan1.GetCurrentPosition().GetY()),
                    plan1.HasArrived() ? "Sí" : "No"
                );
            }

            if (plan2 != null)
            {
                tabla.Rows.Add(
                    plan2.GetId(),
                    plan2.GetVelocidad().ToString("F2"),
                    string.Format("({0:F2}, {1:F2})", plan2.GetCurrentPosition().GetX(), plan2.GetCurrentPosition().GetY()),
                    plan2.HasArrived() ? "Sí" : "No"
                );
            }

            // Enllacem la taula al DataGridView
            dataGridViewVuelos.DataSource = tabla;
            dataGridViewVuelos.ReadOnly = true;
            dataGridViewVuelos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // Connectem l'esdeveniment de clic a la fila
            dataGridViewVuelos.CellClick += DataGridViewVuelos_CellClick;
        }

        private void DataGridViewVuelos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Verifiquem que s'hagi clicat sobre una fila vàlida (no sobre la capçalera de columnes)
            if (e.RowIndex >= 0)
            {
                if (e.RowIndex == 0)
                {
                    DistanceForm df = new DistanceForm(plan1, plan2);
                    df.ShowDialog();
                }
                else if (e.RowIndex == 1)
                {
                    DistanceForm df = new DistanceForm(plan2, plan1);
                    df.ShowDialog();
                }
            }
        }
    }
}