using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
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

        // =====================================================================
        //  NUEVO: cabecera, iconos, tarjetas del menú y tabla para el resto de ventanas
        // =====================================================================

        // Altura de la cabecera azul que se pone arriba en cada ventana
        public const int AltoCabecera = 96;

        // Colores para que otras clases (como Tarjeta) puedan usarlos
        public Color ColorAzulNoche { get { return colorCielo; } }
        public Color ColorAzul { get { return colorCieloClaro; } }
        public Color ColorNaranja { get { return colorNaranja; } }
        public Color ColorFondo { get { return colorFondo; } }
        public Color ColorTexto { get { return colorTexto; } }

        // Panel que no parpadea al repintarse (se usa para la cabecera)
        private class PanelCabecera : Panel
        {
            public PanelCabecera()
            {
                this.DoubleBuffered = true;
                this.ResizeRedraw = true;
            }
        }

        // Pone la cabecera con el cielo, el avión y el título (igual que en el login).
        // Si desplazar es true, baja todos los controles de la ventana para que no queden debajo
        // de la cabecera y hace la ventana más alta. Si es false, la ventana se queda como está.
        public void PonerCabecera(Form f, string titulo, string subtitulo, bool desplazar)
        {
            if (desplazar)
            {
                int i = 0;
                while (i < f.Controls.Count)
                {
                    Control c = f.Controls[i];
                    if (c.Dock == DockStyle.None)
                    {
                        c.Top = c.Top + AltoCabecera;
                    }
                    i++;
                }
                f.ClientSize = new Size(f.ClientSize.Width, f.ClientSize.Height + AltoCabecera);
            }

            PanelCabecera panel = new PanelCabecera();
            panel.Dock = DockStyle.Top;
            panel.Height = AltoCabecera;
            panel.Paint += delegate (object sender, PaintEventArgs e)
            {
                DibujarCabecera(e.Graphics, panel.Width, panel.Height, titulo, subtitulo);
            };
            f.Controls.Add(panel);
            panel.SendToBack();   // así se coloca arriba del todo y lo demás se reparte debajo
        }

        // Ventana normal: fondo, centrada y con el título del programa
        public void PonerVentana(Form f, string nombre)
        {
            PonerFormulario(f);
            f.Text = "Deconflicting Tool · " + nombre;
        }

        // Ventana de tamaño fijo (sin maximizar ni minimizar), para los formularios pequeños
        public void PonerDialogo(Form f, string nombre)
        {
            PonerVentana(f, nombre);
            f.FormBorderStyle = FormBorderStyle.FixedDialog;
            f.MaximizeBox = false;
            f.MinimizeBox = false;
        }

        // Rectángulo con las esquinas redondeadas
        public static GraphicsPath RectRedondo(Rectangle r, int radio)
        {
            GraphicsPath ruta = new GraphicsPath();
            int d = radio * 2;
            ruta.AddArc(r.X, r.Y, d, d, 180, 90);
            ruta.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            ruta.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            ruta.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            ruta.CloseFigure();
            return ruta;
        }

        // Fondo blanco redondeado con una sombra suave, para agrupar campos de un formulario
        public void DibujarTarjetaFondo(Graphics g, Rectangle r)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle sombra = new Rectangle(r.X, r.Y + 2, r.Width, r.Height);
            using (GraphicsPath ruta = RectRedondo(sombra, 12))
            using (SolidBrush pincelSombra = new SolidBrush(Color.FromArgb(18, 11, 31, 58)))
            {
                g.FillPath(pincelSombra, ruta);
            }
            using (GraphicsPath ruta = RectRedondo(r, 12))
            using (SolidBrush fondo = new SolidBrush(Color.White))
            using (Pen borde = new Pen(Color.FromArgb(220, 227, 238), 1))
            {
                g.FillPath(fondo, ruta);
                g.DrawPath(borde, ruta);
            }
        }

        // Cuadrícula suave para el panel de la simulación (como una pantalla de radar)
        public void DibujarCuadricula(Graphics g, int ancho, int alto, int paso)
        {
            using (Pen lapiz = new Pen(Color.FromArgb(232, 237, 245), 1))
            {
                int x = paso;
                while (x < ancho)
                {
                    g.DrawLine(lapiz, x, 0, x, alto);
                    x = x + paso;
                }
                int y = paso;
                while (y < alto)
                {
                    g.DrawLine(lapiz, 0, y, ancho, y);
                    y = y + paso;
                }
            }
        }

        // Botón con icono: lleva el icono a la izquierda del texto.
        // principal = true -> naranja con icono blanco; false -> blanco con borde y icono azul
        public void PonerBotonConIcono(Button b, string icono, bool principal)
        {
            if (principal)
            {
                PonerBotonPrincipal(b);
                b.FlatAppearance.MouseOverBackColor = Color.FromArgb(238, 95, 50);
                b.Image = CrearIcono(icono, 22, Color.White);
            }
            else
            {
                PonerBotonSecundario(b);
                b.FlatAppearance.MouseOverBackColor = Color.FromArgb(232, 241, 255);
                b.Image = CrearIcono(icono, 22, colorCieloClaro);
            }
            b.ImageAlign = ContentAlignment.MiddleCenter;
            b.TextAlign = ContentAlignment.MiddleCenter;
            b.TextImageRelation = TextImageRelation.ImageBeforeText;
        }

        // Tabla (DataGridView) con el mismo aspecto: cabecera azul noche y filas alternas
        public void PonerTabla(DataGridView d)
        {
            d.BorderStyle = BorderStyle.None;
            d.BackgroundColor = Color.White;
            d.GridColor = Color.FromArgb(225, 231, 240);
            d.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            d.EnableHeadersVisualStyles = false;
            d.RowHeadersVisible = false;
            d.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            d.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            d.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            d.ColumnHeadersHeight = 36;
            d.ColumnHeadersDefaultCellStyle.BackColor = colorCielo;
            d.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            d.ColumnHeadersDefaultCellStyle.SelectionBackColor = colorCielo;
            d.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
            d.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);

            d.ColumnHeadersDefaultCellStyle.Padding = new Padding(8, 0, 0, 0);
            d.DefaultCellStyle.Padding = new Padding(8, 0, 0, 0);
            d.RowTemplate.Height = 30;
            d.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            d.DefaultCellStyle.ForeColor = colorTexto;
            d.DefaultCellStyle.SelectionBackColor = Color.FromArgb(214, 230, 255);
            d.DefaultCellStyle.SelectionForeColor = colorTexto;
            d.AlternatingRowsDefaultCellStyle.BackColor = colorFondo;
        }

        // Crea una imagen cuadrada con un icono (para ponerla en un botón)
        public Bitmap CrearIcono(string tipo, int tam, Color color)
        {
            Bitmap imagen = new Bitmap(tam, tam);
            Graphics g = Graphics.FromImage(imagen);
            DibujarIcono(g, tipo, new RectangleF(0, 0, tam, tam), color);
            g.Dispose();
            return imagen;
        }

        // Dibuja un icono sencillo dentro del rectángulo r. Todos se dibujan en una cuadrícula de 100x100.
        // Iconos: planes, carpeta, prueba, historial, parametros, simulacion, play, stop, mover, atras,
        //         datos, alerta, restart, guardar, aceptar, cerrar, varita
        public void DibujarIcono(Graphics g, string tipo, RectangleF r, Color color)
        {
            GraphicsState estado = g.Save();
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TranslateTransform(r.X, r.Y);
            g.ScaleTransform(r.Width / 100f, r.Height / 100f);

            Pen lapiz = new Pen(color, 7);
            lapiz.StartCap = LineCap.Round;
            lapiz.EndCap = LineCap.Round;
            lapiz.LineJoin = LineJoin.Round;
            SolidBrush pincel = new SolidBrush(color);

            if (tipo == "planes")
            {
                // Tablilla con unas líneas
                g.DrawPath(lapiz, RectRedondo(new Rectangle(20, 20, 60, 70), 8));
                g.FillPath(pincel, RectRedondo(new Rectangle(36, 9, 28, 17), 5));
                g.DrawLine(lapiz, 34, 46, 66, 46);
                g.DrawLine(lapiz, 34, 62, 66, 62);
                g.DrawLine(lapiz, 34, 77, 54, 77);
            }
            else if (tipo == "carpeta")
            {
                PointF[] carpeta = { new PointF(12, 26), new PointF(40, 26), new PointF(48, 37), new PointF(88, 37), new PointF(88, 80), new PointF(12, 80) };
                g.DrawPolygon(lapiz, carpeta);
            }
            else if (tipo == "prueba")
            {
                // Matraz
                g.DrawLine(lapiz, 34, 12, 66, 12);
                PointF[] matraz = { new PointF(40, 12), new PointF(40, 40), new PointF(16, 80), new PointF(84, 80), new PointF(60, 40), new PointF(60, 12) };
                g.DrawLines(lapiz, matraz);
                g.DrawLine(lapiz, 28, 64, 72, 64);
            }
            else if (tipo == "historial")
            {
                // Reloj
                g.DrawEllipse(lapiz, 18, 18, 64, 64);
                g.DrawLines(lapiz, new PointF[] { new PointF(50, 32), new PointF(50, 50), new PointF(65, 58) });
            }
            else if (tipo == "parametros")
            {
                // Tres deslizadores
                g.DrawLine(lapiz, 14, 26, 86, 26);
                g.DrawLine(lapiz, 14, 50, 86, 50);
                g.DrawLine(lapiz, 14, 74, 86, 74);
                g.FillEllipse(pincel, 25, 17, 18, 18);
                g.FillEllipse(pincel, 55, 41, 18, 18);
                g.FillEllipse(pincel, 33, 65, 18, 18);
            }
            else if (tipo == "simulacion")
            {
                DibujarAvion(g, 50, 50, 0.42f, -35, color);
            }
            else if (tipo == "play")
            {
                PointF[] triangulo = { new PointF(30, 18), new PointF(30, 82), new PointF(82, 50) };
                g.FillPolygon(pincel, triangulo);
                g.DrawPolygon(lapiz, triangulo);
            }
            else if (tipo == "stop")
            {
                g.FillPath(pincel, RectRedondo(new Rectangle(26, 26, 48, 48), 6));
                g.DrawPath(lapiz, RectRedondo(new Rectangle(26, 26, 48, 48), 6));
            }
            else if (tipo == "mover")
            {
                g.DrawLine(lapiz, 18, 50, 80, 50);
                g.DrawLines(lapiz, new PointF[] { new PointF(56, 26), new PointF(80, 50), new PointF(56, 74) });
            }
            else if (tipo == "atras")
            {
                g.DrawLine(lapiz, 82, 50, 20, 50);
                g.DrawLines(lapiz, new PointF[] { new PointF(44, 26), new PointF(20, 50), new PointF(44, 74) });
            }
            else if (tipo == "datos")
            {
                // Tabla
                g.DrawPath(lapiz, RectRedondo(new Rectangle(12, 18, 76, 64), 6));
                g.DrawLine(lapiz, 12, 42, 88, 42);
                g.DrawLine(lapiz, 12, 62, 88, 62);
                g.DrawLine(lapiz, 40, 42, 40, 82);
            }
            else if (tipo == "alerta")
            {
                PointF[] aviso = { new PointF(50, 14), new PointF(90, 84), new PointF(10, 84) };
                g.DrawPolygon(lapiz, aviso);
                g.DrawLine(lapiz, 50, 40, 50, 60);
                g.FillEllipse(pincel, 45, 67, 10, 10);
            }
            else if (tipo == "restart")
            {
                // Flecha circular
                g.DrawArc(lapiz, 22f, 22f, 56f, 56f, 300f, 300f);
                PointF[] punta = { new PointF(48, 19), new PointF(41, 34), new PointF(31, 17) };
                g.FillPolygon(pincel, punta);
            }
            else if (tipo == "guardar")
            {
                // Disquete
                g.DrawPath(lapiz, RectRedondo(new Rectangle(16, 16, 68, 68), 8));
                g.DrawRectangle(lapiz, 32f, 16f, 34f, 24f);
                g.DrawRectangle(lapiz, 30f, 58f, 40f, 26f);
            }
            else if (tipo == "aceptar")
            {
                g.DrawLines(lapiz, new PointF[] { new PointF(20, 52), new PointF(42, 74), new PointF(82, 28) });
            }
            else if (tipo == "cerrar")
            {
                g.DrawLine(lapiz, 24, 24, 76, 76);
                g.DrawLine(lapiz, 76, 24, 24, 76);
            }
            else if (tipo == "varita")
            {
                // Destello (para "Autorellenado")
                PointF[] estrella = { new PointF(46, 12), new PointF(56, 38), new PointF(82, 48), new PointF(56, 58), new PointF(46, 84), new PointF(36, 58), new PointF(10, 48), new PointF(36, 38) };
                g.FillPolygon(pincel, estrella);
                g.FillEllipse(pincel, 70, 66, 16, 16);
                g.FillEllipse(pincel, 68, 12, 10, 10);
            }
            else
            {
                g.FillEllipse(pincel, 25, 25, 50, 50);
            }

            lapiz.Dispose();
            pincel.Dispose();
            g.Restore(estado);
        }
    }

    // Botón grande del menú: círculo con un icono + título + descripción corta.
    // Se usa igual que un botón: tarjeta.Click += ...
    public class Tarjeta : Control
    {
        Estilo estilo = new Estilo();
        string titulo;
        string descripcion;
        string icono;
        bool principal;       // true = naranja (el botón más importante)
        bool encima = false;  // true si el ratón está encima
        Font fuenteTitulo = new Font("Segoe UI", 11, FontStyle.Bold);
        Font fuenteDescripcion = new Font("Segoe UI", 9);

        public Tarjeta(string titulo, string descripcion, string icono, bool principal)
        {
            this.titulo = titulo;
            this.descripcion = descripcion;
            this.icono = icono;
            this.principal = principal;

            this.SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint
                | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.Selectable, true);
            this.TabStop = true;
            this.Cursor = Cursors.Hand;
            this.Size = new Size(310, 88);
            this.Text = titulo;
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            encima = true;
            Invalidate();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            encima = false;
            Invalidate();
            base.OnMouseLeave(e);
        }

        protected override void OnGotFocus(EventArgs e)
        {
            Invalidate();
            base.OnGotFocus(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            Invalidate();
            base.OnLostFocus(e);
        }

        // Con el teclado: Enter o espacio equivalen a hacer clic
        protected override bool IsInputKey(Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                return true;
            }
            return base.IsInputKey(keyData);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Space)
            {
                OnClick(EventArgs.Empty);
                e.Handled = true;
            }
            base.OnKeyDown(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            if (Parent != null)
            {
                g.Clear(Parent.BackColor);   // rellena las esquinas con el fondo de la ventana
            }

            float k = Height / 88f;   // por si la tarjeta cambia de tamaño
            Rectangle r = new Rectangle(1, 1, Width - 3, Height - 3);

            // Colores según sea principal o normal, y según el ratón
            Color fondo = Color.White;
            Color borde = Color.FromArgb(214, 222, 235);
            Color colorTitulo = estilo.ColorAzulNoche;
            Color colorDescripcion = Color.FromArgb(100, 110, 125);
            Color fondoCirculo = Color.FromArgb(226, 236, 252);
            Color colorIcono = estilo.ColorAzul;

            if (principal)
            {
                fondo = estilo.ColorNaranja;
                borde = estilo.ColorNaranja;
                colorTitulo = Color.White;
                colorDescripcion = Color.FromArgb(240, 255, 255, 255);
                fondoCirculo = Color.FromArgb(70, 255, 255, 255);
                colorIcono = Color.White;
                if (encima)
                {
                    fondo = Color.FromArgb(238, 95, 50);
                    borde = fondo;
                }
            }
            else if (encima)
            {
                fondo = Color.FromArgb(240, 246, 255);
                borde = estilo.ColorAzul;
            }

            // Tarjeta
            using (GraphicsPath ruta = Estilo.RectRedondo(r, 14))
            using (SolidBrush pincelFondo = new SolidBrush(fondo))
            using (Pen lapizBorde = new Pen(borde, encima ? 2 : 1))
            {
                g.FillPath(pincelFondo, ruta);
                g.DrawPath(lapizBorde, ruta);
            }

            // Círculo con el icono
            float d = 52 * k;
            RectangleF circulo = new RectangleF(18 * k, (Height - d) / 2f, d, d);
            using (SolidBrush pincelCirculo = new SolidBrush(fondoCirculo))
            {
                g.FillEllipse(pincelCirculo, circulo);
            }
            float margen = 12 * k;
            estilo.DibujarIcono(g, icono, new RectangleF(circulo.X + margen, circulo.Y + margen, d - 2 * margen, d - 2 * margen), colorIcono);

            // Título y descripción
            float x = circulo.Right + 16 * k;
            float ancho = Width - x - 12;
            float alturaTexto = fuenteTitulo.Height + fuenteDescripcion.Height + 2;
            float y = (Height - alturaTexto) / 2f;
            StringFormat formato = new StringFormat();
            formato.Trimming = StringTrimming.EllipsisCharacter;
            formato.FormatFlags = StringFormatFlags.NoWrap;
            using (SolidBrush pincelTitulo = new SolidBrush(colorTitulo))
            using (SolidBrush pincelDescripcion = new SolidBrush(colorDescripcion))
            {
                g.DrawString(titulo, fuenteTitulo, pincelTitulo, new RectangleF(x, y, ancho, fuenteTitulo.Height), formato);
                g.DrawString(descripcion, fuenteDescripcion, pincelDescripcion, new RectangleF(x, y + fuenteTitulo.Height + 2, ancho, fuenteDescripcion.Height), formato);
            }

            // Marco de foco cuando se llega con el teclado (Tab)
            if (Focused)
            {
                using (GraphicsPath ruta = Estilo.RectRedondo(new Rectangle(4, 4, Width - 9, Height - 9), 11))
                using (Pen lapizFoco = new Pen(principal ? Color.White : estilo.ColorAzul, 1))
                {
                    lapizFoco.DashStyle = DashStyle.Dot;
                    g.DrawPath(lapizFoco, ruta);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                fuenteTitulo.Dispose();
                fuenteDescripcion.Dispose();
            }
            base.Dispose(disposing);
        }
    }

}