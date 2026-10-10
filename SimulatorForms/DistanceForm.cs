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
    public partial class DistanceForm : Form
    {
        FlightPlan seleccionado;
        FlightPlan otro;
        public DistanceForm()
        {
            InitializeComponent();

            double dist = seleccionado.GetCurrentPosition().Distancia(otro.GetCurrentPosition());

            lblDistancia.Text = string.Format("Distancia entre {0} y {1}: {2:F2}",
                seleccionado.GetId(), otro.GetId(), dist);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
