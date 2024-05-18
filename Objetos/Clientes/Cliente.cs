using SIM_TP5_VERSION_CHONA.Objetos.Administrativos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SIM_TP5_VERSION_CHONA.Objetos.Clientes
{
    internal abstract class Cliente
    {
        double proxLlegada;
        double finAtencion;
        double tiempoAtencion;
        double inicioCola;
        int id;

        Administrativo administrativo;
        public Cliente(double tiempoLlegada, double reloj)
        {
            this.proxLlegada = Math.Round((reloj + tiempoLlegada),4);
        }

        public Cliente(double finAtencion)
        {
            this.finAtencion = finAtencion;
        }

        public double getProxLlegada()
        {
            return this.proxLlegada;
        }

        public void setFinAtencion(double finAtencion, double reloj)
        {
            
            this.tiempoAtencion = finAtencion;
            this.finAtencion = Math.Round((finAtencion + reloj),4);
            
        }

        public double getTiempoAtencion()
        {
            return this.tiempoAtencion;
        }
        public double getFinAtencion()
        {
            return this.finAtencion;
        }

        public void asignarAdministrativo(Administrativo adm)
        {
            this.administrativo = adm;
        }

        public Administrativo getAdministrativo()
        {
            return this.administrativo;
        }
        public void setInicioCola(double tiempo)
        {
            this.inicioCola = tiempo;
        }
        public double getInicioCola()
        {
            return this.inicioCola;
        }
        public void setId(int id)
        {
            this.id = id;
        }
        public int getId()
        {
            return this.id;
        }

        public void asignarNuevoFinAtencionModificado(double finAtencion)
        {
            this.finAtencion = finAtencion;
        }
    }
}
