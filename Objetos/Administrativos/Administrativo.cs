using SIM_TP5_VERSION_CHONA.Objetos.Clientes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SIM_TP5_VERSION_CHONA.Objetos.Administrativos
{
    internal abstract class Administrativo
    {
        private Cliente cliente;

        private static int Libre = 0;
        private static int Ocupado = 1;
        private static int Descansando = 2;
        private int Estado;
        private bool descanso;
        private double finDescanso;
        private double horarioDescanso;
        private double tiempoLibre = 0;
        private double iniciaTiempoLibre;


        //tp6


        public Administrativo()
        {
            this.descanso = false;
            this.liberar(0);
        }
        public int getEstado()
        {
            return this.Estado;
        }

        public bool estaOcupado()
        {
            if (this.Estado == Ocupado || this.Estado == Descansando)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void asignarCliente(Cliente cliente,double reloj)
        {
            this.cliente = cliente;
            this.Estado = Ocupado;        
            this.sumarTiempoLibre(reloj);
        }

        public void liberar(double reloj)
        {
            this.Estado = Libre;
            this.cliente = null;
            this.iniciaTiempoLibre = reloj;
        }

        public Cliente getCliente()
        {
            return this.cliente;
        }
        public void descansar(double lapsoEntreDescansos)
        {
            this.Estado = Descansando;
            this.descanso = false;
            this.finDescanso = lapsoEntreDescansos + this.horarioDescanso;
        }
        public bool correspondeDescanso()
        {
            return this.descanso;
        }

        public void setHorarioDescanso(double horarioDescanso)
        {
            this.horarioDescanso = horarioDescanso;
        }

        public double getHorarioDescanso()
        {
            return this.horarioDescanso;
        }

        public void habilitarDescanso()
        {
            this.descanso = true;
        }

        public double getFinDescanso()
        {
            return this.finDescanso;
        }

        public double getTiempoLibre()
        {
            return this.tiempoLibre;
        }

        public void sumarTiempoLibre(double reloj)
        {
            this.tiempoLibre += (reloj - iniciaTiempoLibre);
        }


        public void cambiarFinAtencion(double reloj, double finBloqueo)
        {


            this.cliente.asignarNuevoFinAtencionModificado(this.cliente.getFinAtencion() - reloj + finBloqueo) ;
        }

        
        public void cambiarFinDescanso(double reloj,  double finBloqueo)
        {


            double finDescansoEstimado = this.finDescanso;
            this.finDescanso = finDescansoEstimado - reloj + finBloqueo;
        }
    }

}
