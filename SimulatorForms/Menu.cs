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
            FlightPlan plan1 = lista.GetFlightPlan(0);
            FlightPlan plan2 = lista.GetFlightPlan(1);

            if (plan1 == null || plan2 == null)
            {
                MessageBox.Show("One or both flight plans are missing.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Simulation f = new Simulation(plan1, plan2);
            f.ShowDialog();
        }
    }
}
