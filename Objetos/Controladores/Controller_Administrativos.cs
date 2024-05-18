using SIM_TP5_VERSION_CHONA.Objetos.Administrativos;
using SIM_TP5_VERSION_CHONA.Objetos.Clientes;
using SIM_TP5_VERSION_CHONA.Objetos.Evento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SIM_TP5_VERSION_CHONA.Objetos.Controladores
{
    internal class Controller_Administrativos
    {
        private Queue<Cliente> colaMatricula = new Queue<Cliente>();
        private Queue<Cliente> colaRenovacion = new Queue<Cliente>();

        

        private List<Administrativo> administrativosMixto = new List<Administrativo>();
        private List<Administrativo> administrativosMatricula = new List<Administrativo>();
        private List<Administrativo> administrativosRenovacion = new List<Administrativo>();

        private double lapsoEntreDescansos;

        private int maxCantAdministrativos;

        private double tiempoEnEsperaTotalMatricula = 0;
        private double tiempoEnEsperaTotalRenovacion = 0;

        private int qLlegadasMatriculas = 0;
        private int qLlegadasRenovaciones = 0;

        //TP6
        private Queue<Cliente> colaBloqueados = new Queue<Cliente>();

        public Controller_Administrativos(int qMixto, int qMatricula, int qRenovacion, dynamic vActual)
        {
            for (int i = 0; i < qMixto; i++)
            {
                administrativosMixto.Add(new AdministrativoMixto());
                vActual[22 + i * 12] = "Libre";

            }

            maxCantAdministrativos = administrativosMixto.Count;

            for (int i = 0; i < qMatricula; i++)
            {
                administrativosMatricula.Add(new AdministrativoMatricula());
                vActual[14 + i * 12] = "Libre";
            }

            if(administrativosMatricula.Count > maxCantAdministrativos)
            {
                maxCantAdministrativos = administrativosMatricula.Count;
            }
            for (int i = 0; i < qRenovacion; i++)
            {
                administrativosRenovacion.Add(new AdministrativoRenovacion());
                vActual[18 + i * 12] = "Libre";
            }
            if(administrativosRenovacion.Count > maxCantAdministrativos)
            {
                maxCantAdministrativos=administrativosRenovacion.Count;
            }


        }

        public void asignarCliente(Cliente cliente, dynamic[] vActual,double reloj, Controller_Clientes controllerClientes, Controller_Eventos controllerEventos)
        {
            int indice = 0;
            if(cliente.GetType() == typeof(ClienteMatricula))
            {
                indice = 14;
                verificarAdm(colaMatricula, administrativosMatricula,cliente, indice, vActual,reloj,controllerClientes,controllerEventos);

            }
            else
            {
                indice = 18;
                verificarAdm(colaRenovacion, administrativosRenovacion, cliente, indice, vActual,reloj,controllerClientes,controllerEventos);
            }
        }

        private void verificarAdm(Queue<Cliente> cola, List<Administrativo> administrativos, Cliente cliente, int indice, dynamic[] vActual,double reloj,Controller_Clientes controllerClientes, Controller_Eventos controllerEventos)
        {
            for (int i = 0; i < administrativos.Count; i++)
            {
                if (!administrativos[i].estaOcupado())
                {
                    indice += i * 12;
                    vActual[indice] = "Ocupado";
                    cliente.asignarAdministrativo(administrativos[i]);
                    vActual[indice + 1] = cliente.getId();   
                
                    administrativos[i].asignarCliente(cliente,reloj);
                    controllerClientes.establecerFinAtencion(cliente, reloj, vActual);
                    controllerEventos.crearEventoFinAtencion(cliente);
                    vActual[indice + 2] = cliente.getFinAtencion();
                    return;
                }
            }

            cola.Enqueue(cliente);
            cliente.setInicioCola(vActual[1]);
        }

        public void verificarOcioAdmMixto(Controller_Eventos controllerEventos,
                                          Controller_Clientes controllerClientes,
                                          double reloj, 
                                          dynamic[] vActual,
                                          int indice)
        {
            for(int i = 0; i < administrativosMixto.Count; i++)
            {
                AdministrativoMixto admMxt = (AdministrativoMixto) administrativosMixto[i];
                if (!admMxt.estaOcupado())
                {
                    
                    int c = 0;
                    Cliente cm = null;
                    Cliente cr = null;
                    Cliente cliente_a_atender = null;

                    //Se verifica si la cola de matriculas tiene a alguien esperando
                    //y se obtiene el primer cliente
                    if (colaMatricula.Count > 0)
                    {
                        cm = colaMatricula.Peek();
                        c++;
                    }

                    //Mismo caso que cola de matriculas pero con cola de renovacion
                    if (colaRenovacion.Count > 0)
                    {
                        cr = colaRenovacion.Peek();
                        c++;
                    }


                    //Se comprueba si las dos colas tienen clientes esperando...
                    if (c == 2)
                    {
                        //Se comprueba cual de los dos clientes llevo mas tiempo esperando
                        //Se lo saca de la cola y se asigna al servidor mixto
                        if (cm.getProxLlegada() < cr.getProxLlegada())
                        {
                            cliente_a_atender = cm;
                        }
                        else
                        {
                            cliente_a_atender = cr;
                        }
                        
                    }
                    else
                    {
                        //Se comprueba si alguno de las dos colas posee un cliente esperando
                        //o si se encuentran las dos vacias
                        if (c == 1)
                        {
                            if (cm == null)
                            {
                                cliente_a_atender = cr;

                            }
                            else
                            {
                                cliente_a_atender = cm;
                            }

                        }

                    }

                    if(cliente_a_atender != null)
                    {
                        admMxt.asignarCliente(cliente_a_atender,reloj);
                        cliente_a_atender.asignarAdministrativo(admMxt);
                        vActual[indice + i*12] = "Ocupado";
                        vActual[indice + 1 + i * 12] = cliente_a_atender.GetType().Name + " " + cliente_a_atender.getId(); 
                        controllerClientes.establecerFinAtencion(cliente_a_atender, reloj, vActual);
                        vActual[indice + i * 12 + 2] = cliente_a_atender.getFinAtencion();
                        controllerEventos.crearEventoFinAtencion(cliente_a_atender);
                        if(cliente_a_atender.GetType() == typeof(ClienteRenovacion))
                        {
                            colaRenovacion.Dequeue();
                            this.tiempoEnEsperaTotalRenovacion += Math.Round((Math.Round(reloj,4) - Math.Round(cliente_a_atender.getInicioCola(),4)),4);
                            vActual[37] = Math.Round((this.tiempoEnEsperaTotalRenovacion / qLlegadasRenovaciones),4);
                        }
                        else
                        {
                            colaMatricula.Dequeue();
                            this.tiempoEnEsperaTotalMatricula += Math.Round((Math.Round(reloj,4) - Math.Round(cliente_a_atender.getInicioCola(),4)),4);
                            vActual[36] = Math.Round((this.tiempoEnEsperaTotalMatricula / qLlegadasMatriculas),4);
                        }
                        
                        
                    }
                    else
                    {
                        vActual[indice + i * 12] = "Libre";
                        vActual[i * 12 + indice + 1] = "/";
                        vActual[i * 12 + indice + 2] = 0;
                    }
                    
                }
            }
        }

        public void verificarNuevoCliente(Administrativo adm,
                                          Controller_Clientes controllerClientes,
                                          Controller_Eventos controllerEventos,
                                          double reloj,
                                          dynamic[] vActual)
        {
            int indice = 0;

            adm.liberar(reloj);

            if (adm.correspondeDescanso())
            {
                adm.setHorarioDescanso(reloj);
                verificarDescanso(adm,vActual,controllerEventos);
                return;
            }

            if(adm.GetType() == typeof(AdministrativoRenovacion))
            {
                indice = 18;
                verificarCola(colaRenovacion, adm, reloj, controllerClientes, controllerEventos, vActual, administrativosRenovacion, indice);
            }
            else
            {

                if(adm.GetType() == typeof(AdministrativoMatricula))
                {
                    indice = 14;
                    verificarCola(colaMatricula, adm, reloj, controllerClientes, controllerEventos, vActual, administrativosMatricula, indice);
                }
                else 
                {
                    indice = 22;
                    verificarOcioAdmMixto(controllerEventos, controllerClientes, reloj, vActual, indice);
                    
                }
            }
        }

        private bool verificarCola(Queue<Cliente> cola,
                                   Administrativo adm,
                                   double reloj,
                                   Controller_Clientes controllerClientes,
                                   Controller_Eventos controllerEventos,
                                   dynamic[] vActual,
                                   List<Administrativo> administrativos,
                                   int indice)
        {
            
            if(cola.Count > 0)
            {

                Cliente cliente = cola.Dequeue();

                if(cliente.GetType() == typeof(ClienteMatricula))
                {
                    this.tiempoEnEsperaTotalMatricula += reloj - cliente.getInicioCola();
                    vActual[36] = this.tiempoEnEsperaTotalMatricula / qLlegadasMatriculas;
                }
                else
                {
                    this.tiempoEnEsperaTotalRenovacion += reloj - cliente.getInicioCola();
                    vActual[37] = this.tiempoEnEsperaTotalRenovacion / qLlegadasRenovaciones;
                }

                adm.asignarCliente(cliente,reloj);

                int indiceAux = 0;
                for(int i = 0; i < administrativos.Count; i++)
                {
                    if (administrativos[i].Equals(adm))
                    {
                        vActual[i * 12 + indice] = "Ocupado";
                        vActual[i * 12 + indice + 1] = cliente.getId();

                        indiceAux = i;
                        break;
                    }
                }
                cliente.asignarAdministrativo(adm);

                controllerClientes.establecerFinAtencion(cliente, reloj, vActual);
                controllerEventos.crearEventoFinAtencion(cliente);
                vActual[indiceAux * 12 + indice + 2] = cliente.getFinAtencion();
                return true;
            }
            for (int i = 0; i < administrativos.Count; i++)
            {
                if (administrativos[i].Equals(adm))
                {
                    vActual[i * 12 + indice] = "Libre";
                    vActual[i * 12 + indice + 1] = "/";
                    vActual[i * 12 + indice + 2] = 0;
                    
                    break;
                }
            }
            return false;
        }

        public void verificarDescanso(Administrativo adm,dynamic[] vActual,Controller_Eventos controllerEventos)
        {
            int indice = 0;
            if (adm.GetType() == typeof(AdministrativoMatricula))
            {
                indice = 14;
                asignarDescanso(adm, vActual, controllerEventos, administrativosMatricula, indice);
            }

            else
            {
                if (adm.GetType() == typeof(AdministrativoRenovacion))
                {
                    indice = 18;
                    asignarDescanso(adm, vActual, controllerEventos, administrativosRenovacion, indice);
                }
                else
                {
                    indice = 22;
                    asignarDescanso(adm, vActual, controllerEventos,administrativosMixto, indice);
                }
            }
        }

        public void asignarDescanso(Administrativo adm,
                                    dynamic[] vActual,
                                    Controller_Eventos controllerEventos,
                                    List<Administrativo> administrativos,
                                    int indice)
        {
            for(int i = 0; i < administrativos.Count; i++)
            {
                if (adm.Equals(administrativos[i]))
                {
                    if (!adm.estaOcupado())
                    {
                        adm.descansar(lapsoEntreDescansos);
                        controllerEventos.crearEventoFinDescanso(adm);
                        vActual[indice + i * 12] = "Descansando";
                        vActual[indice + i * 12 + 1] = "/";
                        vActual[indice + i * 12 + 2] = 0;
                        vActual[indice + i * 12 + 3] = adm.getFinDescanso();
                    }
                    break;
                }
            }
        }
        public int getCantidadMatricula()
        {
            return colaMatricula.Count;
        }
        public int getCantidadRenovacion()
        {
            return colaRenovacion.Count;
        }

        public void generarDescansos(double horaDescanso, double lapsoEntreDescansos, Controller_Eventos controllerEventos)
        {
            this.lapsoEntreDescansos = lapsoEntreDescansos;
            double descanso = horaDescanso;
            Administrativo adm;

            for(int i = 0; i < maxCantAdministrativos; i++)
            {
               
                if(administrativosMatricula.Count > i) 
                {
                    adm = administrativosMatricula[i];
                    adm.setHorarioDescanso(descanso);

                    controllerEventos.crearEventoDescanso(adm);
                    descanso += lapsoEntreDescansos;

                }
                if(administrativosRenovacion.Count > i)
                {
                    adm = administrativosRenovacion[i];
                    adm.setHorarioDescanso(descanso);

                    controllerEventos.crearEventoDescanso(adm);
                    descanso += lapsoEntreDescansos;
                }
                if(administrativosMixto.Count > i)
                {
                    adm = administrativosMixto[i];
                    adm.setHorarioDescanso(descanso);
                    controllerEventos.crearEventoDescanso(adm);
                    descanso += lapsoEntreDescansos;
                }
            }
        }

        public void terminarDescanso(Administrativo adm,
                                     Controller_Clientes controllerClientes,
                                     Controller_Eventos controllerEventos,
                                     double reloj,
                                     dynamic[] vActual)
        {
            verificarNuevoCliente(adm,controllerClientes,controllerEventos,reloj,vActual);
        }

        public void incrementarCantLlegadas(Cliente cliente)
        {
            if(cliente.GetType() == typeof(ClienteMatricula))
            {
                qLlegadasMatriculas++;
                cliente.setId(qLlegadasMatriculas);
            }
            else
            {
                qLlegadasRenovaciones++;
                cliente.setId(qLlegadasRenovaciones);
            }
        }

        public double[] calcularEstadisticas(double reloj)
        {

            double sumaLibre = 0;
            double porcLibre = 0;

            for(int i = 0; i < administrativosMatricula.Count; i++)
            {
                if (!administrativosMatricula[i].estaOcupado())
                {
                    administrativosMatricula[i].sumarTiempoLibre(reloj);
                }
                sumaLibre += administrativosMatricula[i].getTiempoLibre();
                //MessageBox.Show("tiempo libre de adm M" + i + ": " + administrativosMatricula[i].getTiempoLibre());
                
            }
            for(int i = 0; i < administrativosRenovacion.Count; i++)
            {
                if (!administrativosRenovacion[i].estaOcupado())
                {
                    administrativosRenovacion[i].sumarTiempoLibre(reloj);
                }
                
                sumaLibre += administrativosRenovacion[i].getTiempoLibre();
                //MessageBox.Show("tiempo libre de adm R" + i + ": " + administrativosRenovacion[i].getTiempoLibre());
            }
            for(int i = 0; i < administrativosMixto.Count; i++)
            {
                if (!administrativosMixto[i].estaOcupado())
                {
                    administrativosMixto[i].sumarTiempoLibre(reloj);
                }
                
                sumaLibre += administrativosMixto[i].getTiempoLibre();
                //MessageBox.Show("tiempo libre de adm Mixto" + i + ": " + administrativosMixto[i].getTiempoLibre());
            }
            int qAdmins = (administrativosMatricula.Count + administrativosMixto.Count + administrativosRenovacion.Count);
            double sumaProm = sumaLibre / qAdmins;
            
            //MessageBox.Show(sumaProm.ToString());
            porcLibre = (sumaProm * 100) / reloj;
            //MessageBox.Show(porcLibre.ToString());
            //sumaLibre /= ( * reloj);
            //porcLibre = sumaLibre * 100;
            //MessageBox.Show(sumaLibre.ToString());
            return new double[2] { porcLibre , (100 - porcLibre)};
        }

        public void bloquearAdmin(dynamic[] vActual, Controller_Eventos controllerEventos, double reloj, double finBloqueo,Controller_Clientes controllerClientes)
        {
            Administrativo adm = administrativosMatricula[0];

            vActual[14] = "Bloqueado";
            

            if (adm.getEstado() == 1)
            {
                controllerEventos.removeEvento(adm.getCliente().getFinAtencion());

                adm.cambiarFinAtencion(reloj, finBloqueo);

                controllerEventos.crearEventoFinAtencion(adm.getCliente());

                vActual[16] = adm.getCliente().getFinAtencion();
                
            }
            else
            {
                controllerEventos.removeEvento(adm.getFinDescanso());
                adm.cambiarFinDescanso(reloj,finBloqueo);

                controllerEventos.crearEventoFinDescanso(adm);

                vActual[17] = adm.getFinDescanso();
            }
            
        }

        public void agregarBloqueado(Cliente cliente)
        {
            this.colaBloqueados.Enqueue(cliente);           
        }

        public void reasignarBloqueados(dynamic[] vActual,double reloj,Controller_Clientes controllerClientes, Controller_Eventos controllerEventos)
        {
            Cliente cliente;
            int indice;
            int qLlegadasBloqueadas = colaBloqueados.Count;
            for (int i = 0; i < qLlegadasBloqueadas ;i++)
            {
                //MessageBox.Show("se recorre por la i = " + i.ToString() + " de: " + colaBloqueados.Count.ToString());
                cliente = colaBloqueados.Dequeue();
                
                if (cliente.GetType() == typeof(ClienteMatricula))
                {
                    indice = 14;
                    verificarAdm(colaMatricula,administrativosMatricula,cliente,indice,vActual,reloj,controllerClientes,controllerEventos);
                    
                }
                else
                {
                    indice = 18;
                    verificarAdm(colaRenovacion, administrativosRenovacion, cliente, indice, vActual, reloj, controllerClientes, controllerEventos);
                    
                }
            }
            indice = 22;
            verificarOcioAdmMixto(controllerEventos,controllerClientes,reloj,vActual,indice);
 
        }

        public int getCountBloqueados()
        {
            return this.colaBloqueados.Count;
        }

        public void desbloquearAdm(Controller_Clientes controllerClientes, Controller_Eventos controllerEventos, double reloj, dynamic[] vActual)
        {
            Administrativo adm = administrativosMatricula[0];
            if (adm.getEstado() == 0)
            {
                if(colaMatricula.Count > 0)
                {
                    verificarNuevoCliente(adm,controllerClientes,controllerEventos,reloj,vActual);
                }
            }
            else
            {
                if (adm.getEstado() == 1)
                {
                    vActual[14] = "Ocupado";
                }
                else
                {
                    vActual[14] = "Descansando";
                }
            }
            
        }

    }
    
}
