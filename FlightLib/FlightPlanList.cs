using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightLib
{
    public class FlightPlanList
    {
        List<FlightPlan> vuelos = new List<FlightPlan>(); // antes era un vector de 10

        Stack<List<Position>> historial = new Stack<List<Position>>();
    


        // Datos del conflicto futuro encontrado (los rellena BuscarConflictoFuturo)
        int conflictoA = -1;
        int conflictoB = -1;
        double conflictoDistancia = 0;
        double conflictoTiempo = 0;

        // Datos de la pareja de vuelos más cercana (los rellena DistanciaMinimaActual)
        int parejaA = -1;
        int parejaB = -1;

        // Añade un vuelo al final de la lista
        public void AddFlightPlan(FlightPlan p)
        {
            vuelos.Add(p);
        }

        // Devuelve el vuelo de la posición i, o null si no existe
        public FlightPlan GetFlightPlan(int i)
        {
            if (i < 0 || i >= vuelos.Count)
            {
                return null;
            }

            return vuelos[i];
        }

        // Devuelve el número de vuelos de la lista
        public int GetNumber()
        {
            return vuelos.Count;
        }

        // Borra los vuelos guardados para poder meter los nuevos
        public void Vaciar()
        {
            vuelos.Clear();
            historial.Clear();
        }

        // Carga los vuelos del fichero (id xi yi xf yf velocidad y, si se guardó a mitad, xa ya)
        // Devuelve 0 si va bien, -1 si no existe, -2 si hay datos mal escritos y -3 si hay una velocidad <= 0
        public int CargarFichero(string nombreFichero)
        {
            if (File.Exists(nombreFichero) == false)
            {
                return -1;
            }

            StreamReader F = null;
            int resultado = 0;
            List<FlightPlan> nuevos = new List<FlightPlan>();
            try
            {
                F = new StreamReader(nombreFichero);
                string linea = F.ReadLine();
                while (linea != null && resultado == 0)
                {
                    linea = linea.Trim();
                    if (linea != "") // las líneas vacías se saltan
                    {
                        string[] trozos = linea.Split(' ');

                        // 6 datos si es un fichero normal, 8 si se guardó con la posición actual
                        if (trozos.Length != 6 && trozos.Length != 8)
                        {
                            resultado = -2;
                        }
                        else
                        {
                            string id = trozos[0];
                            double xi = Convert.ToDouble(trozos[1]);
                            double yi = Convert.ToDouble(trozos[2]);
                            double xf = Convert.ToDouble(trozos[3]);
                            double yf = Convert.ToDouble(trozos[4]);
                            double velocidad = Convert.ToDouble(trozos[5]);

                            // Velocidad 0 o negativa: no se permite
                            if (velocidad <= 0)
                            {
                                resultado = -3;
                            }
                            else
                            {
                                FlightPlan plan = new FlightPlan(id, xi, yi, xf, yf, velocidad);
                                if (plan.VelocidadValida(velocidad) == false)
                                {
                                    resultado = -3;
                                }
                                else
                                {
                                    // El vuelo sigue desde donde estaba cuando se guardó
                                    if (trozos.Length == 8)
                                    {
                                        double xa = Convert.ToDouble(trozos[6]);
                                        double ya = Convert.ToDouble(trozos[7]);
                                        plan.SetCurrentPosition(new Position(xa, ya));
                                    }
                                    nuevos.Add(plan);
                                }
                            }
                        }
                    }
                    linea = F.ReadLine();
                }
            }
            catch (FormatException)
            {
                resultado = -2;
            }
            catch (OverflowException)
            {
                resultado = -2;
            }
            catch (IOException)
            {
                resultado = -1;
            }

            if (F != null)
            {
                F.Close();
            }

            // Solo cambiamos la lista si todo el fichero estaba bien
            if (resultado == 0)
            {
                vuelos = nuevos;
                historial.Clear();
            }
            return resultado;
        }
        // Devuelve true si se ha podido guardar
        public bool GuardarFichero(string nombreFichero)
        {
            try
            {
                StreamWriter W = new StreamWriter(nombreFichero);
                for (int i = 0; i < vuelos.Count; i++)
                {
                    FlightPlan p = vuelos[i];
                    string linea = p.GetId() + " " + p.GetInitialPosition().GetX() + " " + p.GetInitialPosition().GetY()
                        + " " + p.GetFinalPosition().GetX() + " " + p.GetFinalPosition().GetY() + " " + p.GetVelocidad()
                        + " " + p.GetCurrentPosition().GetX() + " " + p.GetCurrentPosition().GetY();
                    W.WriteLine(linea);
                }
                W.Close();
                return true;
            }
            catch (IOException)
            {
                return false;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
        }

        // Mueve un ciclo todos los vuelos que no han llegado, pero antes de mover guarda las posiciones en el historial para poder deshacer
        public void Mover(double tiempo)
        {
            if (TodosHanLlegado())
            {
                return; // no hay nada que mover ni que guardar
            }

            // Guardamos dónde está cada avión ahora
            List<Position> posiciones = new List<Position>();
            for (int i = 0; i < vuelos.Count; i++)
            {
                posiciones.Add(vuelos[i].GetCurrentPosition());
            }
            historial.Push(posiciones);

            // Movemos los que no han llegado
            for (int i = 0; i < vuelos.Count; i++)
            {
                if (vuelos[i].HasArrived() == false)
                {
                    vuelos[i].Move(tiempo);
                }
            }
        }

        // Deshace el último paso, vuelve a poner cada avión donde estaba antes de moverse
        public bool Deshacer()
        {
            if (historial.Count == 0)
            {
                return false;
            }

            List<Position> posiciones = historial.Pop();
            for (int i = 0; i < vuelos.Count; i++)
            {
                vuelos[i].SetCurrentPosition(posiciones[i]);
            }
            return true;
        }

        // Dice si todos los vuelos han llegado a su destino
        // Devuelve true si han llegado todos
        public bool TodosHanLlegado()
        {
            for (int i = 0; i < vuelos.Count; i++)
            {
                if (!vuelos[i].HasArrived())
                    return false;
            }
            return true;
        }

        // Vuelve a poner todos los vuelos en su posición inicial y borra el historial
        public void Restart()
        {
            for (int i = 0; i < vuelos.Count; i++)
            {
                vuelos[i].Restart();
            }
            historial.Clear();
        }

        // Vuelve a poner a cada vuelo la velocidad con la que se creó
        public void RestaurarVelocidadesIniciales()
        {
            for (int i = 0; i < vuelos.Count; i++)
            {
                vuelos[i].RestaurarVelocidadInicial();
            }
        }

        public double[] GetVelocidades()
        {
            double[] v = new double[vuelos.Count];
            for (int i = 0; i < vuelos.Count; i++)
                v[i] = vuelos[i].GetVelocidad();
            return v;
        }
        public void SetVelocidades(double[] v)
        {
            for (int i = 0; i < vuelos.Count && i < v.Length; i++)
                vuelos[i].SetVelocidad(v[i]);
        }

        // Conflicto AHORA (para pintar en rojo)
        public bool EnConflicto(int i, double D)
        {
            for (int j = 0; j < vuelos.Count; j++)
            {
                if (j != i && vuelos[i].Conflicto(vuelos[j], D))
                    return true;
            }
            return false;

        }
        public bool HayConflicto(double D)
        { 
            for (int i = 0; i < vuelos.Count; i++) if (EnConflicto(i, D))
            { 
                return true;
            }
            return false; 
        }

        // Busca la primera pareja de vuelos que entrará en conflicto si siguen como van
        // Guarda la pareja, la distancia mínima y el instante, lo miramos con los Get de abajo
        // Devuelve true si ha encontrado alguna
        public bool BuscarConflictoFuturo(double D)
        {
            conflictoA = -1;
            conflictoB = -1;
            conflictoDistancia = 0;
            conflictoTiempo = 0;

            for (int i = 0; i < vuelos.Count; i++)
            {
                for (int j = i + 1; j < vuelos.Count; j++)
                {
                    double d = vuelos[i].DistanciaMinima(vuelos[j]);
                    if (d < D)
                    {
                        conflictoA = i;
                        conflictoB = j;
                        conflictoDistancia = d;
                        conflictoTiempo = vuelos[i].TiempoMinimo(vuelos[j]);
                        return true;
                    }
                }
            }
            return false;
        }

        public int GetConflictoA()
        {
            return conflictoA;
        }

        public int GetConflictoB()
        {
            return conflictoB;
        }

        public double GetConflictoDistancia()
        {
            return conflictoDistancia;
        }

        public double GetConflictoTiempo()
        {
            return conflictoTiempo;
        }

        // Cuenta cuántas parejas de vuelos entrarán en conflicto y devuelve el número
        public int ContarConflictosFuturos(double D)
        {
            int n = 0;
            for (int i = 0; i < vuelos.Count; i++)
            {
                for (int j = i + 1; j < vuelos.Count; j++)
                {
                    if (vuelos[i].HabraConflicto(vuelos[j], D))
                    {
                        n++;
                    }
                }
            }
            return n;
        }

        // Cuenta con cuántos vuelos entrará en conflicto el vuelo f, mirando solo los vuelos de la posición 0 hasta "hasta" (sin incluirlo)
        private int ConflictosDe(int f, double D, int hasta)
        {
            int n = 0;
            for (int j = 0; j < hasta; j++)
            {
                if (j != f && vuelos[f].HabraConflicto(vuelos[j], D))
                {
                    n++;
                }
            }
            return n;
        }

        // Busca con la derivada una velocidad para el vuelo f que le quite conflictos
        private bool MejorarVuelo(int f, double D, int hasta)
        {
            double original = vuelos[f].GetVelocidad();
            int mejorConflictos = ConflictosDe(f, D, hasta);
            double mejorVelocidad = -1;

            for (int j = 0; j < vuelos.Count; j++)
            {
                if (j != f)
                {
                    List<double> candidatas = vuelos[f].VelocidadesSinConflicto(vuelos[j], D);
                    for (int k = 0; k < candidatas.Count; k++)
                    {
                        vuelos[f].SetVelocidad(candidatas[k]);  // probamos esta velocidad
                        int n = ConflictosDe(f, D, hasta);
                        vuelos[f].SetVelocidad(original);       // y dejamos la que había

                        bool menosConflictos = n < mejorConflictos;
                        bool cambiaMenos = n == mejorConflictos && mejorVelocidad > 0
                            && Math.Abs(candidatas[k] - original) < Math.Abs(mejorVelocidad - original);

                        if (menosConflictos || cambiaMenos)
                        {
                            mejorConflictos = n;
                            mejorVelocidad = candidatas[k];
                        }
                    }
                }
            }

            if (mejorVelocidad > 0)
            {
                vuelos[f].SetVelocidad(mejorVelocidad);
                return true;
            }
            return false;
        }

        // Propone velocidades para quitar todos los conflictos que se puedan
        public int ResolverConflictos(double D)
        {
            // Paso 1: en orden
            for (int k = 1; k < vuelos.Count; k++)
            {
                if (ConflictosDe(k, D, k) > 0)
                {
                    MejorarVuelo(k, D, k);
                }
            }

            // Paso 2: repasar lo que quede, mientras se consiga arreglar algo
            bool hayCambios = true;
            int pasada = 0;
            while (hayCambios == true && pasada < 10)
            {
                hayCambios = false;
                for (int i = 0; i < vuelos.Count; i++)
                {
                    for (int j = i + 1; j < vuelos.Count; j++)
                    {
                        if (vuelos[i].HabraConflicto(vuelos[j], D))
                        {
                            // Primero probamos con el segundo vuelo y, si no, con el primero
                            if (MejorarVuelo(j, D, vuelos.Count) == true)
                            {
                                hayCambios = true;
                            }
                            else if (MejorarVuelo(i, D, vuelos.Count) == true)
                            {
                                hayCambios = true;
                            }
                        }
                    }
                }
                pasada++;
            }
            return ContarConflictosFuturos(D);
        }

        // Distancia actual entre la pareja de vuelos más cercana
        public double DistanciaMinimaActual()
        {
            parejaA = -1;
            parejaB = -1;
            double minima = double.MaxValue;

            for (int i = 0; i < vuelos.Count; i++)
            {
                for (int j = i + 1; j < vuelos.Count; j++)
                {
                    double d = vuelos[i].GetCurrentPosition().Distancia(vuelos[j].GetCurrentPosition());
                    if (d < minima)
                    {
                        minima = d;
                        parejaA = i;
                        parejaB = j;
                    }
                }
            }
            return minima;
        }

        public int GetParejaA()
        {
            return parejaA;
        }

        public int GetParejaB()
        {
            return parejaB;
        }

        // Escribe en la consola los datos de todos los vuelos
        public void EscribeConsola()
        {
            int i = 0;
            while (i < vuelos.Count)
            {
                vuelos[i].EscribeConsola();
                i++;
            }
        }
    }
}