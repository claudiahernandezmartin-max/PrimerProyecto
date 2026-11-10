using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using FlightLib;

namespace SimulatorForms
{
    public partial class Simulation : Form
    {
        FlightPlanList lista;          // ← en vez de plan1 y plan2: sirve para 2 o más vuelos
        double distanciaSeguridad;
        double tiempoCiclo;

        List<PictureBox> aviones = new List<PictureBox>();   // un PictureBox por vuelo
        int tamanoAvion = 32;                                // lado en píxeles de cada PictureBox

        bool alarmaActivada = false;
        bool cargandoTabla = false;    // ← NUEVO: se usa en el punto 18 (tabla de velocidades)

        public Simulation(FlightPlanList lista, double distanciaSeguridad, double tiempoCiclo)
        {
            InitializeComponent();

            this.lista = lista;
            this.distanciaSeguridad = distanciaSeguridad;
            this.tiempoCiclo = tiempoCiclo;

            CrearAviones();
            ColocarAviones();
            CargarTablaVelocidades();  // ← NUEVO: rellena la tabla de velocidades (punto 18)
            ActualizarEstado();
        }

        private void Simulation_Load(object sender, EventArgs e)
        {
        }

        private void panelSimulacion_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias; // bordes suaves

            g.DrawRectangle(Pens.Black, 0, 0,
                panelSimulacion.Width - 1,
                panelSimulacion.Height - 1);

            // Radio = D/2: si dos elipses se tocan, están a menos de la distancia de seguridad
            float radio = (float)(distanciaSeguridad / 2);

            int i = 0;
            while (i < lista.GetNumber())
            {
                FlightPlan plan = lista.GetFlightPlan(i);

                // Colores alternos de la clase Estilo
                Color color = Estilo.ColoresAvion[i % Estilo.ColoresAvion.Length];

                // Trayectoria (línea a rayas)
                Position inicio = plan.GetInitialPosition();
                Position final = plan.GetFinalPosition();
                g.DrawLine(Estilo.LapizTrayectoria(color),
                    (float)inicio.GetX(), (float)inicio.GetY(),
                    (float)final.GetX(), (float)final.GetY());

                // Posición actual
                float x = (float)plan.GetCurrentPosition().GetX();
                float y = (float)plan.GetCurrentPosition().GetY();

                // Elipse de seguridad: en ROJO si este vuelo está en conflicto ahora
                Color colorElipse = color;
                if (lista.EnConflicto(i, distanciaSeguridad))
                {
                    colorElipse = Color.Red;
                }
                g.FillEllipse(Estilo.PincelSuave(colorElipse), x - radio, y - radio, radio * 2, radio * 2);
                g.DrawEllipse(Estilo.LapizElipse(colorElipse), x - radio, y - radio, radio * 2, radio * 2);

                // Identificador al lado del avión
                g.DrawString(plan.GetId(), Estilo.FuenteNormal, new SolidBrush(color), x + 12, y + 8);

                i++;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Mueve todos los vuelos que no han llegado (punto 6)
            lista.Mover(tiempoCiclo);
            ColocarAviones();
            panelSimulacion.Invalidate();

            // Después de mover, comprobamos si hay conflicto
            ActualizarEstado();
            ComprobarAlarma();   // ← NUEVO: ahora también salta la alarma en modo paso a paso

            if (lista.TodosHanLlegado())
            {
                MessageBox.Show("Todos los vuelos han llegado a su destino.");
            }
        }

