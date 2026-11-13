using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using FlightLib;

namespace SimulatorForms
{
    public partial class Simulation : Form
    {
        FlightPlanList lista;          // en vez de plan1 y plan2: sirve para 2 o más vuelos
        double distanciaSeguridad;
        double tiempoCiclo;

        List<PictureBox> aviones = new List<PictureBox>();   // un PictureBox por vuelo
        int tamanoAvion = 32;   // lado en píxeles de cada PictureBox
        Estilo estilo = new Estilo();   // colores y dibujos de la simulación

        public Simulation(FlightPlanList lista, double distanciaSeguridad, double tiempoCiclo)
        {
            InitializeComponent();
            AplicarEstilo();

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

        // Aspecto de la ventana: cabecera, botones con icono y tabla con los colores del programa
        private void AplicarEstilo()
        {
            estilo.PonerDialogo(this, "Simulación");
            panelSimulacion.BackColor = Color.White;

            estilo.PonerBotonConIcono(buttonIniciar, "play", true);
            estilo.PonerBotonConIcono(buttonDetener, "stop", false);
            estilo.PonerBotonConIcono(buttonMover, "mover", false);
            buttonDeshacer.Text = "Deshacer";
            estilo.PonerBotonConIcono(buttonDeshacer, "atras", false);
            estilo.PonerBotonConIcono(button1, "restart", false);
            estilo.PonerBotonConIcono(buttonGuardar, "guardar", false);
            estilo.PonerBotonConIcono(buttonVelocidadesOriginales, "restart", false);
            estilo.PonerBotonConIcono(buttonVerDatos, "datos", false);
            estilo.PonerBotonConIcono(Conflicte, "alerta", false);

            estilo.PonerEtiqueta(label1);
            label1.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            estilo.PonerEtiqueta(labelDistancia);
            estilo.PonerEtiqueta(labelEstado);
            labelEstado.Font = new Font("Segoe UI", 10, FontStyle.Bold);   // el color lo pone ActualizarEstado
            estilo.PonerTabla(dataGridViewVelocidades);

            estilo.PonerCabecera(this, "Simulación", "Controla los vuelos y detecta conflictos", true);
        }

        private void panelSimulacion_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias; // bordes suaves

            // Cuadrícula suave (cada 50 unidades) y borde del panel
            estilo.DibujarCuadricula(g, panelSimulacion.Width, panelSimulacion.Height, 50);
            g.DrawRectangle(Pens.LightSteelBlue, 0, 0,
                panelSimulacion.Width - 1,
                panelSimulacion.Height - 1);

            // Radio = D/2: si dos elipses se tocan, están a menos de la distancia de seguridad
            float radio = (float)(distanciaSeguridad / 2);

            int i = 0;
            while (i < lista.GetNumber())
            {
                FlightPlan plan = lista.GetFlightPlan(i);

                // Colores alternos de la clase Estilo
                Color color = estilo.GetColorAvion(i);

                // Trayectoria (línea a rayas)
                Position inicio = plan.GetInitialPosition();
                Position final = plan.GetFinalPosition();
                g.DrawLine(estilo.LapizTrayectoria(color),
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
                g.FillEllipse(estilo.PincelSuave(colorElipse), x - radio, y - radio, radio * 2, radio * 2);
                g.DrawEllipse(estilo.LapizElipse(colorElipse), x - radio, y - radio, radio * 2, radio * 2);

                // Identificador al lado del avión
                g.DrawString(plan.GetId(), estilo.GetFuenteNormal(), new SolidBrush(color), x + 12, y + 8);

                i++;
            }
        }

        // Botón Mover: avanza un ciclo todos los vuelos que no han llegado
        private void button1_Click(object sender, EventArgs e)
        {
            lista.Mover(tiempoCiclo);
            ColocarAviones();
            panelSimulacion.Invalidate();   // al repintar, las elipses en conflicto salen en rojo
            ActualizarEstado();

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
                labelEstado.Text = "CONFLICTO Hay vuelos a menos de la distancia de seguridad";
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
        // Crea un PictureBox por cada vuelo con la silueta del avión girada hacia su destino
        private void CrearAviones()
        {
            int i = 0;
            while (i < lista.GetNumber())
            {
                FlightPlan plan = lista.GetFlightPlan(i);
                Color color = estilo.GetColorAvion(i);

                Position inicio = plan.GetInitialPosition();
                Position final = plan.GetFinalPosition();
                float angulo = (float)(Math.Atan2(final.GetY() - inicio.GetY(), final.GetX() - inicio.GetX()) * 180 / Math.PI);

                // La silueta se dibuja una sola vez en una imagen
                Bitmap dibujo = new Bitmap(tamanoAvion, tamanoAvion);
                Graphics g = Graphics.FromImage(dibujo);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                estilo.DibujarAvion(g, tamanoAvion / 2f, tamanoAvion / 2f, 0.12f, angulo, color);
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
        // Calcula con la derivada nuevas velocidades, las enseña y pregunta si se aplican
        private void ProponerCambiosVelocidad()
        {
            double[] antes = lista.GetVelocidades();
            int sinResolver = lista.ResolverConflictos(distanciaSeguridad);
            double[] despues = lista.GetVelocidades();

            // Escribimos solo los vuelos que cambian de velocidad
            string texto = "";
            for (int i = 0; i < despues.Length; i++)
            {
                if (despues[i] != antes[i])
                {
                    texto = texto + string.Format("{0}: {1:F2} -> {2:F2}\n", lista.GetFlightPlan(i).GetId(), antes[i], despues[i]);
                }
            }

            if (texto == "")
            {
                MessageBox.Show("No se ha podido resolver ningún conflicto cambiando las velocidades.");
                return;
            }

            texto = "Cambios de velocidad propuestos:\n\n" + texto;
            if (sinResolver > 0)
            {
                texto = texto + "\nQuedan " + sinResolver + " conflictos sin solución (los aviones ya están demasiado cerca " +
                    "o haría falta una velocidad no permitida).\n";
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
            lista.Mover(tiempoCiclo);
            ColocarAviones();
            panelSimulacion.Invalidate();   // al repintar, las elipses en conflicto salen en rojo
            ActualizarEstado();

            if (lista.TodosHanLlegado())
            {
                timerSimulacion.Stop();
                buttonIniciar.Enabled = true;
                buttonDetener.Enabled = false;
                buttonMover.Enabled = true;
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

            buttonIniciar.Enabled = true;
            buttonDetener.Enabled = false;
            buttonMover.Enabled = true;

            panelSimulacion.Invalidate();
            ActualizarEstado();
        }

        private void CargarTablaVelocidades()
        {
            dataGridViewVelocidades.Rows.Clear();
            int i = 0;
            while (i < lista.GetNumber())
            {
                FlightPlan plan = lista.GetFlightPlan(i);
                dataGridViewVelocidades.Rows.Add(plan.GetId(), plan.GetVelocidad().ToString("F2"));
                i++;
            }
        }
        private void buttonVelocidadesOriginales_Click(object sender, EventArgs e)
        {
            lista.RestaurarVelocidadesIniciales();
            CargarTablaVelocidades();
            Restart_Click(null, null);

        }
        // Cuando el usuario cambia una velocidad en la tabla: la aplica y reinicia la simulación
        private void dataGridViewVelocidades_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= lista.GetNumber() || e.ColumnIndex != 1)
            {
                return;
            }

            FlightPlan plan = lista.GetFlightPlan(e.RowIndex);
            DataGridViewCell celda = dataGridViewVelocidades.Rows[e.RowIndex].Cells[1];
            bool valida = true;

            try
            {
                double nueva = Convert.ToDouble(celda.Value);
                if (plan.SetVelocidad(nueva) == false)
                {
                    valida = false;
                    MessageBox.Show("La velocidad tiene que ser mayor que 0 y como máximo " + plan.GetVelocidadMaxima() + ".");
                }
            }
            catch (FormatException)
            {
                valida = false;
                MessageBox.Show("La velocidad tiene que ser un número.");
            }
            catch (OverflowException)
            {
                valida = false;
                MessageBox.Show("La velocidad es demasiado grande.");
            }

            // En la celda se queda la velocidad que tiene el vuelo (la nueva o, si era mala, la de antes)
            celda.Value = plan.GetVelocidad().ToString("F2");

            if (valida == true)
            {
                Restart_Click(null, null);   // los aviones vuelven al principio con la velocidad nueva
            }
        }
        private void label2_Click(object sender, EventArgs e)
        {

        }

        // Guarda los vuelos, con su posición actual, en el fichero "vuelos_guardados.txt"
        private void buttonGuardar_Click(object sender, EventArgs e)
        {
            timerSimulacion.Stop();   // paramos para guardar las posiciones de este momento
            buttonIniciar.Enabled = true;
            buttonDetener.Enabled = false;
            buttonMover.Enabled = true;

            lista.GuardarFichero("vuelos_guardados.txt");
            MessageBox.Show("Vuelos guardados. Para seguir otro día: Menú → Continuar simulación guardada.");
        }

        private void buttonDeshacer_Click(object sender, EventArgs e)
        {
            timerSimulacion.Stop();   // si iba en automático, primero paramos
            buttonIniciar.Enabled = true;
            buttonDetener.Enabled = false;
            buttonMover.Enabled = true;

            if (lista.Deshacer() == false)
            {
                MessageBox.Show("No hay ningún paso que deshacer.");
                return;
            }

            ColocarAviones();
            panelSimulacion.Invalidate();
            ActualizarEstado();
        }
    }
}