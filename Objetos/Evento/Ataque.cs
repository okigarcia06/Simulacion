using SIM_TP5_VERSION_CHONA.Objetos.Administrativos;
using SIM_TP5_VERSION_CHONA.Objetos.Clientes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIM_TP5_VERSION_CHONA.Objetos.Evento
{
    internal class Ataque : I_Evento
    {
        private string tipoAtaque;
        private double proxAtaque;
        public Ataque(string tipoAtaque,double proxAtaque)
        {
            this.tipoAtaque = tipoAtaque;
            this.proxAtaque = proxAtaque;
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
            return this.proxAtaque;
        }

        public string getTipoAtaque()
        {
            return this.tipoAtaque;
        }

    }
}
