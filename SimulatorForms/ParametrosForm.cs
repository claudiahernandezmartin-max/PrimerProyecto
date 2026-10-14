using System;
using System.Windows.Forms;

namespace SimulatorForms
{
    public partial class ParametrosForm : Form
    {
        double distancia;
        double ciclo;
        bool aceptado = false;

        public ParametrosForm()
        {
            InitializeComponent();
        }

        public double GetDistancia()
        {
            return distancia;
        }

        public double GetCiclo()
        {
            return ciclo;
        }

        public bool GetAceptado()
        {
            return aceptado;
        }

        private void buttonAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                double d = Convert.ToDouble(TextBoxDistancia.Text);
                double c = Convert.ToDouble(TextBoxCiclo.Text);

                if (d <= 0 || c <= 0)
                {
                    MessageBox.Show("La distancia de seguridad y el tiempo de ciclo deben ser mayores que 0.");
                }
                else
                {
                    distancia = d;
                    ciclo = c;
                    aceptado = true;
                    Close();
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Hay algún dato mal escrito. Revisa que los números sean correctos.");
            }
        }
    }
}