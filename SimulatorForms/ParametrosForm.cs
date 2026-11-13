using System;
using System.Windows.Forms;

namespace SimulatorForms
{
    public partial class ParametrosForm : Form
    {
        double distancia;
        double ciclo;
        bool aceptado = false;

        Estilo estilo = new Estilo();

        public ParametrosForm()
        {
            InitializeComponent();

            // Aspecto de la ventana (igual que el login)
            estilo.PonerDialogo(this, "Parámetros");
            estilo.PonerEtiqueta(label1);
            estilo.PonerEtiqueta(label2);
            estilo.PonerCaja(TextBoxDistancia);
            estilo.PonerCaja(TextBoxCiclo);
            estilo.PonerBotonConIcono(button1, "varita", false);
            estilo.PonerBotonConIcono(buttonAceptar, "aceptar", true);
            this.AcceptButton = buttonAceptar;   // con la tecla Enter se pulsa Aceptar
            estilo.PonerCabecera(this, "Parámetros", "Ajusta la simulación", true);
        }

        // Pone en las cajas los valores que hay ahora
        public void SetDatos(double distancia, double ciclo)
        {
            TextBoxDistancia.Text = distancia.ToString();
            TextBoxCiclo.Text = ciclo.ToString();
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
            catch (OverflowException)
            {
                MessageBox.Show("Alguno de los números es demasiado grande.");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TextBoxDistancia.Text = "15";
            TextBoxCiclo.Text = "2";
        }
    }
}