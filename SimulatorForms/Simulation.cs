using System;
using System.Drawing;
using System.Windows.Forms;
using FlightLib;

namespace SimulatorForms
{
    public partial class Simulation : Form
    {
        FlightPlan plan1;
        FlightPlan plan2;
        double distanciaSeguridad;
        double tiempoCiclo;

        bool alarmaActivada = false;

        public Simulation(FlightPlan plan1, FlightPlan plan2, double distanciaSeguridad, double tiempoCiclo)
        {
            InitializeComponent();
            
            this.plan1 = plan1;
            this.plan2 = plan2;
            this.distanciaSeguridad = distanciaSeguridad;
            this.tiempoCiclo = tiempoCiclo;
        }

        private void panelSimulacion_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            g.DrawRectangle(Pens.Black, 0, 0,
                panelSimulacion.Width - 1,
                panelSimulacion.Height - 1);

            float radio = (float)distanciaSeguridad;

            // Dibujo avión 1
            Position inicio1 = plan1.GetInitialPosition();
            Position final1 = plan1.GetFinalPosition();
            g.DrawLine(Pens.Blue, (float)inicio1.GetX(), (float)inicio1.GetY(), (float)final1.GetX(), (float)final1.GetY());

            Position p1 = plan1.GetCurrentPosition();
            float x1 = (float)p1.GetX();
            float y1 = (float)p1.GetY();
            g.DrawEllipse(Pens.Blue, x1 - radio, y1 - radio, radio * 2, radio * 2);
            g.FillEllipse(Brushes.Blue, x1 - 5, y1 - 5, 10, 10);

            // Dibujo avión 2
            Position inicio2 = plan2.GetInitialPosition();
            Position final2 = plan2.GetFinalPosition();
            g.DrawLine(Pens.Red, (float)inicio2.GetX(), (float)inicio2.GetY(), (float)final2.GetX(), (float)final2.GetY());

            Position p2 = plan2.GetCurrentPosition();
            float x2 = (float)p2.GetX();
            float y2 = (float)p2.GetY();
            g.DrawEllipse(Pens.Red, x2 - radio, y2 - radio, radio * 2, radio * 2);
            g.FillEllipse(Brushes.Red, x2 - 5, y2 - 5, 10, 10);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            plan1.Move(tiempoCiclo);
            plan2.Move(tiempoCiclo);
            panelSimulacion.Invalidate();
        }

        private void panelSimulacion_MouseClick(object sender, MouseEventArgs e)
        {
            float x = e.X;
            float y = e.Y;

            Position p1 = plan1.GetCurrentPosition();
            double distancia1 = Math.Sqrt((x - p1.GetX()) * (x - p1.GetX()) + (y - p1.GetY()) * (y - p1.GetY()));
            if (distancia1 <= 10)
            {
                MessageBox.Show(
                    "Identificador: " + plan1.GetId() +
                    "\nVelocidad: " + plan1.GetVelocidad() +
                    "\nPosición actual: (" +
                    plan1.GetCurrentPosition().GetX() + ", " +
                    plan1.GetCurrentPosition().GetY() + ")"
                );
            }

            Position p2 = plan2.GetCurrentPosition();
            double distancia2 = Math.Sqrt((x - p2.GetX()) * (x - p2.GetX()) + (y - p2.GetY()) * (y - p2.GetY()));
            if (distancia2 <= 10)
            {
                MessageBox.Show(
                    "Identificador: " + plan2.GetId() +
                    "\nVelocidad: " + plan2.GetVelocidad() +
                    "\nPosición actual: (" +
                    plan2.GetCurrentPosition().GetX() + ", " +
                    plan2.GetCurrentPosition().GetY() + ")"
                );
            }
        }

