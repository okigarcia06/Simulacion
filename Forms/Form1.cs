
using SIM_TP5_VERSION_CHONA.Forms;
using SIM_TP5_VERSION_CHONA.Objetos.Administrativos;
using SIM_TP5_VERSION_CHONA.Objetos.Clientes;
using SIM_TP5_VERSION_CHONA.Objetos.Controladores;
using SIM_TP5_VERSION_CHONA.Objetos.Evento;
using SIM_TP5_VERSION_CHONA.Objetos.GeneracionDeValores;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SIM_TP5_VERSION_CHONA
{
    public partial class Form1 : Form
    {
        double reloj;
        double[] valoresBoxMuller = new double[2] { 0, 0 };
        dynamic[] vAnterior = new dynamic[47] {0,0,0,0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,0,0,0,0,0, 0, 0, 0, 0, 0, 0 };
        double[] estadisticasOcupacion;
        double qEventos;


        Form_RungeKutta form_RungeKutta;

        //TP6
        double A;
        bool bloqueoSistema;
        public Form1()
        {
            InitializeComponent();
        }

        private void btn_generar_Click_1(object sender, EventArgs e)
        {
            if (!verificaciones())
            {
                return;
            }

            //se crea el form para RungeKutta
            this.form_RungeKutta = new Form_RungeKutta();

            this.qEventos = 0;

            dataGridView2.Rows.Clear();
            this.estadisticasOcupacion = new double[2] {0,0};

            //tp6
            this.bloqueoSistema = false;

            for (int i = 0; i < Math.Truncate(double.Parse(diasSimulacion.Text)); i++ )
            {
                reloj = 0;

                bloqueoSistema = false;

                dynamic[] vActual = new dynamic[47] { "Inicio de Simulación, Dia: " + (i + 1), 0,0,0,0,0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,0 };

                Controller_Eventos controllerEventos = new Controller_Eventos();
                Controller_Clientes controllerClientes = new Controller_Clientes(double.Parse(mediaLlegadaM.Text),
                                                                                 double.Parse(aAtencionMatricula.Text),
                                                                                 double.Parse(bAtencionMatricula.Text),
                                                                                 double.Parse(mediaLlegadaR.Text),
                                                                                 double.Parse(desvAtencionR.Text),
                                                                                 double.Parse(mediaAtencionR.Text));

                controllerClientes.crearNuevaLlegada(reloj, controllerEventos, vActual);


                Controller_Administrativos controllerAdministrativos = new Controller_Administrativos(1, 2, 2, vActual);
                controllerAdministrativos.generarDescansos(180, 30, controllerEventos);


                while (reloj <= 480)
                {
                    if(qEventos >= int.Parse(qEventosDesde.Text) && qEventos <= int.Parse(qEventosDesde.Text) + 400)
                    {
                        dataGridView2.Rows.Add(qEventos,vActual[0], vActual[1], vActual[2], vActual[3], vActual[4], vActual[5], vActual[6], vActual[7], vActual[8], vActual[9], vActual[10], vActual[11], vActual[12], vActual[13], vActual[14], vActual[15], vActual[16], vActual[17], vActual[18], vActual[19], vActual[20], vActual[21], vActual[22], vActual[23], vActual[24], vActual[25], vActual[26], vActual[27], vActual[28], vActual[29], vActual[30], vActual[31], vActual[32], vActual[33], vActual[34], vActual[35], vActual[36], vActual[37], vActual[38], vActual[39], vActual[40],vActual[41], vActual[42], vActual[43], vActual[44], vActual[45], vActual[46]);
                    }
                    
                    simular(controllerEventos, controllerClientes, controllerAdministrativos, vActual);
                }

                double[] estadisticasTemp = controllerAdministrativos.calcularEstadisticas(reloj);
                this.estadisticasOcupacion[0] += estadisticasTemp[0];
                this.estadisticasOcupacion[1] += estadisticasTemp[1];
                
            }

            this.porcOcioAdmins.Text = (estadisticasOcupacion[0] / Math.Truncate(double.Parse(diasSimulacion.Text))).ToString();
            this.porcOcupAdmins.Text = (estadisticasOcupacion[1] / Math.Truncate(double.Parse(diasSimulacion.Text))).ToString();
        }

        private void simular(Controller_Eventos controllerEventos,Controller_Clientes controllerClientes, Controller_Administrativos controllerAdministrativos, dynamic[] vActual)
        {

            vActual[2] = 0; vActual[3] = 0; vActual[4] = 0; vActual[5] = 0; vActual[6] = 0; vActual[7] = 0; vActual[8] = 0; vActual[9] = 0; vActual[10] = 0; vActual[11] = 0; vActual[12] = 0; vActual[13] = 0;
            List<I_Evento> eventoLista = controllerEventos.getProxEvento();

            reloj = eventoLista[0].getTiempoOcurrencia();
            vActual[1] = reloj;
            vActual[0] = "";
            for (int i = 0; i < eventoLista.Count; i++)
            {
                qEventos++;
                

                if (controllerEventos.verificarEventoLlegada(eventoLista[i]))
                {
                    Cliente cliente = eventoLista[i].getCliente();
                    
                    vActual[38] += 1;

                    if (vActual[38] == 80)
                    {
                        vActual[0] += "Comienzo de ataques / "; 
                        this.A = reloj;
                        controllerEventos.crearEventoAtaque(A,reloj, form_RungeKutta,vActual);
                    }

                    controllerAdministrativos.incrementarCantLlegadas(cliente);
                    vActual[0] += "evento llegada " + cliente.GetType().Name + " " + cliente.getId() + "/";

                    if (bloqueoSistema)
                    {
                        controllerAdministrativos.agregarBloqueado(cliente);
                        controllerClientes.crearNuevaLlegada(reloj,controllerEventos,cliente,vActual);
                        vActual[45] = controllerAdministrativos.getCountBloqueados();
                    }
                    else
                    {
                        controllerAdministrativos.asignarCliente(cliente, vActual, reloj, controllerClientes, controllerEventos);
                        controllerClientes.crearNuevaLlegada(reloj, controllerEventos, cliente, vActual);
                        controllerAdministrativos.verificarOcioAdmMixto(controllerEventos, controllerClientes, reloj, vActual, 22);
                    }    

          
                }
                else
                {
                    if (controllerEventos.verificarEventoFinAtencion(eventoLista[i]))
                    {
                        Administrativo adm_a_cargo = eventoLista[i].getAdministrativo();


                        vActual[0] += "evento finAtencion /";
                        vActual[39] += 1;
                        

                        controllerAdministrativos.verificarNuevoCliente(adm_a_cargo, controllerClientes, controllerEventos, reloj, vActual);
                    }
                    else
                    {
                        if (controllerEventos.verificarEventoDescanso(eventoLista[i]))
                        {
                            vActual[0] += "evento Descanso /";
                            Administrativo admDescanso = eventoLista[i].getAdministrativo();
                            admDescanso.habilitarDescanso();
                            controllerAdministrativos.verificarDescanso(admDescanso, vActual, controllerEventos);
                        }
                        else
                        {
                            if (controllerEventos.verificarEventoFinDescanso(eventoLista[i]))
                            {
                                Administrativo adm = eventoLista[i].getAdministrativo();

                                vActual[0] += "evento fin Descanso /";
                                
                                controllerAdministrativos.terminarDescanso(adm, controllerClientes, controllerEventos, reloj, vActual);
                            }
                            else
                            {
                                if (controllerEventos.verificarEventoAtaque(eventoLista[i]))
                                {
                                    Ataque atq = (Ataque) eventoLista[i];
                                    vActual[0] += "evento de ataque (" + atq.getTipoAtaque() + ") / ";
                                    double fin;
                                    if(atq.getTipoAtaque() == "bloqueoClientes")
                                    {
                                        fin = RungeKutta.generarDuracionBloqueoClientes(reloj,form_RungeKutta,vActual) + reloj;
                                        bloqueoSistema = true;

                                    }
                                    else
                                    {
                                        fin = RungeKutta.generarAtaqueServidores(reloj, form_RungeKutta,vActual) + reloj;

                                        controllerAdministrativos.bloquearAdmin(vActual,controllerEventos,reloj,fin,controllerClientes);

                                        
                                    }

                                    controllerEventos.crearEventoFinAtaque(atq.getTipoAtaque(),fin,vActual);

                                }
                                else
                                {
                                    FinAtaque finAtq = (FinAtaque) eventoLista[i];
                                    vActual[0] += "evento fin de ataque (" + finAtq.getTipoAtaque() + ")";

                                    controllerEventos.crearEventoAtaque(A,reloj,form_RungeKutta,vActual);

                                    if(finAtq.getTipoAtaque() == "ataqueServ")
                                    {
                                        controllerAdministrativos.desbloquearAdm(controllerClientes,controllerEventos,reloj,vActual);
                                        vActual[46] = 0;
                                    }
                                    else
                                    {
                                        bloqueoSistema = false;
                                        controllerAdministrativos.reasignarBloqueados(vActual,reloj,controllerClientes,controllerEventos);
                                        vActual[44] = 0;
                                        vActual[45] = 0;
                                    }

                                }
                            }
                        }
                    }
                }
            }

            
            

            controllerEventos.removeEvento(reloj).ToString();
            vActual[34] = controllerAdministrativos.getCantidadMatricula();
            vActual[35] = controllerAdministrativos.getCantidadRenovacion();

            
            
        }

        private void generarDefecto_Click(object sender, EventArgs e)
        {
            this.mediaLlegadaM.Text = 2.91.ToString();
            this.aAtencionMatricula.Text = 8.71.ToString();
            this.bAtencionMatricula.Text = 15.2.ToString();

            this.desvAtencionR.Text = 5.ToString();
            this.mediaAtencionR.Text = 16.7.ToString();
            this.mediaLlegadaR.Text = 4.725.ToString();

            this.diasSimulacion.Text = 1.ToString();

            this.qEventosDesde.Text = 0.ToString();
            btn_generar_Click_1(sender, e);
        }

        private bool verificaciones()
        {
            if (aAtencionMatricula.Text.Length <= 0 ||
                bAtencionMatricula.Text.Length <= 0 ||
                mediaLlegadaM.Text.Length <= 0 ||
                mediaLlegadaR.Text.Length <= 0 ||
                desvAtencionR.Text.Length <= 0 ||
                diasSimulacion.Text.Length <= 0 ||
                qEventosDesde.Text.Length < 0)
            {
                MessageBox.Show("Alguno de los parametros se encuentra vacio, por favor verifique");
                return false;
            }

            if (double.Parse(aAtencionMatricula.Text) < 0 || double.Parse(bAtencionMatricula.Text) < 0)
            {
                MessageBox.Show("Alguno de los valores de la atencion de matricula es negativo...");
                return false;
            }

            if (double.Parse(aAtencionMatricula.Text) > double.Parse(bAtencionMatricula.Text))
            {
                MessageBox.Show("El limite inferior es mayor al superior en la atencion en matricula, verifique...");
                return false;
            }

            if (double.Parse(mediaLlegadaM.Text) <= 0 ||
                double.Parse(mediaAtencionR.Text) <= 0 ||
                double.Parse(mediaLlegadaR.Text) <= 0)
            {
                MessageBox.Show("Alguno de los valores de la media es negativo, verifique");
                return false;
            }

            if (double.Parse(desvAtencionR.Text) <= 0)
            {
                MessageBox.Show("La desviacion no puede ser negativa, verifique");
                return false;
            }

            if(double.Parse(diasSimulacion.Text) <= 0)
            {
                MessageBox.Show("La cantidad de dias a simular no puede ser menor o igual a 0 (cero)");
                return false;
            }
            if(double.Parse(qEventosDesde.Text) < 0)
            {
                MessageBox.Show("La cantidad de eventos no puede ser negativa...");
            }

            return true;
        }

        private void btn_rungekutta_Click(object sender, EventArgs e)
        {
            this.form_RungeKutta.ShowDialog();
        }
    }
}
