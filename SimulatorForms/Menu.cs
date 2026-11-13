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
using System.IO;

namespace SimulatorForms
{
    public partial class Menu : Form
    {
        FlightPlanList lista = new FlightPlanList();

        // FASE 2.3: valores por defecto si el usuario no los cambia
        double distanciaSeguridad = 15;
        double tiempoCiclo = 2;
        bool reiniciar = true; // false justo después de cargar un fichero, para seguir desde donde se guardó

        Estilo estilo = new Estilo();
        Label labelVuelos = new Label();   // "Vuelos cargados: N", abajo del todo

        public Menu()
        {
            InitializeComponent();
            CrearInterfaz();
        }

        // Crea la cabecera y los 6 botones grandes con icono (antes era una barra de menús gris)
        private void CrearInterfaz()
        {
            estilo.PonerVentana(this, "Menú");
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            int margen = 40;
            int ancho = 310;
            int alto = 88;
            int hueco = 20;        // espacio entre las dos columnas
            int separacion = 16;   // espacio entre filas
            int arriba = estilo.GetAltoCabecera() + 28;

            // Cada tarjeta se construye con controles normales de Windows Forms.
            Panel[] tarjetas = new Panel[6];
            tarjetas[0] = CrearTarjeta("Planes de vuelo", "Escribe los datos de los vuelos", "planes", false, 0);
            tarjetas[1] = CrearTarjeta("Cargar desde fichero", "Abre un fichero con vuelos", "carpeta", false, 1);
            tarjetas[2] = CrearTarjeta("Vuelos de prueba", "Carga los 8 vuelos de ejemplo", "prueba", false, 2);
            tarjetas[3] = CrearTarjeta("Continuar guardada", "Sigue donde lo dejaste", "historial", false, 3);
            tarjetas[4] = CrearTarjeta("Distancia y ciclo", "Ajusta la seguridad y el tiempo", "parametros", false, 4);
            tarjetas[5] = CrearTarjeta("Simulación", "Lanza la simulación", "simulacion", true, 5);

            // Se colocan de dos en dos.
            int i = 0;
            while (i < tarjetas.Length)
            {
                int columna = i % 2;
                int fila = i / 2;
                tarjetas[i].Size = new Size(ancho, alto);
                tarjetas[i].Location = new Point(margen + columna * (ancho + hueco), arriba + fila * (alto + separacion));
                this.Controls.Add(tarjetas[i]);
                i++;
            }

            // Texto de abajo con los vuelos que hay cargados
            estilo.PonerEtiqueta(labelVuelos);
            labelVuelos.ForeColor = Color.FromArgb(100, 110, 125);
            labelVuelos.AutoSize = false;
            labelVuelos.TextAlign = ContentAlignment.MiddleLeft;
            labelVuelos.Location = new Point(margen, arriba + 3 * alto + 2 * separacion + 16);
            labelVuelos.Size = new Size(2 * ancho + hueco, 24);
            this.Controls.Add(labelVuelos);

            this.ClientSize = new Size(2 * margen + 2 * ancho + hueco, labelVuelos.Bottom + 24);
            estilo.PonerCabecera(this, "Deconflicting Tool", "Centro de control de vuelos", false);
            ActualizarContador();
        }

        // Crea una tarjeta usando únicamente Panel, PictureBox y Label.
        private Panel CrearTarjeta(string titulo, string descripcion, string icono, bool principal, int accion)
        {
            Panel tarjeta = new Panel();
            tarjeta.Size = new Size(310, 88);
            tarjeta.BorderStyle = BorderStyle.FixedSingle;
            tarjeta.Cursor = Cursors.Hand;

            Color fondo = Color.White;
            Color colorTitulo = Color.FromArgb(11, 31, 58);
            Color colorDescripcion = Color.FromArgb(100, 110, 125);
            Color fondoIcono = Color.FromArgb(226, 236, 252);
            Color colorIcono = estilo.GetColorAzul();

            if (principal)
            {
                fondo = Color.FromArgb(255, 112, 67);
                colorTitulo = Color.White;
                colorDescripcion = Color.White;
                fondoIcono = Color.FromArgb(255, 145, 110);
                colorIcono = Color.White;
            }

            tarjeta.BackColor = fondo;

            PictureBox imagen = new PictureBox();
            imagen.Location = new Point(18, 18);
            imagen.Size = new Size(52, 52);
            imagen.BackColor = fondoIcono;
            imagen.SizeMode = PictureBoxSizeMode.CenterImage;
            imagen.Image = estilo.CrearIcono(icono, 28, colorIcono);

            Label etiquetaTitulo = new Label();
            etiquetaTitulo.Location = new Point(84, 22);
            etiquetaTitulo.Size = new Size(210, 24);
            etiquetaTitulo.Text = titulo;
            etiquetaTitulo.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            etiquetaTitulo.ForeColor = colorTitulo;
            etiquetaTitulo.BackColor = Color.Transparent;

            Label etiquetaDescripcion = new Label();
            etiquetaDescripcion.Location = new Point(84, 48);
            etiquetaDescripcion.Size = new Size(210, 22);
            etiquetaDescripcion.Text = descripcion;
            etiquetaDescripcion.Font = new Font("Segoe UI", 9);
            etiquetaDescripcion.ForeColor = colorDescripcion;
            etiquetaDescripcion.BackColor = Color.Transparent;

            tarjeta.Controls.Add(imagen);
            tarjeta.Controls.Add(etiquetaTitulo);
            tarjeta.Controls.Add(etiquetaDescripcion);

            AsignarAccion(tarjeta, accion);
            AsignarAccion(imagen, accion);
            AsignarAccion(etiquetaTitulo, accion);
            AsignarAccion(etiquetaDescripcion, accion);

            return tarjeta;
        }

