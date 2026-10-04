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

        // Constructures
        public FlightPlan(string id, double cpx, double cpy, double fpx, double fpy, double velocidad)
        {
            this.id = id;
            this.currentPosition = new Position(cpx, cpy);
            this.finalPosition = new Position(fpx, fpy);
            this.initialPosition = new Position(cpx, cpy);
            this.velocidad = velocidad;
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

        //Fer métode que detecti quan hi ha dos vols que estan més aprop de la distància de seguretat 
        public bool Conflicto(FlightPlan b, double distanciaSeguridad)
        {  
            bool conflicto = false;
            if (this.currentPosition.Distancia(b.currentPosition) < distanciaSeguridad)
                conflicto = true;
             
            return conflicto;
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
