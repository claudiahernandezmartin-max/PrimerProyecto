using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Métode sempre s'ha de tenir en compte  a quina classe s'afegeix
namespace FlightLib
{
    public class FlightPlan
    {
        // Atributos

        string id; // identificador
        Position currentPosition; // posicion actual
        Position finalPosition; // posicion final
        Position initialPosition; // posicion inicial
        double velocidad;
        double velocidadInicial; // para el Reset
        double velocidadMaxima = 600; // ningún avión puede ir más rápido que esto

        // Resultado del último cálculo de CalcularMinimo
        double minimoDistancia;
        double minimoTiempo;

        // Constructures
        public FlightPlan(string id, double cpx, double cpy, double fpx, double fpy, double velocidad)
        {
            // Velocidades negativas NO
            if (velocidad <= 0)
            {
                throw new ArgumentException("La velocidad tiene que ser mayor que 0.");
            }
            this.id = id;
            this.currentPosition = new Position(cpx, cpy);
            this.finalPosition = new Position(fpx, fpy);
            this.initialPosition = new Position(cpx, cpy);
            this.velocidad = velocidad;
            this.velocidadInicial = velocidad;
        }

        // Dice si una velocidad está permitida: mayor que 0 y no más que la máxima
        public bool VelocidadValida(double v)
        {
            return v > 0 && v <= velocidadMaxima;
        }

        // getter de la velocidad máxima permitida
        public double GetVelocidadMaxima()
        {
            return velocidadMaxima;
        }

        // Gets i Sets

        // getter de atribut velocidad
        public double GetVelocidad()
        {
            return velocidad;
        }

        // setter de atribut velocidad
        // Solo cambia la velocidad si es válida. Devuelve true si la ha cambiado y false si no
        public bool SetVelocidad(double velocidad)
        {
            if (VelocidadValida(velocidad) == false)
            {
                return false;
            }
            this.velocidad = velocidad;
            return true;
        }

        // getter de atribut id
        public string GetId()
        {
            return id;
        }

        // setter de atribut id
        public void SetId(string id)
        {
            this.id = id;
        }

        // Getter de atribut CurrentPosition
        public Position GetCurrentPosition()
        {
            return currentPosition;
        }
       
        // setter de atribut CurrentPosition
        public void SetCurrentPosition(Position currentPosition)
        {
            this.currentPosition = currentPosition;
        }

        // Getter de atribut FinalPosition
        public Position GetFinalPosition()
        {
            return finalPosition;
        }

        // Setter de atribut FinalPosition
        public void SetFinalPosition(Position finalPosition)
        {
            this.finalPosition = finalPosition;
        }

        // Getter de atribut InitialPosition
        public Position GetInitialPosition()
        {
            return initialPosition;
        }

        // Setter de atribut InitialPosition
        public void SetInitialPosition(Position initialPosition)
        {
            this.initialPosition = initialPosition;
        }

        // Funcions

        public void Move(double tiempo)
        // Mueve el vuelo a la posición correspondiente a viajar durante el tiempo que se recibe como parámetro
        {
            //Calculamos la distancia recorrida en el tiempo dado
            double distancia = tiempo * this.velocidad / 60;

            //Calculamos las razones trigonométricas
            double hipotenusa = Math.Sqrt((finalPosition.GetX() - currentPosition.GetX()) * (finalPosition.GetX() - currentPosition.GetX()) + (finalPosition.GetY() - currentPosition.GetY()) * (finalPosition.GetY() - currentPosition.GetY()));
            if (hipotenusa == 0) 
            { 
                currentPosition = finalPosition; return; 
            }
            double coseno = (finalPosition.GetX() - currentPosition.GetX()) / hipotenusa;
            double seno = (finalPosition.GetY() - currentPosition.GetY()) / hipotenusa;

            //Caculamos la nueva posición del vuelo
            double x = currentPosition.GetX() + distancia * coseno;
            double y = currentPosition.GetY() + distancia * seno;

            //Modificar métode perquè no es passi del destí
            Position nextPosition = new Position(x, y);
            if (currentPosition.Distancia(nextPosition)<hipotenusa)
            {
                currentPosition = nextPosition;
            }
            else
            {
                currentPosition = finalPosition;
            }
        }
        //Métode arribar al seu destí
        public bool HasArrived()
        {
            bool resultado = false;
            if (currentPosition.GetX() == finalPosition.GetX() && currentPosition.GetY() == finalPosition.GetY())
            {
                resultado = true;
            }
            return resultado;
        }

