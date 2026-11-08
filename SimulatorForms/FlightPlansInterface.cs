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

            // Si ya había vuelos guardados, los ponemos en las cajas para no tener que escribirlos otra vez
            if (lista.GetFlightPlan(0) != null)
            {
                PonerDatos(lista.GetFlightPlan(0), FP1_id, FP1_vel, FP1_xi, FP1_yi, FP1_xf, FP1_yf);
            }
            if (lista.GetFlightPlan(1) != null)
            {
                PonerDatos(lista.GetFlightPlan(1), FP2_id, FP2_vel, FP2_xi, FP2_yi, FP2_xf, FP2_yf);
            }
        }
        private void PonerDatos(FlightPlan p, TextBox id, TextBox vel, TextBox xi, TextBox yi, TextBox xf, TextBox yf)
        {
            id.Text = p.GetId();
            vel.Text = p.GetVelocidadInicial().ToString();
            xi.Text = p.GetInitialPosition().GetX().ToString();
            yi.Text = p.GetInitialPosition().GetY().ToString();
            xf.Text = p.GetFinalPosition().GetX().ToString();
            yf.Text = p.GetFinalPosition().GetY().ToString();
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
                string id1 = FP1_id.Text.Trim();
                double vel1 = Convert.ToDouble(FP1_vel.Text);
                double xi1 = Convert.ToDouble(FP1_xi.Text);
                double yi1 = Convert.ToDouble(FP1_yi.Text);
                double xf1 = Convert.ToDouble(FP1_xf.Text);
                double yf1 = Convert.ToDouble(FP1_yf.Text);

                // Avión 2: leer los datos
                string id2 = FP2_id.Text.Trim();
                double vel2 = Convert.ToDouble(FP2_vel.Text);
                double xi2 = Convert.ToDouble(FP2_xi.Text);
                double yi2 = Convert.ToDouble(FP2_yi.Text);
                double xf2 = Convert.ToDouble(FP2_xf.Text);
                double yf2 = Convert.ToDouble(FP2_yf.Text);

                // Comprobaciones ANTES de crear los vuelos
                if (vel1 <= 0 || vel2 <= 0)
                {
                    MessageBox.Show("La velocidad tiene que ser mayor que 0.");
                    return;
                }
                if (id1 == "" || id2 == "")
                {
                    MessageBox.Show("Los dos vuelos necesitan un identificador.");
                    return;
                }
                if (id1 == id2)
                {
                    MessageBox.Show("Los dos vuelos no pueden tener el mismo identificador.");
                    return;
                }

                // Ahora sí, creamos los vuelos
                FlightPlan plan1 = new FlightPlan(id1, xi1, yi1, xf1, yf1, vel1);
                FlightPlan plan2 = new FlightPlan(id2, xi2, yi2, xf2, yf2, vel2);

                // Quitamos los vuelos anteriores para que no se acumulen
                lista.Vaciar();

                // Añadir los dos a la lista y cerrar
                lista.AddFlightPlan(plan1);
                lista.AddFlightPlan(plan2);
                Close();
            }
            catch (FormatException)
            {
                MessageBox.Show("Hay algún dato mal escrito. Revisa que los números sean correctos.");
            }
            catch (OverflowException)
            {
                MessageBox.Show("Alguno de los números es demasiado grande.");
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Vuelo 1
            FP1_id.Text = "IB101";
            FP1_vel.Text = "100";
            FP1_xi.Text = "0";
            FP1_yi.Text = "0";
            FP1_xf.Text = "250";
            FP1_yf.Text = "250";

            // Vuelo 2 (se cruza con el 1 en el centro, así hay conflicto)
            FP2_id.Text = "VY202";
            FP2_vel.Text = "100";
            FP2_xi.Text = "0";
            FP2_yi.Text = "250";
            FP2_xf.Text = "250";
            FP2_yf.Text = "0";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            lista.Vaciar();
            lista.AddFlightPlan(new FlightPlan("AV1", 0, 0, 250, 250, 100));
            lista.AddFlightPlan(new FlightPlan("AV2", 0, 250, 250, 0, 100));
            lista.AddFlightPlan(new FlightPlan("AV3", 0, 125, 250, 125, 90));
            lista.AddFlightPlan(new FlightPlan("AV4", 125, 0, 125, 250, 90));
            lista.AddFlightPlan(new FlightPlan("AV5", 0, 60, 250, 190, 95));
            lista.AddFlightPlan(new FlightPlan("AV6", 250, 60, 0, 190, 95));
            lista.AddFlightPlan(new FlightPlan("AV7", 60, 0, 190, 250, 85));
            lista.AddFlightPlan(new FlightPlan("AV8", 190, 0, 60, 250, 85));
            Close();
        }
    }
}
