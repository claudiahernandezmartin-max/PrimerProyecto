using System;
using System.Windows.Forms;
using FlightLib;

namespace SimulatorForms
{
    public partial class DistanceForm : Form
    {
        public DistanceForm(FlightPlanList lista, int seleccionado)
        {
            InitializeComponent();

            FlightPlan plan = lista.GetFlightPlan(seleccionado);

            // Distancia del vuelo elegido a cada uno de los demás
            string texto = "Distancias desde " + plan.GetId() + ":\n\n";
            int i = 0;
            while (i < lista.GetNumber())
            {
                if (i != seleccionado)
                {
                    FlightPlan otro = lista.GetFlightPlan(i);
                    double dist = plan.GetCurrentPosition().Distancia(otro.GetCurrentPosition());
                    texto = texto + string.Format("  a {0}: {1:F2}\n", otro.GetId(), dist);
                }
                i++;
            }

            lblDistancia.Text = texto;
        }
    }
}