        public void Restart()
        {
            currentPosition = initialPosition;
        }
        public void RestaurarVelocidadInicial() 
        { 
            velocidad = velocidadInicial; 
        }
        public double GetVelocidadInicial() 
        { 
            return velocidadInicial; 
        }
        public string GetCompania()
        {
            // Partimos el identificador por los números: la primera parte son las letras
            string[] trozos = id.Split('0', '1', '2', '3', '4', '5', '6', '7', '8', '9');
            string prefijo = trozos[0];

            string compania = "Desconocida";
            if (prefijo == "IB")
            {
                compania = "Iberia";
            }
            else if (prefijo == "VY" || prefijo == "VLG")
            {
                compania = "Vueling";
            }
            else if (prefijo == "RYR")
            {
                compania = "Ryanair";
            }
            else if (prefijo == "EZY")
            {
                compania = "easyJet";
            }
            else if (prefijo == "AEA")
            {
                compania = "Air Europa";
            }
            else if (prefijo == "TAP")
            {
                compania = "TAP Air Portugal";
            }
            else if (prefijo == "KLM")
            {
                compania = "KLM";
            }
            return compania;
        }
        //Fer métode que detecti quan hi ha dos vols que estan més aprop de la distància de seguretat 
        public bool Conflicto(FlightPlan b, double distanciaSeguridad)
        {  
            bool conflicto = false;
            if (this.currentPosition.Distancia(b.currentPosition) < distanciaSeguridad)
                conflicto = true;
             
            return conflicto;
        }

        public double TiempoHastaLlegada()
        { 
            return currentPosition.Distancia(finalPosition) / (velocidad / 60); 
        }

        public double GetVelocidadX()
        {
            double hipotenusa = currentPosition.Distancia(finalPosition);
            double vx = 0;
            if (hipotenusa != 0)
            {
                double coseno = (finalPosition.GetX() - currentPosition.GetX()) / hipotenusa;
                vx = (velocidad / 60) * coseno;
            }
            return vx;
        }

        // Componente Y de la velocidad (igual que el seno de Move). 0 si ya ha llegado
        public double GetVelocidadY()
        {
            double hipotenusa = currentPosition.Distancia(finalPosition);
            double vy = 0;
            if (hipotenusa != 0)
            {
                double seno = (finalPosition.GetY() - currentPosition.GetY()) / hipotenusa;
                vy = (velocidad / 60) * seno;
            }
            return vy;
        }

        public Position PosicionEnTiempo(double t)
        {
            Position resultado = finalPosition;
            if (t < TiempoHastaLlegada())
            {
                double x = currentPosition.GetX() + GetVelocidadX() * t;
                double y = currentPosition.GetY() + GetVelocidadY() * t;
                resultado = new Position(x, y);
            }
            return resultado;
        }
        // Instante entre inicio y fin en el que este avión y b están más cerca y se calcula con la derivada de la distancia al cuadrado igualada a 0
        private double MinimoEnTramo(FlightPlan b, double inicio, double fin)
        {
            // Dónde están los dos al empezar el tramo
            Position pa = PosicionEnTiempo(inicio);
            Position pb = b.PosicionEnTiempo(inicio);

            // Velocidad de cada uno en el tramo (si ya ha llegado, está parado)
            double vax = 0;
            double vay = 0;
            double vbx = 0;
            double vby = 0;
            if (inicio < TiempoHastaLlegada())
            {
                vax = GetVelocidadX();
                vay = GetVelocidadY();
            }
            if (inicio < b.TiempoHastaLlegada())
            {
                vbx = b.GetVelocidadX();
                vby = b.GetVelocidadY();
            }

            // Posición y velocidad de b respecto a este avión
            double px = pb.GetX() - pa.GetX();
            double py = pb.GetY() - pa.GetY();
            double vx = vbx - vax;
            double vy = vby - vay;

            // d²(s) = (px + vx·s)² + (py + vy·s)²
            // Derivada: 2·(px + vx·s)·vx + 2·(py + vy·s)·vy = 0  ->  s = -(px·vx + py·vy) / (vx² + vy²)
            double s = 0;
            double v2 = vx * vx + vy * vy;
            if (v2 > 0)
            {
                s = -(px * vx + py * vy) / v2;
            }

            // El mínimo tiene que caer dentro del tramo
            if (s < 0)
            {
                s = 0;
            }
            if (s > fin - inicio)
            {
                s = fin - inicio;
            }
            return inicio + s;
        }

