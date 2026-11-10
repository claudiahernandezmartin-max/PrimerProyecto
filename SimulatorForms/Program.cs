using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimulatorForms
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Login login = new Login();   // Crea la ventana de login
            login.ShowDialog();                  // La muestra y espera a que se cierre

            if (login.GetAceptado())             // Esta bien?
            {
                Application.Run(new Menu());     // Si es que sí, se abre el Menu como siempre
            }
            // si no ha entrado (ha cerrado la ventana o falla la base de datos), no se abre nada y el programa termina
        }
    }
}