        // Asigna la acción correspondiente a la tarjeta y a los controles que contiene.
        private void AsignarAccion(Control control, int accion)
        {
            if (accion == 0)
            {
                control.Click += planesDeVueloToolStripMenuItem_Click;
            }
            else if (accion == 1)
            {
                control.Click += cargarPlanesDesdeFicheroToolStripMenuItem_Click;
            }
            else if (accion == 2)
            {
                control.Click += cargarVuelosDePruebaToolStripMenuItem_Click;
            }
            else if (accion == 3)
            {
                control.Click += continuarSimulacionGuardadaToolStripMenuItem_Click;
            }
            else if (accion == 4)
            {
                control.Click += distanciaDeSeguridadYTiempoDeCicloToolStripMenuItem_Click;
            }
            else if (accion == 5)
            {
                control.Click += simulacionToolStripMenuItem_Click;
            }
        }

        // Actualiza el texto de abajo con el número de vuelos cargados
        private void ActualizarContador()
        {
            labelVuelos.Text = "Vuelos cargados: " + lista.GetNumber();
        }

        private void planesDeVueloToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FlightPlansInterface f = new FlightPlansInterface();
            f.SetLista(lista);
            f.ShowDialog();
            ActualizarContador();

        }

        private void simulacionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Hacen falta al menos 2 vuelos (pueden ser más, por ejemplo los 8 de prueba)
            if (lista.GetNumber() < 2)
            {
                MessageBox.Show("Primero hay que introducir al menos dos planes de vuelo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Los vuelos empiezan desde su posición inicial, menos si se acaba de cargar un fichero
            if (reiniciar)
            {
                lista.Restart();
            }
            reiniciar = true;

            // Ahora se pasa la lista entera en vez de plan1 y plan2
            Simulation f = new Simulation(lista, distanciaSeguridad, tiempoCiclo);
            f.ShowDialog();
        }

        private void distanciaDeSeguridadYTiempoDeCicloToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ParametrosForm f = new ParametrosForm();
            f.SetDatos(distanciaSeguridad, tiempoCiclo);
            f.ShowDialog();

            if (f.GetAceptado() == true)
            {
                distanciaSeguridad = f.GetDistancia();
                tiempoCiclo = f.GetCiclo();
            }
        }

        // Elige el fichero de vuelos con un OpenFileDialog y lo carga en la lista
        private void cargarPlanesDesdeFicheroToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialogo = new OpenFileDialog();

            if (dialogo.ShowDialog() == DialogResult.OK)
            {
                int resultado = lista.CargarFichero(dialogo.FileName);

                if (resultado == -1)
                {
                    MessageBox.Show("El fichero no existe.");
                }
                else if (resultado == -2)
                {
                    MessageBox.Show("Hay datos mal escritos o que faltan en el fichero.");
                }
                else if (resultado == -3)
                {
                    MessageBox.Show("Alguna velocidad del fichero no está permitida (tiene que ser mayor que 0 y como máximo 600).");
                }
                else if (lista.GetNumber() == 0)
                {
                    MessageBox.Show("El fichero está vacío.");
                }
                else
                {
                    reiniciar = false; // la simulación seguirá desde la posición guardada
                    MessageBox.Show("Se han cargado " + lista.GetNumber() + " vuelos.");
                }
            }
            ActualizarContador();
        }
        // Carga el fichero de prueba sin tener que buscarlo (está en bin\Debug, junto al programa)
        private void cargarVuelosDePruebaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int resultado = lista.CargarFichero("Fichero Vuelos Prueba.txt");

            if (resultado == -1)
            {
                MessageBox.Show("No se encuentra el fichero Fichero Vuelos Prueba.txt.");
            }
            else if (resultado == -2 || resultado == -3)
            {
                MessageBox.Show("El fichero de prueba tiene datos mal escritos.");
            }
            else
            {
                reiniciar = false;
                MessageBox.Show("Se han cargado " + lista.GetNumber() + " vuelos de prueba.");
            }
            ActualizarContador();
        }

        // Carga los vuelos que se guardaron con el botón Guardar, para seguir donde se dejó
        private void continuarSimulacionGuardadaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int resultado = lista.CargarFichero("vuelos_guardados.txt");

            if (resultado == -1)
            {
                MessageBox.Show("Todavía no hay ninguna simulación guardada.");
            }
            else if (resultado == -2 || resultado == -3)
            {
                MessageBox.Show("El fichero vuelos_guardados.txt tiene datos mal escritos.");
            }
            else
            {
                reiniciar = false; // la simulación seguirá desde la posición guardada
                MessageBox.Show("Se han cargado " + lista.GetNumber() + " vuelos. Abre Simulación para continuar.");
            }
            ActualizarContador();
        }
    }
}