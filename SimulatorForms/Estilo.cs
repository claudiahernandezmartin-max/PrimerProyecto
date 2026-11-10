using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SimulatorForms
{
    // Esta clase guarda los colores, letras y dibujos que usa la simulación.
    // Se crea un objeto (Estilo estilo = new Estilo();) y se usan sus métodos.
    public class Estilo
    {
        // Colores para los 8 aviones (si hay más, se repiten)
        Color[] coloresAvion =
        {
            Color.FromArgb(255, 112, 67),
            Color.FromArgb(38, 103, 115),
            Color.FromArgb(142, 68, 173),
            Color.FromArgb(39, 174, 96),
            Color.FromArgb(41, 128, 185),
            Color.FromArgb(233, 30, 99),
            Color.FromArgb(121, 85, 72),
            Color.FromArgb(127, 140, 141)
        };

        // Letra para el identificador de cada avión
        Font fuenteNormal = new Font("Segoe UI", 10);
        // Colores de las ventanas
        Color colorCielo = Color.FromArgb(11, 31, 58);        // azul noche
        Color colorCieloClaro = Color.FromArgb(30, 90, 168);  // azul cielo
        Color colorFondo = Color.FromArgb(244, 247, 252);     // blanco azulado
        Color colorNaranja = Color.FromArgb(255, 112, 67);    // naranja (botón principal)
        Color colorTexto = Color.FromArgb(33, 37, 41);        // casi negro

        // Devuelve el color del avión número i
        public Color GetColorAvion(int i)
        {
            return coloresAvion[i % coloresAvion.Length];
        }

        // Devuelve la letra normal
        public Font GetFuenteNormal()
        {
            return fuenteNormal;
        }

        // Dibuja un avión visto desde arriba.
        // (centroX, centroY) = dónde se pone, escala = tamaño, angulo = giro en grados
        public void DibujarAvion(Graphics g, float centroX, float centroY, float escala, float angulo, Color color)
        {
            // Los puntos del contorno del avión, de la nariz a la cola y vuelta
            PointF[] avion = new PointF[]
            {
                new PointF(100, 0),
                new PointF(92, -5),
                new PointF(70, -9),
                new PointF(35, -9),
                new PointF(-5, -72),
                new PointF(-20, -72),
                new PointF(-9, -9),
                new PointF(-60, -9),
                new PointF(-85, -30),
                new PointF(-97, -30),
                new PointF(-92, -4),
                new PointF(-100, -3),
                new PointF(-100, 3),
                new PointF(-92, 4),
                new PointF(-97, 30),
                new PointF(-85, 30),
                new PointF(-60, 9),
                new PointF(-9, 9),
                new PointF(-20, 72),
                new PointF(-5, 72),
                new PointF(35, 9),
                new PointF(70, 9),
                new PointF(92, 5)
            };

            GraphicsState estado = g.Save();        // guardamos cómo estaba el dibujo
            g.TranslateTransform(centroX, centroY); // movemos el dibujo al centro del avión
            g.RotateTransform(angulo);              // giramos
            g.ScaleTransform(escala, escala);       // cambiamos el tamaño

            SolidBrush pincel = new SolidBrush(color);
            g.FillPolygon(pincel, avion);

            g.Restore(estado);                      // dejamos el dibujo como estaba
        }

        // Línea de la trayectoria: fina y a rayas
        public Pen LapizTrayectoria(Color color)
        {
            Pen lapiz = new Pen(color, 1);
            lapiz.DashStyle = DashStyle.Dash;
            return lapiz;
        }

        // Borde de la elipse de seguridad: algo más gruesa
        public Pen LapizElipse(Color color)
        {
            Pen lapiz = new Pen(color, 2);
            return lapiz;
        }

        // Relleno transparente para dentro de la elipse
        public SolidBrush PincelSuave(Color color)
        {
            SolidBrush pincel = new SolidBrush(Color.FromArgb(35, color));
            return pincel;
        }
        // Pone el color de fondo a una ventana y la centra en la pantalla
        public void PonerFormulario(Form f)
        {
            f.BackColor = colorFondo;
            f.StartPosition = FormStartPosition.CenterScreen;
        }

        // Botón principal: naranja con letras blancas y sin borde
        public void PonerBotonPrincipal(Button b)
        {
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.BackColor = colorNaranja;
            b.ForeColor = Color.White;
            b.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            b.Cursor = Cursors.Hand;   // la manita al pasar el ratón
        }

        // Botón secundario: blanco con borde y letras azules
        public void PonerBotonSecundario(Button b)
        {
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderColor = colorCieloClaro;
            b.BackColor = Color.White;
            b.ForeColor = colorCieloClaro;
            b.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            b.Cursor = Cursors.Hand;
        }

        // Caja de texto con letra más grande
        public void PonerCaja(TextBox t)
        {
            t.Font = new Font("Segoe UI", 12);
            t.BorderStyle = BorderStyle.FixedSingle;
        }

        // Etiqueta en gris oscuro
        public void PonerEtiqueta(Label l)
        {
            l.ForeColor = colorTexto;
            l.Font = new Font("Segoe UI", 10);
        }

        // Dibuja la cabecera: cielo degradado, nubes, la ruta a rayas, el avión y el título
        public void DibujarCabecera(Graphics g, int ancho, int alto, string titulo, string subtitulo)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Cielo: de azul noche (arriba) a azul cielo (abajo)
            LinearGradientBrush cielo = new LinearGradientBrush(new Rectangle(0, 0, ancho, alto), colorCielo, colorCieloClaro, 90f);
            g.FillRectangle(cielo, 0, 0, ancho, alto);

            // Nubes: elipses blancas casi transparentes
            SolidBrush nube = new SolidBrush(Color.FromArgb(35, Color.White));
            g.FillEllipse(nube, ancho - 200, 15, 110, 35);
            g.FillEllipse(nube, ancho - 150, 5, 90, 40);
            g.FillEllipse(nube, 30, alto - 45, 140, 30);

            // Ruta del avión: curva a rayas
            Pen ruta = new Pen(Color.FromArgb(150, Color.White), 2);
            ruta.DashStyle = DashStyle.Dash;
            g.DrawBezier(ruta, 10, alto - 10, ancho / 3, alto - 5, ancho / 2, alto / 2, ancho - 95, alto / 2 + 5);

            // El avión, girado hacia arriba a la derecha
            DibujarAvion(g, ancho - 65, alto / 2 - 5, 0.4f, -20, Color.White);

            // Título y subtítulo
            g.DrawString(titulo, new Font("Segoe UI", 20, FontStyle.Bold), Brushes.White, 18, alto / 2 - 38);
            g.DrawString(subtitulo, new Font("Segoe UI", 10), new SolidBrush(Color.FromArgb(210, Color.White)), 21, alto / 2 + 2);
        }
    }

}