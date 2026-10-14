using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SimulatorForms
{
    // Esta clase guarda el "diseño" de toda la aplicación:
    // colores, letras y trozos de código que se repiten (botones, tarjetas, el avión...).
    // Así todas las ventanas se ven iguales y no hay que repetir código.
    public static class Estilo
    {
        // ---------------- COLORES ----------------
        public static Color ColorFondo = Color.FromArgb(245, 238, 224);       // crema (fondo de las ventanas)
        public static Color ColorBarra = Color.FromArgb(238, 229, 210);       // crema algo más oscuro
        public static Color ColorTarjeta = Color.FromArgb(236, 226, 206);     // color de las tarjetas
        public static Color ColorCampo = Color.FromArgb(253, 250, 244);       // casi blanco (cajas y botones suaves)
        public static Color ColorTexto = Color.FromArgb(45, 38, 34);          // marrón muy oscuro
        public static Color ColorGris = Color.FromArgb(140, 128, 112);        // gris cálido para textos pequeños

        public static Color ColorPrincipal = Color.FromArgb(255, 112, 67);    // naranja
        public static Color ColorPrincipalOscuro = Color.FromArgb(224, 95, 50); // naranja oscuro
        public static Color ColorSuave = Color.FromArgb(255, 226, 208);       // naranja muy claro

        public static Color ColorAvion1 = Color.FromArgb(255, 112, 67);       // avión 1: naranja
        public static Color ColorAvion2 = Color.FromArgb(38, 103, 115);       // avión 2: verde azulado

        // ---------------- LETRAS ----------------
        public static Font FuenteNormal = new Font("Segoe UI", 10);
        public static Font FuenteCampo = new Font("Segoe UI", 11);
        public static Font FuenteBoton = new Font("Segoe UI Semibold", 11);
        public static Font FuenteSeccion = new Font("Segoe UI Semibold", 15);
        public static Font FuenteTitulo = new Font("Segoe UI Semibold", 26);
        public static Font FuenteSubtitulo = new Font("Segoe UI", 12);

        // ---------------- FORMULARIO ----------------
        // Pone el fondo, centra la ventana, la deja de tamaño fijo
        // y da estilo a las etiquetas y cajas de texto que ya existen
        public static void AplicarFormulario(Form formulario)
        {
            formulario.BackColor = ColorFondo;
            formulario.StartPosition = FormStartPosition.CenterScreen;
            formulario.FormBorderStyle = FormBorderStyle.FixedSingle;
            formulario.MaximizeBox = false;

            // IMPORTANTE: así Windows NO cambia el tamaño ni la posición de los controles
            formulario.AutoScaleMode = AutoScaleMode.None;

            foreach (Control control in formulario.Controls)
            {
                if (control is Label)
                {
                    control.Font = FuenteNormal;
                    control.ForeColor = ColorTexto;
                }
                if (control is TextBox)
                {
                    control.Font = FuenteCampo;
                    control.ForeColor = ColorTexto;
                    control.BackColor = ColorCampo;
                    ((TextBox)control).BorderStyle = BorderStyle.FixedSingle;
                }
            }
        }

        // ---------------- ESQUINAS REDONDEADAS ----------------
        // Recorta un control (botón, panel...) para que tenga las esquinas redondas.
        // Hay que llamarlo DESPUÉS de ponerle su tamaño.
        public static void Redondear(Control control, int radio)
        {
            int d = radio * 2;   // diámetro

            GraphicsPath camino = new GraphicsPath();
            camino.AddArc(0, 0, d, d, 180, 90);                                       // esquina arriba izquierda
            camino.AddArc(control.Width - d, 0, d, d, 270, 90);                       // arriba derecha
            camino.AddArc(control.Width - d, control.Height - d, d, d, 0, 90);        // abajo derecha
            camino.AddArc(0, control.Height - d, d, d, 90, 90);                       // abajo izquierda
            camino.CloseFigure();

            control.Region = new Region(camino);
        }

        // ---------------- BOTONES ----------------
        // Deja un botón con texto, posición, tamaño y estilo, todo de una vez.
        // principal = true  -> botón naranja
        // principal = false -> botón claro
        public static void PonerBoton(Button boton, string texto, int x, int y, int ancho, int alto, bool principal)
        {
            boton.Text = texto;
            boton.Location = new Point(x, y);
            boton.Size = new Size(ancho, alto);

            boton.FlatStyle = FlatStyle.Flat;            // botón plano
            boton.FlatAppearance.BorderSize = 0;         // sin borde
            boton.UseVisualStyleBackColor = false;
            boton.Font = FuenteBoton;
            boton.Cursor = Cursors.Hand;                 // el cursor se vuelve una mano

            if (principal == true)
            {
                boton.BackColor = ColorPrincipal;
                boton.ForeColor = Color.White;
                boton.FlatAppearance.MouseOverBackColor = ColorPrincipalOscuro;
                boton.FlatAppearance.MouseDownBackColor = ColorPrincipalOscuro;
            }
            else
            {
                boton.BackColor = ColorCampo;
                boton.ForeColor = ColorTexto;
                boton.FlatAppearance.MouseOverBackColor = ColorSuave;
                boton.FlatAppearance.MouseDownBackColor = ColorSuave;
            }

            Redondear(boton, 12);
        }

        // ---------------- ETIQUETAS Y TARJETAS ----------------
        // Crea una etiqueta (texto) ya colocada
        public static Label CrearEtiqueta(string texto, Font fuente, Color color, int x, int y)
        {
            Label etiqueta = new Label();
            etiqueta.Text = texto;
            etiqueta.Font = fuente;
            etiqueta.ForeColor = color;
            etiqueta.AutoSize = true;
            etiqueta.Location = new Point(x, y);
            return etiqueta;
        }

        // Crea una tarjeta: un panel con las esquinas redondas
        public static Panel CrearTarjeta(int x, int y, int ancho, int alto)
        {
            Panel tarjeta = new Panel();
            tarjeta.BackColor = ColorTarjeta;
            tarjeta.Location = new Point(x, y);
            tarjeta.Size = new Size(ancho, alto);
            Redondear(tarjeta, 20);
            return tarjeta;
        }

        // ---------------- SILUETA DE AVIÓN ----------------
        // Dibuja un avión visto desde arriba.
        // (centroX, centroY) = dónde se pone, escala = tamaño, angulo = giro en grados
        // Con angulo = 0 el avión mira hacia la derecha.
        public static void DibujarAvion(Graphics g, float centroX, float centroY, float escala, float angulo, Color color)
        {
            // Los puntos del contorno del avión, de la nariz a la cola y vuelta
            PointF[] avion = new PointF[]
            {
                new PointF(100, 0),     // nariz
                new PointF(92, -5),
                new PointF(70, -9),
                new PointF(35, -9),     // empieza el ala de arriba
                new PointF(-5, -72),
                new PointF(-20, -72),
                new PointF(-9, -9),
                new PointF(-60, -9),    // empieza el timón de arriba
                new PointF(-85, -30),
                new PointF(-97, -30),
                new PointF(-92, -4),
                new PointF(-100, -3),   // cola
                new PointF(-100, 3),
                new PointF(-92, 4),
                new PointF(-97, 30),    // timón de abajo
                new PointF(-85, 30),
                new PointF(-60, 9),
                new PointF(-9, 9),
                new PointF(-20, 72),    // ala de abajo
                new PointF(-5, 72),
                new PointF(35, 9),
                new PointF(70, 9),
                new PointF(92, 5)
            };

            GraphicsState estado = g.Save();        // guardamos cómo estaba el dibujo

            g.TranslateTransform(centroX, centroY); // 1) movemos el punto de dibujo al centro del avión
            g.RotateTransform(angulo);              // 2) giramos
            g.ScaleTransform(escala, escala);       // 3) cambiamos el tamaño

            SolidBrush pincel = new SolidBrush(color);
            g.FillPolygon(pincel, avion);

            g.Restore(estado);                      // dejamos el dibujo como estaba
        }

        // ---------------- LÁPICES PARA LA SIMULACIÓN ----------------
        // Línea de la trayectoria: fina y a rayas
        public static Pen LapizTrayectoria(Color color)
        {
            Pen lapiz = new Pen(color, 1);
            lapiz.DashStyle = DashStyle.Dash;
            return lapiz;
        }

        // Borde de la elipse de seguridad: algo más gruesa
        public static Pen LapizElipse(Color color)
        {
            Pen lapiz = new Pen(color, 2);
            return lapiz;
        }

        // Relleno transparente para dentro de la elipse
        public static SolidBrush PincelSuave(Color color)
        {
            SolidBrush pincel = new SolidBrush(Color.FromArgb(35, color));
            return pincel;
        }
    }
}