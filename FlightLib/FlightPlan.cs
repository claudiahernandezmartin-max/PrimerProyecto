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
        public double DistanciaMinima(FlightPlan b, out double tiempoMinimo)
        {
            double llegadaA = this.TiempoHastaLlegada();
            double llegadaB = b.TiempoHastaLlegada();
            double[] inicioTramo = { 0, Math.Min(llegadaA, llegadaB) };
            double[] finTramo = { Math.Min(llegadaA, llegadaB), Math.Max(llegadaA, llegadaB) };

            // De momento, la mínima es la distancia de ahora
            double dMin = this.currentPosition.Distancia(b.currentPosition);
            tiempoMinimo = 0;

            int i = 0;
            while (i < 2)
            {
                double ta = inicioTramo[i];
                double tb = finTramo[i];

                if (tb > ta)
                {
                    // Dónde están los dos al empezar el tramo
                    Position pa = this.PosicionEnTiempo(ta);
                    Position pb = b.PosicionEnTiempo(ta);

                    // Velocidades durante el tramo (si ya ha llegado, se queda parado)
                    double vax = 0, vay = 0, vbx = 0, vby = 0;
                    if (ta < llegadaA)
                    {
                        vax = this.GetVelocidadX();
                        vay = this.GetVelocidadY();
                    }
                    if (ta < llegadaB)
                    {
                        vbx = b.GetVelocidadX();
                        vby = b.GetVelocidadY();
                    }

                    // Posición y velocidad relativas (B respecto a A)
                    double px = pb.GetX() - pa.GetX();
                    double py = pb.GetY() - pa.GetY();
                    double vx = vbx - vax;
                    double vy = vby - vay;

                    // Derivada de d² igual a 0
                    double s = 0;
                    double v2 = vx * vx + vy * vy;
                    if (v2 > 0)
                    {
                        s = -(px * vx + py * vy) / v2;
                    }

                    // El mínimo tiene que estar dentro del tramo
                    if (s < 0)
                    {
                        s = 0;
                    }
                    if (s > tb - ta)
                    {
                        s = tb - ta;
                    }

                    // Posiciones de los dos aviones en ese momento y su distancia
                    Position a2 = new Position(pa.GetX() + vax * s, pa.GetY() + vay * s);
                    Position b2 = new Position(pb.GetX() + vbx * s, pb.GetY() + vby * s);
                    double d = a2.Distancia(b2);

                    if (d < dMin)
                    {
                        dMin = d;
                        tiempoMinimo = ta + s;
                    }
                }
                i++;
            }
            return dMin;
        }

        // Dice si, siguiendo como van, los dos aviones entrarán en conflicto
        public bool HabraConflicto(FlightPlan b, double distanciaSeguridad)
        {
            double tiempoMinimo;
            double dMin = DistanciaMinima(b, out tiempoMinimo);
            return dMin < distanciaSeguridad;
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
