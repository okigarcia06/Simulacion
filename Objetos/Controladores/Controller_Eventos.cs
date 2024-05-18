using SIM_TP5_VERSION_CHONA.Forms;
using SIM_TP5_VERSION_CHONA.Objetos.Administrativos;
using SIM_TP5_VERSION_CHONA.Objetos.Clientes;
using SIM_TP5_VERSION_CHONA.Objetos.Evento;
using SIM_TP5_VERSION_CHONA.Objetos.GeneracionDeValores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SIM_TP5_VERSION_CHONA.Objetos.Controladores
{
    internal class Controller_Eventos
    {
        SortedList<double,List<I_Evento>> eventosOrdenados = new SortedList<double, List<I_Evento>>();

        public void crearEventoLlegada(Cliente cliente)
        {
            EventoLlegada eventoLlegada = new EventoLlegada(cliente);
            verificarSiHayOtroEvento(eventoLlegada);
        }

        public void crearEventoFinAtencion(Cliente cliente)
        {

            FinAtencion eventoFinAtencion = new FinAtencion(cliente);
            verificarSiHayOtroEvento(eventoFinAtencion);
        }

        private void verificarSiHayOtroEvento(I_Evento ev_a_guardar)
        {
            if (this.eventosOrdenados.ContainsKey(ev_a_guardar.getTiempoOcurrencia()))
            {
                eventosOrdenados[ev_a_guardar.getTiempoOcurrencia()].Add(ev_a_guardar);
            }
            else
            {
                List<I_Evento> l = new List<I_Evento>();
                l.Add(ev_a_guardar);
                eventosOrdenados.Add(ev_a_guardar.getTiempoOcurrencia(), l);
            }
        }



        public bool verificarEventoLlegada(I_Evento evento)
        {
            if(evento.GetType() == typeof(EventoLlegada))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<I_Evento> getProxEvento()
        {
            return this.eventosOrdenados.First().Value;
        }

        public bool removeEvento(double reloj)
        {
            return this.eventosOrdenados.Remove(reloj);
        }

        public void crearEventoDescanso(Administrativo adm)
        {
            ComienzoDescanso descanso = new ComienzoDescanso(adm);
            verificarSiHayOtroEvento(descanso);
        }
        public bool verificarEventoFinAtencion(I_Evento evento)
        {
            if (evento.GetType() == typeof(FinAtencion))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void crearEventoFinDescanso(Administrativo adm)
        {
            FinDescanso finDescanso = new FinDescanso(adm);
            verificarSiHayOtroEvento(finDescanso);
        }

        public bool verificarEventoDescanso(I_Evento evento)
        {
            if (evento.GetType() == typeof(ComienzoDescanso))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool verificarEventoFinDescanso(I_Evento evento)
        {
            if(evento.GetType() == typeof(FinDescanso))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void crearEventoAtaque(double A,double reloj,Form_RungeKutta form_Runge,dynamic[] vActual)
        {
            Random rnd = new Random();
            double valorRnd = rnd.NextDouble();
            vActual[40] = Math.Round(valorRnd, 4);
            string tipoE;
            if (valorRnd < 0.70)
            {
                tipoE = "bloqueoClientes";
            }
            else
            {
                tipoE = "ataqueServ";
            }
            vActual[41] = tipoE;

            double valorRunge = RungeKutta.generarLlegadaAtaque(A, form_Runge, vActual);
            double proxAtaque = reloj + valorRunge;
            Ataque atq = new Ataque(tipoE,proxAtaque);
            verificarSiHayOtroEvento(atq);

            vActual[42] = valorRunge;
            vActual[43] = proxAtaque;
        }

        public void crearEventoFinAtaque(string tipoAtaque, double fin, dynamic[] vActual)
        {

            FinAtaque finAtaque = new FinAtaque(tipoAtaque,fin);
            if (tipoAtaque == "bloqueoClientes")
            {
                vActual[44] = fin;
            }
            else
            {
                vActual[46] = fin;
            }
            verificarSiHayOtroEvento(finAtaque);
        }

        public bool verificarEventoAtaque(I_Evento evento)
        {
            if (evento.GetType() == typeof(Ataque))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
