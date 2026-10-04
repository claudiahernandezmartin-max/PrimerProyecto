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
    public partial class FlightPlansInterface : Form
    {
        FlightPlanList lista;
        public void SetLista(FlightPlanList lista)
        {
            this.lista = lista;
        }

        public FlightPlansInterface()
        {
            InitializeComponent();
        }

        private void AcceptButtonFlightplans_Click(object sender, EventArgs e)
        {
            try
            {
                // Avión 1: leer los datos
                string id1 = FP1_id.Text;
                double vel1 = Convert.ToDouble(FP1_vel.Text);
                double xi1 = Convert.ToDouble(FP1_xi.Text);
                double yi1 = Convert.ToDouble(FP1_yi.Text);
                double xf1 = Convert.ToDouble(FP1_xf.Text);
                double yf1 = Convert.ToDouble(FP1_yf.Text);
                FlightPlan plan1 = new FlightPlan(id1, xi1, yi1, xf1, yf1, vel1);

                // Avión 2: leer los datos
                string id2 = FP2_id.Text;
                double vel2 = Convert.ToDouble(FP2_vel.Text);
                double xi2 = Convert.ToDouble(FP2_xi.Text);
                double yi2 = Convert.ToDouble(FP2_yi.Text);
                double xf2 = Convert.ToDouble(FP2_xf.Text);
                double yf2 = Convert.ToDouble(FP2_yf.Text);
                FlightPlan plan2 = new FlightPlan(id2, xi2, yi2, xf2, yf2, vel2);

                // Añadir los dos a la lista y cerrar
                lista.AddFlightPlan(plan1);
                lista.AddFlightPlan(plan2);
                Close();
            }
            catch (FormatException)
            {
                MessageBox.Show("Hay algún dato mal escrito. Revisa que los números sean correctos.");
            }
        }
    }
}
