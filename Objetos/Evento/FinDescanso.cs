using SIM_TP5_VERSION_CHONA.Objetos.Administrativos;
using SIM_TP5_VERSION_CHONA.Objetos.Clientes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIM_TP5_VERSION_CHONA.Objetos.Evento
{
    internal class FinDescanso : I_Evento
    {
        Administrativo adm;

        public FinDescanso(Administrativo adm)
        {
            this.adm = adm;
        }
        public Administrativo getAdministrativo()
        {
            return this.adm;
        }

        public Cliente getCliente()
        {
            throw new NotImplementedException();
        }

        public double getTiempoOcurrencia()
        {
            return this.adm.getFinDescanso();
        }
    }
}
