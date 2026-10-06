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

            Position p1 = plan1.GetCurrentPosition();

            float x1 = (float)p1.GetX();
            float y1 = (float)p1.GetY();

            g.FillEllipse(Brushes.Blue, x1 - 5, y1 - 5, 10, 10);

            Position p2 = plan2.GetCurrentPosition();

            float x2 = (float)p2.GetX();
            float y2 = (float)p2.GetY();

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
    }
}