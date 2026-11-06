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

        // Añade un vuelo al final de la lista
        public void AddFlightPlan(FlightPlan p)
        {
            vuelos.Add(p);
        }
        public FlightPlan GetFlightPlan(int i)
        {
            if (i < 0 || i >= vuelos.Count)
            {
                return null;
            }

            return vuelos[i];
        }
        // Borra los vuelos guardados para poder meter los nuevos
        public void Vaciar()
        {
            vuelos.Clear();
        }

        // Carga los vuelos del fichero (una línea por vuelo: id xi yi xf yf velocidad)
        // Devuelve 0 si va bien, -1 si el fichero no existe y -2 si hay datos mal escritos
        public int CargarFichero(string nombreFichero)
        {
            StreamReader F = null;
            try
            {
                F = new StreamReader(nombreFichero);

                // Los guardamos aparte para no perder los vuelos que había si el fichero está mal
                List<FlightPlan> nuevos = new List<FlightPlan>();

                string linea = F.ReadLine();
                while (linea != null)
                {
                    string[] trozos = linea.Split(' ');
                    string id = trozos[0];
                    double xi = Convert.ToDouble(trozos[1]);
                    double yi = Convert.ToDouble(trozos[2]);
                    double xf = Convert.ToDouble(trozos[3]);
                    double yf = Convert.ToDouble(trozos[4]);
                    double velocidad = Convert.ToDouble(trozos[5]);

                    nuevos.Add(new FlightPlan(id, xi, yi, xf, yf, velocidad));
                    linea = F.ReadLine();
                }
                F.Close();

                vuelos = nuevos;
                return 0;
            }
            catch (FileNotFoundException)
            {
                return -1;
            }
            catch (FormatException)
            {
                F.Close();
                return -2;
            }
        }

        public void Mover(double tiempo)
        {
            int i = 0;
            while (i < vuelos.Count)
            {
                if (vuelos[i].HasArrived() == false)
                {
                    vuelos[i].Move(tiempo);
                }
                i++;
            }

        }
        public int GetNumber()
        {
            return vuelos.Count;
        }

        public bool TodosHanLlegado()
        {
            for (int i = 0; i < vuelos.Count; i++)
            {
                if (!vuelos[i].HasArrived())
                    return false;
            }
            return true;
        }

        public void Restart()
        {
            for (int i = 0; i < vuelos.Count; i++)
                vuelos[i].Restart();
        }
        public void RestaurarVelocidadesIniciales()
        {
            for (int i = 0; i < vuelos.Count; i++)
                vuelos[i].RestaurarVelocidadInicial();
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
        { for (int i = 0; i < vuelos.Count; i++) if (EnConflicto(i, D)) return true; return false; }

        // Datos del conflicto futuro encontrado (los rellena BuscarConflictoFuturo)
        int conflictoA = -1;
        int conflictoB = -1;
        double conflictoDistancia = 0;
        double conflictoTiempo = 0;

        // Datos de la pareja más cercana (los rellena DistanciaMinimaActual)
        int parejaA = -1;
        int parejaB = -1;

        // Conflicto FUTURO: busca la primera pareja que entrará en conflicto
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

        public bool HabraConflicto(double D)
        {
            return BuscarConflictoFuturo(D);
        }

        // Propone velocidades: frena un vuelo de la pareja (99%..10%) hasta que no haya conflictos
        public bool ResolverConflictos(double D)
        {
            double[] orig = GetVelocidades();
            int intentos = 0;

            while (BuscarConflictoFuturo(D) == true && intentos < 100)
            {
                int a = conflictoA;
                int b = conflictoB;

                bool frenado = Frenar(b, a, D);
                if (frenado == false)
                {
                    frenado = Frenar(a, b, D);
                }
                if (frenado == false)
                {
                    SetVelocidades(orig);
                    return false;
                }
                intentos++;
            }

            if (HabraConflicto(D) == true)
            {
                SetVelocidades(orig);
                return false;
            }
            return true;
        }

        private bool Frenar(int f, int o, double D)
        {
            double v0 = vuelos[f].GetVelocidad();
            for (int p = 99; p >= 10; p--)
            {
                vuelos[f].SetVelocidad(v0 * p / 100.0);
                if (!vuelos[f].HabraConflicto(vuelos[o], D))
                    return true;
            }
            vuelos[f].SetVelocidad(v0);
            return false;
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