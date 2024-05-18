using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIM_TP5_VERSION_CHONA.Objetos.GeneracionDeValores
{
    internal class Entidad
    {
        double probabilidadAc;
        int valor;

        public Entidad(double probabilidadAc, int valor)
        {
            this.probabilidadAc = probabilidadAc;
            this.valor = valor;
        }

        public double getProbabilidad()
        {
            return probabilidadAc;
        }
        public double getValor()
        {
            return valor;
        }
    }
}
