using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection.Emit;
using System.Security.Policy;
using System.Windows.Forms;

namespace SimulatorForms
{
    // Esta clase guarda el "diseño" de toda la aplicación:
    // colores, letras y cómo se ven los botones.
    // Así todos los formularios se ven iguales.
    public static class Estilo
    {
        // ---------------- COLORES ----------------
        public static Color ColorFondo = Color.FromArgb(246, 247, 249);      // gris casi blanco
        public static Color ColorTexto = Color.FromArgb(40, 44, 52);         // gris muy oscuro
        public static Color ColorPrincipal = Color.FromArgb(20, 33, 61);     // azul marino
        public static Color ColorSecundario = Color.FromArgb(45, 66, 112);   // azul marino más claro
        public static Color ColorResalte = Color.FromArgb(252, 163, 17);     // ámbar
        public static Color ColorGris = Color.FromArgb(120, 125, 135);       // gris para textos pequeños

        public static Color ColorAvion1 = Color.FromArgb(0, 119, 182);       // azul
        public static Color ColorAvion2 = Color.FromArgb(230, 57, 70);       // rojo

        // ---------------- LETRAS ----------------
        public static Font FuenteNormal = new Font("Segoe UI", 10);
        public static Font FuenteBoton = new Font("Segoe UI Semibold", 10);
        public static Font FuenteSeccion = new Font("Segoe UI Semibold", 14);
        public static Font FuenteTitulo = new Font("Segoe UI Semibold", 26);
        public static Font FuenteSubtitulo = new Font("Segoe UI", 12);

        // ---------------- FORMULARIO ----------------
        // Pone el fondo, centra la ventana y da estilo a todo lo que hay dentro
        public static void AplicarFormulario(Form formulario)
        {
            formulario.BackColor = ColorFondo;
            formulario.StartPosition = FormStartPosition.CenterScreen;

            // Recorremos uno por uno todos los controles del formulario
            foreach (Control control in formulario.Controls)
            {
                if (control is Button)
                {
                    AplicarBoton((Button)control);
                }
                if (control is TextBox)
                {
                    control.Font = FuenteNormal;
                    control.ForeColor = ColorTexto;
                    ((TextBox)control).BorderStyle = BorderStyle.FixedSingle;
                }
                if (control is MenuStrip)
                {
                    AplicarMenu((MenuStrip)control);
                }
            }
        }

        // ---------------- BOTONES ----------------
        public static void AplicarBoton(Button boton)
        {
            boton.FlatStyle = FlatStyle.Flat;                         // botón plano, sin relieve
            boton.FlatAppearance.BorderSize = 0;                      // sin borde
            boton.UseVisualStyleBackColor = false;
            boton.BackColor = ColorPrincipal;                         // fondo azul marino
            boton.ForeColor = Color.White;                            // letra blanca
            boton.FlatAppearance.MouseOverBackColor = ColorSecundario; // color al pasar el ratón
            boton.FlatAppearance.MouseDownBackColor = ColorResalte;    // color al hacer clic
            boton.Font = FuenteBoton;
            boton.Cursor = Cursors.Hand;                              // el cursor se vuelve una mano
            boton.Size = new Size(160, 44);                           // todos del mismo tamaño
        }

        // ---------------- MENÚ ----------------
        public static void AplicarMenu(MenuStrip menu)
        {
            menu.BackColor = Color.White;
            menu.Font = FuenteBoton;

            foreach (ToolStripItem opcion in menu.Items)
            {
                opcion.ForeColor = ColorPrincipal;
            }
        }

        // ---------------- LÁPICES PARA DIBUJAR AVIONES ----------------
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
            SolidBrush pincel = new SolidBrush(Color.FromArgb(30, color));
            return pincel;
        }
    }
}