        // Calcula cuándo y a qué distancia pasarán más cerca este avión y b
        private void CalcularMinimo(FlightPlan b)
        {
            double primeraLlegada = Math.Min(TiempoHastaLlegada(), b.TiempoHastaLlegada());
            double ultimaLlegada = Math.Max(TiempoHastaLlegada(), b.TiempoHastaLlegada());

            // Tramo 1: desde ahora hasta que llega el primero
            minimoTiempo = MinimoEnTramo(b, 0, primeraLlegada);
            minimoDistancia = PosicionEnTiempo(minimoTiempo).Distancia(b.PosicionEnTiempo(minimoTiempo));

            // Tramo 2: desde que llega el primero hasta que llega el segundo
            double t2 = MinimoEnTramo(b, primeraLlegada, ultimaLlegada);
            double d2 = PosicionEnTiempo(t2).Distancia(b.PosicionEnTiempo(t2));
            if (d2 < minimoDistancia)
            {
                minimoDistancia = d2;
                minimoTiempo = t2;
            }
        }

        // Instante (en minutos) en el que los dos aviones están más cerca
        public double TiempoMinimo(FlightPlan b)
        {
            CalcularMinimo(b);
            return minimoTiempo;
        }

        // Distancia mínima a la que se acercarán los dos aviones
        public double DistanciaMinima(FlightPlan b)
        {
            CalcularMinimo(b);
            return minimoDistancia;
        }
        // Dice si, siguiendo como van, los dos aviones entrarán en conflicto
        public bool HabraConflicto(FlightPlan b, double distanciaSeguridad)
        {
            return DistanciaMinima(b) < distanciaSeguridad;
        }
        // Calcula con la derivada las velocidades de este avión con las que pasaría justo a la distancia de seguridad del avión b (una frenando y otra acelerando).
        
        public List<double> VelocidadesSinConflicto(FlightPlan b, double D)
        {
            List<double> resultado = new List<double>();

            double hipotenusa = currentPosition.Distancia(finalPosition);
            if (hipotenusa == 0)
            {
                return resultado; // ya ha llegado: cambiar su velocidad no sirve
            }

            // u = dirección de este avión (vector de módulo 1)
            double ux = (finalPosition.GetX() - currentPosition.GetX()) / hipotenusa;
            double uy = (finalPosition.GetY() - currentPosition.GetY()) / hipotenusa;

            // P = posición relativa (este - b) y vB = velocidad del otro avión
            double px = currentPosition.GetX() - b.currentPosition.GetX();
            double py = currentPosition.GetY() - b.currentPosition.GetY();
            double vbx = b.GetVelocidadX();
            double vby = b.GetVelocidadY();

            // Con k = nueva velocidad / 60, la velocidad relativa es V = k·u - vB.
            // Con la derivada sale t* = -(P·V)/|V|² y la distancia mínima d_min² = (P×V)² / |V|².
            // Si pedimos d_min = D queda una ecuación de segundo grado en k: a·k² + bb·k + c = 0
            double c1 = px * uy - py * ux;      // P×u
            double c0 = px * vby - py * vbx;    // P×vB
            double uv = ux * vbx + uy * vby;    // u·vB
            double vb2 = vbx * vbx + vby * vby; // |vB|²

            double a = c1 * c1 - D * D;
            double bb = 2 * (D * D * uv - c1 * c0);
            double c = c0 * c0 - D * D * vb2;

            // Soluciones de la ecuación de segundo grado
            List<double> soluciones = new List<double>();
            if (a != 0)
            {
                double discriminante = bb * bb - 4 * a * c;
                if (discriminante >= 0)
                {
                    soluciones.Add((-bb + Math.Sqrt(discriminante)) / (2 * a));
                    soluciones.Add((-bb - Math.Sqrt(discriminante)) / (2 * a));
                }
            }
            else if (bb != 0)
            {
                soluciones.Add(-c / bb);
            }

            // Pasamos k a velocidad y probamos un 1% por debajo y por encima para no quedarnos justo en D
            for (int i = 0; i < soluciones.Count; i++)
            {
                double v = soluciones[i] * 60;
                if (VelocidadValida(v * 0.99))
                {
                    resultado.Add(v * 0.99);
                }
                if (VelocidadValida(v * 1.01))
                {
                    resultado.Add(v * 1.01);
                }
            }
            return resultado;
        }

        public void EscribeConsola()
        // escribe en consola los datos del plan de vuelo
        {
            //Millorem l'estètica
            Console.WriteLine("******************************");
            Console.WriteLine("Datos del vuelo: ");
            Console.WriteLine("Identificador: {0}", id);
            //Dades reals amb 2 decimals, posem el float
            Console.WriteLine("Velocidad: {0:F2}", velocidad);
            Console.WriteLine("Posición actual: ({0:F2},{1:F2})", currentPosition.GetX(), currentPosition.GetY());
            if (this.HasArrived()) 
            {
                Console.WriteLine("Ha llegado al destino ");
            }
            Console.WriteLine("******************************");
        }
    }
}