        // Escribe en el label si hay conflicto ahora, si lo habrá o si no
        private void ActualizarEstado()
        {
            // Distancia entre la pareja de vuelos más cercana ahora mismo
            double dActual = lista.DistanciaMinimaActual();
            int p = lista.GetParejaA();
            int q = lista.GetParejaB();
            if (p >= 0)
            {
                labelDistancia.Text = string.Format("Pareja más cercana: {0} y {1} (distancia {2:F2})",
                    lista.GetFlightPlan(p).GetId(), lista.GetFlightPlan(q).GetId(), dActual);
            }

            if (lista.HayConflicto(distanciaSeguridad))
            {
                labelEstado.Text = "¡CONFLICTO! Hay vuelos a menos de la distancia de seguridad";
                labelEstado.ForeColor = Color.Red;
            }
            else if (lista.BuscarConflictoFuturo(distanciaSeguridad))
            {
                int a = lista.GetConflictoA();
                int b = lista.GetConflictoB();
                labelEstado.Text = string.Format("Aviso: {0} y {1} entrarán en conflicto (dist. mínima {2:F2} dentro de {3:F1} ciclos)",
                    lista.GetFlightPlan(a).GetId(), lista.GetFlightPlan(b).GetId(), lista.GetConflictoDistancia(), lista.GetConflictoTiempo() / tiempoCiclo);
                labelEstado.ForeColor = Color.DarkOrange;
            }
            else
            {
                labelEstado.Text = "No habrá conflicto";
                labelEstado.ForeColor = Color.Green;
            }
        }
        private void ComprobarAlarma()
        {
            if (lista.HayConflicto(distanciaSeguridad))
            {
                if (!alarmaActivada)
                {
                    alarmaActivada = true;
                    MessageBox.Show("¡Alarma! Hay vuelos a una distancia inferior a la de seguridad.",
                        "Pérdida de separación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                alarmaActivada = false;
            }
        }
        // Crea un PictureBox por cada vuelo con la silueta del avión girada hacia su destino
        private void CrearAviones()
        {
            int i = 0;
            while (i < lista.GetNumber())
            {
                FlightPlan plan = lista.GetFlightPlan(i);
                Color color = Estilo.ColoresAvion[i % Estilo.ColoresAvion.Length];

                Position inicio = plan.GetInitialPosition();
                Position final = plan.GetFinalPosition();
                float angulo = (float)(Math.Atan2(final.GetY() - inicio.GetY(), final.GetX() - inicio.GetX()) * 180 / Math.PI);

                // La silueta se dibuja una sola vez en una imagen
                Bitmap dibujo = new Bitmap(tamanoAvion, tamanoAvion);
                Graphics g = Graphics.FromImage(dibujo);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                Estilo.DibujarAvion(g, tamanoAvion / 2f, tamanoAvion / 2f, 0.12f, angulo, color);
                g.Dispose();

                PictureBox avion = new PictureBox();
                avion.Size = new Size(tamanoAvion, tamanoAvion);
                avion.BackColor = Color.Transparent;
                avion.Image = dibujo;
                avion.MouseClick += new MouseEventHandler(avion_MouseClick);

                panelSimulacion.Controls.Add(avion);
                aviones.Add(avion);
                i++;
            }
        }

        // Pone cada PictureBox sobre la posición actual de su vuelo
        private void ColocarAviones()
        {
            int i = 0;
            while (i < aviones.Count)
            {
                Position p = lista.GetFlightPlan(i).GetCurrentPosition();
                aviones[i].Location = new Point((int)p.GetX() - tamanoAvion / 2, (int)p.GetY() - tamanoAvion / 2);
                i++;
            }
        }

        // Al hacer clic en un avión, muestra los datos de su vuelo
        private void avion_MouseClick(object sender, MouseEventArgs e)
        {
            PictureBox avion = (PictureBox)sender;
            FlightPlan plan = lista.GetFlightPlan(aviones.IndexOf(avion));

            string texto = "Identificador: " + plan.GetId() + "\nCompañía: " + plan.GetCompania() +
                "\nVelocidad: " + plan.GetVelocidad().ToString("F2") +
                "\nPosición actual: (" + plan.GetCurrentPosition().GetX().ToString("F2") + ", " +
                                         plan.GetCurrentPosition().GetY().ToString("F2") + ")" +
                "\nDestino: (" + plan.GetFinalPosition().GetX().ToString("F2") + ", " +
                                 plan.GetFinalPosition().GetY().ToString("F2") + ")";

            if (plan.HasArrived())
            {
                texto = texto + "\nHa llegado al destino";
            }

            MessageBox.Show(texto, "Vuelo " + plan.GetId());
        }

        private void buttonIniciar_Click(object sender, EventArgs e)
        {
            // Antes de empezar, anticipamos el conflicto e informamos al usuario
            if (lista.BuscarConflictoFuturo(distanciaSeguridad))
            {
                int a = lista.GetConflictoA();
                int b = lista.GetConflictoB();
                DialogResult respuesta = MessageBox.Show(
                    string.Format("Los vuelos {0} y {1} entrarán en conflicto: se acercarán a {2:F2} (la distancia de seguridad es {3:F2}) dentro de {4:F1} ciclos.\n\n¿Quieres que te proponga cambios de velocidad?",
                        lista.GetFlightPlan(a).GetId(), lista.GetFlightPlan(b).GetId(), lista.GetConflictoDistancia(), distanciaSeguridad, lista.GetConflictoTiempo() / tiempoCiclo),
                    "Conflicto detectado", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (respuesta == DialogResult.Yes)
                {
                    ProponerCambiosVelocidad();
                }
            }

            timerSimulacion.Start();
            buttonIniciar.Enabled = false;
            buttonDetener.Enabled = true;
            buttonMover.Enabled = false;
        }
        private void ProponerCambiosVelocidad()
        {
            double[] antes = lista.GetVelocidades();

            if (lista.ResolverConflictos(distanciaSeguridad) == false)
            {
                MessageBox.Show("No se ha podido resolver el conflicto cambiando las velocidades.");
                return;
            }

            double[] despues = lista.GetVelocidades();
            string texto = "Cambios de velocidad propuestos:\n\n";
            int i = 0;
            while (i < despues.Length)
            {
                if (despues[i] != antes[i])
                {
                    texto = texto + string.Format("{0}: {1:F2} -> {2:F2}\n", lista.GetFlightPlan(i).GetId(), antes[i], despues[i]);
                }
                i++;
            }
            texto = texto + "\n¿Aplicar estos cambios?";

            if (MessageBox.Show(texto, "Propuesta", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                CargarTablaVelocidades();
                Restart_Click(null, null);
            }
            else
            {
                lista.SetVelocidades(antes);
            }
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
            // Mueve todos los vuelos que no han llegado
            lista.Mover(tiempoCiclo);
            ColocarAviones();
            panelSimulacion.Invalidate();
            ActualizarEstado();

            // Si ya han llegado todos, paramos el timer ANTES de la alarma
            bool fin = lista.TodosHanLlegado();
            if (fin)
            {
                timerSimulacion.Stop();
                buttonIniciar.Enabled = true;
                buttonDetener.Enabled = false;
                buttonMover.Enabled = true;
            }

            ComprobarAlarma();   // ← aquí va la llamada, en lugar del bloc FASE 10

            if (fin)
            {
                MessageBox.Show("Todos los vuelos han llegado a su destino.");
            }
        }

        // FASE 9: Abrir el formulario con el DataGridView
        private void buttonVerDatos_Click(object sender, EventArgs e)
        {
            FlightDataForm f = new FlightDataForm(lista);
            f.ShowDialog();
        }

        // FASE 10.2: Comprobar si entrarán en conflicto
        private void Conflicte_Click(object sender, EventArgs e)
        {
            if (lista.BuscarConflictoFuturo(distanciaSeguridad))
            {
                int a = lista.GetConflictoA();
                int b = lista.GetConflictoB();
                MessageBox.Show(string.Format("Los vuelos {0} y {1} SÍ entrarán en conflicto.\nDistancia mínima: {2:F2}\nDentro de {3:F1} ciclos.",
                    lista.GetFlightPlan(a).GetId(), lista.GetFlightPlan(b).GetId(), lista.GetConflictoDistancia(), lista.GetConflictoTiempo() / tiempoCiclo),
                    "Comprobación de conflicto", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                MessageBox.Show("Los vuelos NO entrarán en conflicto.", "Comprobación de conflicto",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void buttonVerDatos_Click_1(object sender, EventArgs e)
        {
            FlightDataForm f = new FlightDataForm(lista);
            f.ShowDialog();
        }

        private void Restart_Click(object sender, EventArgs e)
        {
            timerSimulacion.Stop();
            lista.Restart();            // ← en vez de plan1.Restart(); plan2.Restart();
            ColocarAviones();
            alarmaActivada = false;

            buttonIniciar.Enabled = true;
            buttonDetener.Enabled = false;
            buttonMover.Enabled = true;

            panelSimulacion.Invalidate();
            ActualizarEstado();
        }

        private void CargarTablaVelocidades()
        {
            cargandoTabla = true;
            dataGridViewVelocidades.Rows.Clear();
            int i = 0;
            while (i < lista.GetNumber())
            {
                FlightPlan plan = lista.GetFlightPlan(i);
                dataGridViewVelocidades.Rows.Add(plan.GetId(), plan.GetVelocidad().ToString("F2"));
                i++;
            }
            cargandoTabla = false;
        }
        private void buttonVelocidadesOriginales_Click(object sender, EventArgs e)
        {
            lista.RestaurarVelocidadesIniciales();
            CargarTablaVelocidades();
            Restart_Click(null, null);

        }
        private void dataGridViewVelocidades_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (cargandoTabla || e.RowIndex < 0 || e.RowIndex >= lista.GetNumber() || e.ColumnIndex != 1)
            {
                return;
            }

            FlightPlan plan = lista.GetFlightPlan(e.RowIndex);
            object valor = dataGridViewVelocidades.Rows[e.RowIndex].Cells[1].Value;
            bool valida = true;

            try
            {
                plan.SetVelocidad(Convert.ToDouble(valor));   // si es <= 0 lanza excepción
            }
            catch (FormatException)
            {
                valida = false;
                MessageBox.Show("La velocidad tiene que ser un número.");
            }
            catch (ArgumentException)
            {
                valida = false;
                MessageBox.Show("La velocidad tiene que ser mayor que 0.");
            }
            catch (OverflowException)
            {
                valida = false;
                MessageBox.Show("La velocidad es demasiado grande.");
            }

            // Esperamos a que la tabla acabe de editar, la recargamos y, solo si era válida, reiniciamos
            BeginInvoke(new Action(delegate ()
            {
                CargarTablaVelocidades();
                if (valida)
                {
                    Restart_Click(null, null);
                }
            }));
        }
        private void label2_Click(object sender, EventArgs e)
        {

        }

        // Guarda los vuelos, con su posición actual, en el fichero que elija el usuario
        private void buttonGuardar_Click(object sender, EventArgs e)
        {
            buttonDetener_Click(null, null); // paramos para guardar las posiciones de este momento

            SaveFileDialog dialogo = new SaveFileDialog();
            if (dialogo.ShowDialog() == DialogResult.OK)
            {
                lista.GuardarFichero(dialogo.FileName);
                MessageBox.Show("Vuelos guardados.");
            }
        }
    }
}