using SIM_TP5_VERSION_CHONA.Objetos.Administrativos;
using SIM_TP5_VERSION_CHONA.Objetos.Clientes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIM_TP5_VERSION_CHONA.Objetos.Evento
{
    internal class EventoLlegada : I_Evento
    {
        Cliente cliente;

        public EventoLlegada(Cliente cliente)
        {
            this.cliente = cliente;
        }

        public Administrativo getAdministrativo()
        {
            throw new NotImplementedException();
        }

        public Cliente getCliente()
        {
            return this.cliente;
        }

        public double getTiempoOcurrencia()
        {
            return this.cliente.getProxLlegada();
        }
    }
}
