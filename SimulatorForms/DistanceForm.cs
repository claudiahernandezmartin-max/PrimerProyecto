
using System;
using System.Drawing;
using System.Windows.Forms;
using FlightLib;

namespace SimulatorForms
{
    public class DistanceForm : Form
    {
        public DistanceForm(FlightPlanList lista, int seleccionado)
        {
            FlightPlan plan = lista.GetFlightPlan(seleccionado);

            Label etiquetaDistancia = new Label();
            etiquetaDistancia.AutoSize = true;
            etiquetaDistancia.Font = new Font("Segoe UI", 12);
            etiquetaDistancia.Location = new Point(28, 24);

            string texto = "Distancias desde " + plan.GetId() + ":\n\n";

            int i = 0;

            while (i < lista.GetNumber())
            {
                if (i != seleccionado)
                {
                    FlightPlan otro = lista.GetFlightPlan(i);

                    double distancia = plan.GetCurrentPosition().Distancia(
                        otro.GetCurrentPosition()
                    );

                    texto = texto + string.Format(
                        "  a {0}: {1:F2}\n",
                        otro.GetId(),
                        distancia
                    );
                }

                i++;
            }

            etiquetaDistancia.Text = texto;
            this.Controls.Add(etiquetaDistancia);

            Estilo estiloLocal = new Estilo();

            estiloLocal.PonerDialogo(this, "Distancias");

            etiquetaDistancia.ForeColor = estiloLocal.GetColorTexto();

            this.ClientSize = new Size(
                Math.Max(380, etiquetaDistancia.PreferredSize.Width + 56),
                etiquetaDistancia.PreferredSize.Height + 48
            );

            estiloLocal.PonerCabecera(
                this,
                "Distancias",
                "Vuelo seleccionado: " + plan.GetId(),
                true
            );
        }
    }
}
