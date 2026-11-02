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

        // Gets i Sets

        // getter de atribut velocidad
        public double GetVelocidad()
        {
            return velocidad;
        }

        // setter de atribut velocidad
        public void SetVelocidad(double velocidad)
        {
            // Velocidades negativas NO
            if (velocidad <= 0)
            {
                throw new ArgumentException("La velocidad tiene que ser mayor que 0.");
            }
            this.velocidad = velocidad;
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
        // Instante (en minutos) en el que los dos aviones están más cerca
        public double TiempoMinimo(FlightPlan b)
        {
            // Posición y velocidad de b respecto a este avión
            double px = b.currentPosition.GetX() - currentPosition.GetX();
            double py = b.currentPosition.GetY() - currentPosition.GetY();
            double vx = b.GetVelocidadX() - GetVelocidadX();
            double vy = b.GetVelocidadY() - GetVelocidadY();

            // d²(t) = (px + vx·t)² + (py + vy·t)²
            // Derivada: d²'(t) = 2·(px + vx·t)·vx + 2·(py + vy·t)·vy
            // Igualamos a 0 (mínimo) y despejamos: t = -(px·vx + py·vy) / (vx² + vy²)
            double t = 0;
            double v2 = vx * vx + vy * vy;
            if (v2 > 0)
            {
                t = -(px * vx + py * vy) / v2;
            }

            // El instante tiene que estar entre ahora y la llegada del primero
            if (t < 0)
            {
                t = 0;
            }
            double limite = Math.Min(TiempoHastaLlegada(), b.TiempoHastaLlegada());
            if (t > limite)
            {
                t = limite;
            }
            return t;
        }

        // Distancia mínima a la que se acercarán los dos aviones
        public double DistanciaMinima(FlightPlan b)
        {
            double t = TiempoMinimo(b);
            return PosicionEnTiempo(t).Distancia(b.PosicionEnTiempo(t));
        }

        // Dice si, siguiendo como van, los dos aviones entrarán en conflicto
        public bool HabraConflicto(FlightPlan b, double distanciaSeguridad)
        {
            return DistanciaMinima(b) < distanciaSeguridad;
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
