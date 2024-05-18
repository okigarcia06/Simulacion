using SIM_TP5_VERSION_CHONA.Objetos.Administrativos;
using SIM_TP5_VERSION_CHONA.Objetos.Clientes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIM_TP5_VERSION_CHONA.Objetos.Evento
{
    internal class FinAtencion : I_Evento
    {
        Cliente cliente;
        Administrativo administrativo;
        public FinAtencion(Cliente cliente)
        {
            this.cliente = cliente;
            this.administrativo = cliente.getAdministrativo();
        }

        public double getTiempoOcurrencia()
        {
            return this.cliente.getFinAtencion();
        }

        public Administrativo getAdministrativo()
        {
            return this.administrativo;
        }

        public Cliente getCliente()
        {
            return this.cliente;
        }
    }
}