        private void buttonIniciar_Click(object sender, EventArgs e)
        {
            // FASE 11: abans de començar, mirem si hi haura conflicte
            if (HabraConflicto() == true)
            {
                DialogResult respuesta = MessageBox.Show(
                    "Los vuelos entrarán en conflicto.\n¿Quieres resolverlo cambiando automáticamente la velocidad de uno de los vuelos?",
                    "Conflicto detectado", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (respuesta == DialogResult.Yes)
                {
                    // Primero se intenta frenando el vuelo 1; si no funciona, frenando el vuelo 2
                    if (ResolverConflicto(plan1) == true)
                    {
                        MessageBox.Show("Conflicto resuelto. Nueva velocidad de " + plan1.GetId() + ": " + plan1.GetVelocidad().ToString("F2"));
                    }
                    else if (ResolverConflicto(plan2) == true)
                    {
                        MessageBox.Show("Conflicto resuelto. Nueva velocidad de " + plan2.GetId() + ": " + plan2.GetVelocidad().ToString("F2"));
                    }
                    else
                    {
                        MessageBox.Show("No se ha podido resolver el conflicto cambiando la velocidad. La simulación continuará con las velocidades originales.");
                    }
                }
                // Si responde que no, no se hace nada y la simulación continúa de forma habitual
            }
            timerSimulacion.Start();
            buttonIniciar.Enabled = false;
            buttonDetener.Enabled = true;
            buttonMover.Enabled = false;
        }

        private void buttonDetener_Click(object sender, EventArgs e)
        {
            timerSimulacion.Stop();
            buttonIniciar.Enabled = true;
            buttonDetener.Enabled = false;
            buttonMover.Enabled = true;
        }

        private void timerSimulacion_Tick(object sender, EventArgs e)
        {
            if (plan1.HasArrived() == false)
            {
                plan1.Move(tiempoCiclo);
            }
            if (plan2.HasArrived() == false)
            {
                plan2.Move(tiempoCiclo);
            }

            panelSimulacion.Invalidate();

            // FASE 10.1: Alarma de pérdida de separación
            if (plan1.Conflicto(plan2, distanciaSeguridad))
            {
                if (!alarmaActivada)
                {
                    alarmaActivada = true;
                    MessageBox.Show("¡Alarma! Los vuelos están a una distancia inferior a la de seguridad.", "Pérdida de separación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                alarmaActivada = false;
            }

            if (plan1.HasArrived() == true && plan2.HasArrived() == true)
            {
                timerSimulacion.Stop();
                buttonIniciar.Enabled = true;
                buttonDetener.Enabled = false;
                buttonMover.Enabled = true;
                MessageBox.Show("Los dos vuelos han llegado a su destino.");
            }
        }

        // FASE 9.1: Abrir el formulario con el DataGridView
        private void buttonVerDatos_Click(object sender, EventArgs e)
        {
            FlightDataForm f = new FlightDataForm(plan1, plan2);
            f.ShowDialog();
        }

        // FASE 10.2: Comprobar si entrarán en conflicto
        private void Conflicte_Click(object sender, EventArgs e)
        {
            if (HabraConflicto() == true)
            {
                MessageBox.Show("Los vuelos SÍ entrarán en conflicto.", "Comprobación de conflicto", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                MessageBox.Show("Los vuelos NO entrarán en conflicto.", "Comprobación de conflicto", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private bool HabraConflicto()
        {
            Position posOriginal1 = plan1.GetCurrentPosition();
            Position posOriginal2 = plan2.GetCurrentPosition();

            FlightPlan testPlan1 = new FlightPlan(plan1.GetId(), posOriginal1.GetX(), posOriginal1.GetY(), plan1.GetFinalPosition().GetX(), plan1.GetFinalPosition().GetY(), plan1.GetVelocidad());
            FlightPlan testPlan2 = new FlightPlan(plan2.GetId(), posOriginal2.GetX(), posOriginal2.GetY(), plan2.GetFinalPosition().GetX(), plan2.GetFinalPosition().GetY(), plan2.GetVelocidad());

            bool conflicteFutur = false;

            while ((testPlan1.HasArrived() == false || testPlan2.HasArrived() == false) && conflicteFutur == false)
            {
                // Se avanza en pasos de una décima de ciclo para no saltarse
                // un cruce que ocurra entre dos ciclos (si los aviones van rápido)
                if (testPlan1.HasArrived() == false)
                    testPlan1.Move(tiempoCiclo / 10);

                if (testPlan2.HasArrived() == false)
                    testPlan2.Move(tiempoCiclo / 10);
            }

            return conflicteFutur;
        }

        // FASE 11: Baixa mica en mica la velocitat del vol fins que ja no hi hagi conflicte, tornant true si no hi ha conflicte.
        // Si no es pot resoldre el conflicte, torna false i deixa la velocitat original.
        private bool ResolverConflicto(FlightPlan plan)
        {
            double velocidadOriginal = plan.GetVelocidad();
            int porcentaje = 99;
            bool resuelto = false;

            // El límit del 10% evita que sigui infinit
            while (resuelto == false && porcentaje >= 10)
            {
                plan.SetVelocidad(velocidadOriginal * porcentaje / 100.0);

                if (HabraConflicto() == false)
                {
                    resuelto = true;
                }
                else
                {
                    porcentaje = porcentaje - 1;
                }
            }

            if (resuelto == false)
            {
                plan.SetVelocidad(velocidadOriginal);
            }

            return resuelto;
        }

        private void buttonVerDatos_Click_1(object sender, EventArgs e)
        {
            FlightDataForm f = new FlightDataForm(plan1, plan2);
            f.ShowDialog();
        }
    }
}