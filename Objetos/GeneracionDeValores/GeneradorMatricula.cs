using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIM_TP5_VERSION_CHONA.Objetos.GeneracionDeValores
{
    internal class GeneradorMatricula
    {
        private Random generador = new Random();

        private double aAtencion;
        private double bAtencion;
        private double mediaLlegada;
        public GeneradorMatricula(double aAtencion,double bAtencion, double mediaLlegada)
        {
            this.aAtencion = aAtencion;
            this.bAtencion = bAtencion;
            this.mediaLlegada = mediaLlegada;
        }
        public double calcularAtencionMatricula(dynamic[] vActual, int indice)
        {
            double rnd = generador.NextDouble();
            vActual[indice] = Math.Round(rnd,4);
            double valor = Math.Round(this.aAtencion + (rnd * (this.bAtencion - this.aAtencion)),4);
            return valor;
        }

        public double calcularLlegadaMatricula(dynamic[] vActual)
        {
            double rnd = generador.NextDouble();
            vActual[2] = Math.Round(rnd,4);
            double valor = Math.Round(-this.mediaLlegada*Math.Log(1-rnd),4);
              
            return valor;
        }
    }
}
