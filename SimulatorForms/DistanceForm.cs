using System;
using System.Windows.Forms;
using FlightLib;

namespace SimulatorForms
{
    public partial class DistanceForm : Form
    {
        FlightPlan seleccionado;
        FlightPlan otro;

        public DistanceForm(FlightPlan seleccionado, FlightPlan otro)
        {
            InitializeComponent();

            this.seleccionado = seleccionado;
            this.otro = otro;

            // Calculem la distància entre les posicions actuals
            double dist = this.seleccionado.GetCurrentPosition().Distancia(this.otro.GetCurrentPosition());

            lblDistancia.Text = string.Format("Distancia entre {0} y {1}: {2:F2}",
                this.seleccionado.GetId(), this.otro.GetId(), dist);
        }

        public DistanceForm()
        {
            InitializeComponent();
        }
    }
}