using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIM_TP5_VERSION_CHONA.Objetos.Clientes;
using SIM_TP5_VERSION_CHONA.Objetos.GeneracionDeValores;

namespace SIM_TP5_VERSION_CHONA.Objetos.Controladores
{
    internal class Controller_Clientes
    {
        private double[] valoresBoxMuller = new double[] {0,0};

        
        private GeneradorMatricula generadorM;
        private GeneradorRenovacion generadorR;


        public Controller_Clientes(double mediaLlegadaM,
                                   double aAtencionM,
                                   double bAtencionM,
                                   double mediaLlegadaR,
                                   double desvR,
                                   double mediaAtencionR)
        {
            this.generadorM = new GeneradorMatricula(aAtencionM,bAtencionM,mediaLlegadaM);
            this.generadorR = new GeneradorRenovacion(mediaLlegadaR,desvR,mediaAtencionR);
        }

        public void crearNuevaLlegada(double reloj, Controller_Eventos controllerEventos, dynamic[] vActual)
        {
            double randomMatricula = this.generadorM.calcularLlegadaMatricula(vActual);
            double randomRenovacion = this.generadorR.calcularLlegadaRenovacion(vActual);

            ClienteMatricula cm = new ClienteMatricula(randomMatricula, reloj);
            ClienteRenovacion cr = new ClienteRenovacion(randomRenovacion, reloj);
           
            controllerEventos.crearEventoLlegada(cr);
            controllerEventos.crearEventoLlegada(cm);
            vActual[3] = randomMatricula;
            vActual[4] = cm.getProxLlegada();
            vActual[6] = randomRenovacion;
            vActual[7] = cr.getProxLlegada();
        }

        public Cliente crearNuevaLlegada(double reloj, Controller_Eventos controllerEventos,Cliente clienteTipo, dynamic[] vActual)
        {
            Cliente cliente;
            if(clienteTipo.GetType() == typeof(ClienteRenovacion))
            {
                double randomRenovacion = Math.Round(this.generadorR.calcularLlegadaRenovacion(vActual),4);
                cliente = new ClienteRenovacion(randomRenovacion, reloj);
                vActual[6] = randomRenovacion;
                vActual[7] = cliente.getProxLlegada();
            }
            else
            {
                double randomMatricula = Math.Round(this.generadorM.calcularLlegadaMatricula(vActual),4);
                cliente = new ClienteMatricula(randomMatricula, reloj);
                vActual[3] = randomMatricula;
                vActual[4] = cliente.getProxLlegada();
            }
            controllerEventos.crearEventoLlegada(cliente);
            
            return cliente;
        }

        public void establecerFinAtencion(Cliente cliente, double reloj, dynamic[] vActual)
        {
            if(cliente.GetType() == typeof(ClienteRenovacion))
            {
                double randomRenovacion = verificarValoresBOXMULLER(vActual);
                cliente.setFinAtencion(randomRenovacion,reloj);
                vActual[12] = randomRenovacion;
                vActual[13] = cliente.getFinAtencion();

            }
            else
            {
                double randomMatricula = Math.Round(this.generadorM.calcularAtencionMatricula(vActual,8),4);
                cliente.setFinAtencion(randomMatricula,reloj);
                vActual[9] = randomMatricula;
                vActual[10] = cliente.getFinAtencion();
            }
        }

        public double verificarValoresBOXMULLER(dynamic[] vActual)
        {
            double finAtencion;
            if (valoresBoxMuller[0] == 0 && valoresBoxMuller[1] == 0)
            {
                valoresBoxMuller = this.generadorR.calcularAtencionRenovacion();
                finAtencion = valoresBoxMuller[0];
                valoresBoxMuller[0] = 0;
                vActual[11] = this.generadorR.getRandomAsociado(0);
            } 
            else
            {
                finAtencion = valoresBoxMuller[1];
                valoresBoxMuller[1] = 0;
                vActual[11] = this.generadorR.getRandomAsociado(1);
            }

            return finAtencion;
        }
    }
}
