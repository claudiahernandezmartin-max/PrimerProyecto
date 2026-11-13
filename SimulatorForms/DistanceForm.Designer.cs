using System;
using System.Drawing;
using System.Windows.Forms;
using FlightLib;

namespace SimulatorForms
{
    public partial class DistanceForm : Form
    {
        Estilo estilo = new Estilo();

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

            // Aspecto de la ventana: la ventana se ajusta al tamaño del texto
            estilo.PonerDialogo(this, "Distancias");
            lblDistancia.Font = new Font("Segoe UI", 12);
            lblDistancia.ForeColor = estilo.GetColorTexto();
            lblDistancia.Location = new Point(28, 24);
            this.ClientSize = new Size(Math.Max(380, lblDistancia.PreferredSize.Width + 56), lblDistancia.PreferredSize.Height + 48);
            estilo.PonerCabecera(this, "Distancias", "Vuelo seleccionado: " + plan.GetId(), true);
        }
    }
}