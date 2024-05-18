using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIM_TP5_VERSION_CHONA.Objetos.GeneracionDeValores
{
    internal class GeneradorRenovacion
    {
        private Random generador = new Random();
        private double mediaLlegada;
        private double desvAtencion;
        private double mediaAtencion;

        private double[] randoms;
        public GeneradorRenovacion(double mediaLlegada, double desvAtencion, double mediaAtencion)
        {
            this.mediaLlegada = mediaLlegada;
            this.desvAtencion = desvAtencion;
            this.mediaAtencion = mediaAtencion;
        }
        public double calcularLlegadaRenovacion(dynamic [] vActual)
        {
            double rnd = generador.NextDouble();
            vActual[5] = Math.Round(rnd,4);
            double valor = Math.Round(-this.mediaLlegada*Math.Log(1-rnd),4);

            return valor;
        }

        public double[] calcularAtencionRenovacion()
        {
            double[] valoresBoxMuller;
            double rnd1 = Math.Round(generador.NextDouble(),4);
            double rnd2 = Math.Round(generador.NextDouble(),4);

            this.randoms = new double[2] {rnd1,rnd2};
            double valorNormal1 = Math.Round((Math.Sqrt(-2 * Math.Log(rnd1)) * Math.Cos(2 * Math.PI * rnd2)) * this.desvAtencion + this.mediaAtencion,4);
            double valorNormal2 = Math.Round((Math.Sqrt(-2 * Math.Log(rnd1)) * Math.Sin(2 * Math.PI * rnd2)) * this.desvAtencion + this.mediaAtencion,4);

            return valoresBoxMuller = new double[2] { valorNormal1, valorNormal2 };
        }

        public double getRandomAsociado(int indice)
        {
            return this.randoms[indice];
        }
    }

}
