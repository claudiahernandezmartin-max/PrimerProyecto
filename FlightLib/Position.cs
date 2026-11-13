using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightLib
{
    public class Position
    {
        double x; // coordenada X (2D)
        double y; // coordenada Y (2D)

        // Constructores

        public Position(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        // Métodos

        public double GetX()
        // getter del atributo x
        { 
            return x; 
        }

        public double GetY()
        // getter del atributo y
        { 
            return y; 
        }

        // Calcula la distancia entre este punto y el punto b
        // Devuelve la distancia
        public double Distancia(Position b)
        {
            double resultado = Math.Sqrt((x - b.x) * (x - b.x) + (y - b.y) * (y - b.y));
            return resultado;
        }
    }
}
