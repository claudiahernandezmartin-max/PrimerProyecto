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
    public partial class Simulation : Form
    {
        FlightPlan plan1;
        FlightPlan plan2;
        // Distancia de seguridad y tiempo de ciclo.
        double distanciaSeguridad = 30;

        // De momento tienen un valor por defecto. Cuando esté hecho el formulario
        // de "Distancia de seguridad y tiempo de ciclo" (Fase 2) se rellenarán desde ahí.
        public Simulation(FlightPlan plan1, FlightPlan plan2)
        {
            InitializeComponent();

            this.plan1 = plan1;
            this.plan2 = plan2;
        }

        private void panelSimulacion_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            g.DrawRectangle(Pens.Black, 0, 0,
                panelSimulacion.Width - 1,
                panelSimulacion.Height - 1);
            
            //Calcular el radio
            float radio = (float)distanciaSeguridad;

            //Linia avión 1 (paso6)
            Position inicio1 = plan1.GetInitialPosition();
            Position final1 = plan1.GetFinalPosition();
            
            float xInicio1 = (float)inicio1.GetX();
            float yInicio1 = (float)inicio1.GetY();
            float xFinal1 = (float)final1.GetX();
            float yFinal1 = (float)final1.GetY();

            g.DrawLine(Pens.Blue, xInicio1, yInicio1, xFinal1, yFinal1);

            Position p1 = plan1.GetCurrentPosition();

            float x1 = (float)p1.GetX();
            float y1 = (float)p1.GetY();
            
            //paso7
            g.DrawEllipse(Pens.Blue, x1 - radio, y1 - radio, radio * 2, radio * 2);
            
            //Linia avión 2 (paso6)
            g.FillEllipse(Brushes.Blue, x1 - 5, y1 - 5, 10, 10);

            Position inicio2 = plan2.GetInitialPosition();
            Position final2 = plan2.GetFinalPosition();

            float xInicio2 = (float)inicio2.GetX();
            float yInicio2 = (float)inicio2.GetY();
            float xFinal2 = (float)final2.GetX();
            float yFinal2 = (float)final2.GetY();

            g.DrawLine(Pens.Red, xInicio2, yInicio2, xFinal2, yFinal2);

            Position p2 = plan2.GetCurrentPosition();

            float x2 = (float)p2.GetX();
            float y2 = (float)p2.GetY();

            //paso7 avión2
            g.DrawEllipse(Pens.Red, x2 - radio, y2 - radio, radio * 2, radio * 2);

            g.FillEllipse(Brushes.Red, x2 - 5, y2 - 5, 10, 10);
        }

        private void button1_Clock(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            plan1.Move(1);
            plan2.Move(1);

            panelSimulacion.Invalidate();
        }

        private void panelSimulacion_MouseClick(object sender, MouseEventArgs e)
        {
            float x = e.X;
            float y = e.Y;

            Position p1 = plan1.GetCurrentPosition();

            double distancia1 = Math.Sqrt(
                (x - p1.GetX()) * (x - p1.GetX()) +
                (y - p1.GetY()) * (y - p1.GetY())
            );

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

            double distancia2 = Math.Sqrt(
                (x - p2.GetX()) * (x - p2.GetX()) +
                (y - p2.GetY()) * (y - p2.GetY())
            );

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
            timerSimulacion.Start();

            buttonIniciar.Enabled = false; //Deshabilitar el botón de iniciar
            buttonDetener.Enabled = true; //Habilitar el botón de detener
            buttonMover.Enabled = false; //Deshabilitar el botón de mover
        }

        private void buttonDetener_Click(object sender, EventArgs e)
        {
            timerSimulacion.Stop();

            //Contrario a lo que se hace en el botón de iniciar, habilitar y deshabilitar de antes
            buttonIniciar.Enabled = true;
            buttonDetener.Enabled = false;
            buttonMover.Enabled = true;
        }

        private void timerSimulacion_Tick(object sender, EventArgs e)
        {
            //Mueven cada avión un ciclo, pero solo si no ha llegado ya (usa tu HasArrived de la Fase 1)
            if (plan1.HasArrived() == false)
            {
                plan1.Move(1);
            }
            if (plan2.HasArrived() == false)
            {
                plan2.Move(1);
            }

            //Repintar el panel para que se vea la nueva posición de los aviones
            panelSimulacion.Invalidate();

            //Si los dos aviones han llegado a su destino, parar la simulación y mostrar un mensaje
            if (plan1.HasArrived() == true && plan2.HasArrived() == true)
            {
                timerSimulacion.Stop();

                buttonIniciar.Enabled = true;
                buttonDetener.Enabled = false;
                buttonMover.Enabled = true;

                MessageBox.Show("Los dos vuelos han llegado a su destino.");
            }
        }
    }
}