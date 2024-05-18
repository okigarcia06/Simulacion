using SIM_TP5_VERSION_CHONA.Objetos.Administrativos;
using SIM_TP5_VERSION_CHONA.Objetos.Clientes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIM_TP5_VERSION_CHONA.Objetos.Evento
{
    internal class FinAtaque : I_Evento
    {
        private object obj;
        private string tipoAtaque;
        private double finAtaque;
        public FinAtaque(string tipoAtaque, double finAtaque)
        {
            this.tipoAtaque = tipoAtaque;
            this.finAtaque = finAtaque;
        }

        public Administrativo getAdministrativo()
        {
            throw new NotImplementedException();
        }

        public Cliente getCliente()
        {
            throw new NotImplementedException();
        }

        public double getTiempoOcurrencia()
        {
            return this.finAtaque;
        }

        public string getTipoAtaque()
        {
            return this.tipoAtaque;
        }
    }
}
