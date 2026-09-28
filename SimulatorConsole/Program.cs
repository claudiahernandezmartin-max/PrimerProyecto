using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Utilitzar la llibreria
using FlightLib;

namespace SimulatorConsole
{
    public class Program
    {
        static void Main(string[] args)
        {
            //Programa principal te una llista
            FlightPlanList list = new FlightPlanList();

            try 
            {
                Console.WriteLine("Escribe el identificador");
                //   string nombre = Console.ReadLine();
                string identificador = Console.ReadLine(); ;

                Console.WriteLine("Escribe la velocidad");
                double velocidad = Convert.ToDouble(Console.ReadLine());

                Console.WriteLine("Escribe las coordenadas de la posición inicial, separadas por un blanco");
                string linea = Console.ReadLine();
                string[] trozos = linea.Split(' ');
                double ix = Convert.ToDouble(trozos[0]);
                double iy = Convert.ToDouble(trozos[1]);

                Console.WriteLine("Escribe las coordenadas de la posición final, separadas por un blanco");
                linea = Console.ReadLine();
                trozos = linea.Split(' ');
                double fx = Convert.ToDouble(trozos[0]);
                double fy = Convert.ToDouble(trozos[1]);

                //Crear el pla de vol amb les dades que hem llegit
                FlightPlan plan_a = new FlightPlan(identificador, ix, iy, fx, fy, velocidad);


                //Segon Pla de vol pel métode de distáncia de seguretat
                Console.WriteLine("Escribe el identificador");
                
                //   Treiem el double i string per cerar una segona variable de cada
                identificador = Console.ReadLine(); ;

                Console.WriteLine("Escribe la velocidad");
                velocidad = Convert.ToDouble(Console.ReadLine());

                Console.WriteLine("Escribe las coordenadas de la posición inicial, separadas por un blanco");
                linea = Console.ReadLine();
                trozos = linea.Split(' ');
                ix = Convert.ToDouble(trozos[0]);
                iy = Convert.ToDouble(trozos[1]);

                Console.WriteLine("Escribe las coordenadas de la posición final, separadas por un blanco");
                linea = Console.ReadLine();
                trozos = linea.Split(' ');
                fx = Convert.ToDouble(trozos[0]);
                fy = Convert.ToDouble(trozos[1]);

                //Instancia d'un objecte del tipus FlightPlan
                FlightPlan plan_b = new FlightPlan(identificador, ix, iy, fx, fy, velocidad);

                //Estem fent que del vector que hem creat a la classe Flight Plan List apareixin al programa principal on es presenten les dades de cada llista guardades al flight plan A o B
                list.AddFlightPlan(plan_a);
                list.AddFlightPlan(plan_b);

                //Fem el bucle de moure i escriure el pla de vol
                int i = 0;
                int ciclos = 15;
                int tiempoCiclo = 10;
                double distanciaSeguridad = 10;

                //Contador més petit que elnúmero de cicles
                while (i < ciclos) 
                {
                    //Considerant els vectors del Flight Plan ,ara tenen una nova forma de moure's. Hem cambiat on aparegui plan_a per GetFlightPlan(0) i plan_b per (1)

                    //list.GetFlightPlan(0).Mover(tiempoCiclo);
                    //plan_a.EscribeConsola();
                    //list.GetFlightPlan(1).Mover(tiempoCiclo);
                    //plan_b.EscribeConsola();
                    //if (plan_a.Conflicto(plan_b,distanciaSeguridad))
                    //Console.WriteLine("Conflicto detectado");
                    //i++;

                    //Afegim una part que permet moure la llista completa i no un per un, convervem el codi de moure un per un adalt. 
                    list.Mover(tiempoCiclo);
                    list.EscribeConsola();
                    //Això significa no m'importa quans hik ha dins del vector, perque se que el codi esta dins de la classe llsita.
                  
                    if (plan_a.Conflicto(plan_b,distanciaSeguridad))
                    Console.WriteLine("Conflicto detectado");
                    i++;
                }

                Console.ReadLine();
            }
            catch (FormatException) 
            {
                Console.WriteLine("Error de formato");
                Console.ReadLine();
            }
        }
    }
}
