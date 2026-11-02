using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightLib
{
    public class FlightPlanList
    {
        FlightPlan[] vector = new FlightPlan[10];
        int number = 0;

        public int AddFlightPlan(FlightPlan p)
        {

            if (number == 10)
            {
                return -1;
            }

            vector[number] = p;
            number++;
            return 0;
        }
        public FlightPlan GetFlightPlan(int i)
        {
            if (i < 0 || i >= number)
            {
                return null;
            }

            return vector[i];
        }
        // Borra los vuelos guardados para poder meter los nuevos
        public void Vaciar()
        {
            number = 0;
        }
        public void Mover(double tiempo)
        {
            int i = 0;
            while (i < number)
            {
                if (vector[i].HasArrived() == false)
                {
                    vector[i].Move(tiempo);
                }
                i++;
            }

        }
        public int GetNumber() 
        { 
            return number; 
        }

        public bool TodosHanLlegado()
        {
            for (int i = 0; i < number; i++)
            {
                if (!vector[i].HasArrived())
                    return false;
            }
            return true; 
        }

        public void Restart()
        {
            for (int i = 0; i < number; i++)
                vector[i].Restart();
        }
        public void RestaurarVelocidadesIniciales() 
        { 
            for (int i = 0; i < number; i++) 
                vector[i].RestaurarVelocidadInicial(); 
        }

        public double[] GetVelocidades()
        { 
            double[] v = new double[number]; 
            for (int i = 0; i < number; i++) 
                v[i] = vector[i].GetVelocidad(); 
            return v; 
        }
        public void SetVelocidades(double[] v)
        { 
            for (int i = 0; i < number && i < v.Length; i++) 
                vector[i].SetVelocidad(v[i]); 
        }

        // Conflicto AHORA (para pintar en rojo)
        public bool EnConflicto(int i, double D)
        {
            for (int j = 0; j < number; j++)
            {
                if (j != i && vector[i].Conflicto(vector[j], D))
                    return true;
            }
                return false;
            
        }
        public bool HayConflicto(double D)
        { for (int i = 0; i < number; i++) if (EnConflicto(i, D)) return true; return false; }

        // Conflicto FUTURO: primera pareja que entrará en conflicto
        public bool BuscarConflictoFuturo(double D, out int a, out int b, out double dMin, out double t)
        {
            a = -1; b = -1; dMin = 0; t = 0;
            for (int i = 0; i < number; i++)
            {
                for (int j = i + 1; j < number; j++)
                {
                    double tt, d = vector[i].DistanciaMinima(vector[j], out tt);
                    if (d < D)
                    {
                        a = i; b = j; dMin = d; t = tt;
                        return true;
                    }
                }
            }
            return false;
        }
        public bool HabraConflicto(double D)
        { 
            int a, b; double d, t; 
            return BuscarConflictoFuturo(D, out a, out b, out d, out t); 
        }

        // Propone velocidades: frena un vuelo de la pareja (99%..10%) hasta que no haya conflictos
        public bool ResolverConflictos(double D)
        {
            double[] orig = GetVelocidades();
            int a, b, intentos = 0; double d, t;
            while (BuscarConflictoFuturo(D, out a, out b, out d, out t) && intentos < 100)
            {
                if (!Frenar(b, a, D) && !Frenar(a, b, D)) { SetVelocidades(orig); return false; }
                intentos++;
            }
            if (HabraConflicto(D)) 
            {
                SetVelocidades(orig); 
                return false;
            }
            return true;
        }
        private bool Frenar(int f, int o, double D)
        {
            double v0 = vector[f].GetVelocidad();
            for (int p = 99; p >= 10; p--)
            {
                vector[f].SetVelocidad(v0 * p / 100.0);
                if (!vector[f].HabraConflicto(vector[o], D)) 
                    return true;
            }
            vector[f].SetVelocidad(v0);
            return false;
        }

        // Distancia actual entre la pareja de vuelos más cercana
        public double DistanciaMinimaActual(out int a, out int b)
        {
            a = -1;
            b = -1;
            double minima = double.MaxValue;

            for (int i = 0; i < number; i++)
            {
                for (int j = i + 1; j < number; j++)
                {
                    double d = vector[i].GetCurrentPosition().Distancia(vector[j].GetCurrentPosition());
                    if (d < minima)
                    {
                        minima = d;
                        a = i;
                        b = j;
                    }
                }
            }
            return minima;
        }
        public void EscribeConsola()
        {
            int i = 0;
            while (i < number)
            {
                vector[i].EscribeConsola();
                i++;
            }
        }
    }
}