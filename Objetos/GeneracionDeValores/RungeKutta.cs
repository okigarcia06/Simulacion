using SIM_TP5_VERSION_CHONA.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SIM_TP5_VERSION_CHONA.Objetos.GeneracionDeValores
{
    internal static class RungeKutta
    {
        public static double generarLlegadaAtaque(double A, Form_RungeKutta form_Runge, dynamic[] vActual)
        {
            Random random = new Random();
            double B = random.NextDouble();
            double h = 0.1;
            decimal x = 0;
            double tope = A * 2;

            double k1, k2, k3, k4;

            dynamic[] vEstadoRunge = new dynamic[15];

            vEstadoRunge[0] = "Llegada Ataque";
            vEstadoRunge[1] = "/";
            vEstadoRunge[2] = "/";
            vEstadoRunge[3] = "/";
            vEstadoRunge[4] = "/";
            vEstadoRunge[5] = "/";
            vEstadoRunge[6] = "/";
            vEstadoRunge[7] = "/";
            vEstadoRunge[8] = "/";
            vEstadoRunge[9] = "/";
            vEstadoRunge[10] = "/";
            vEstadoRunge[11] = "/";
            vEstadoRunge[12] = "/";
            vEstadoRunge[13] = 0;
            vEstadoRunge[14] = A;

            form_Runge.agregarLinea(vEstadoRunge);



            while (A < tope)
            {
                k1 = A * B;
                double y_mas_k1_x_h_sobre_2 = A + k1 * h / 2;
                k2 = y_mas_k1_x_h_sobre_2 * B;
                double y_mas_k2_x_h_sobre_2 = A + k2 * h / 2;
                k3 = y_mas_k2_x_h_sobre_2 * B;
                double y_mas_k3_x_h = A + k3 * h;
                k4 = y_mas_k3_x_h * B;

                vEstadoRunge[1] = x;
                vEstadoRunge[2] = A;
                vEstadoRunge[3] = k1;
                vEstadoRunge[4] = "/";
                vEstadoRunge[5] = y_mas_k1_x_h_sobre_2;
                vEstadoRunge[6] = k2;
                vEstadoRunge[7] = "/";
                vEstadoRunge[8] = y_mas_k2_x_h_sobre_2;
                vEstadoRunge[9] = k3;
                vEstadoRunge[10] = "/";
                vEstadoRunge[11] = y_mas_k3_x_h;
                vEstadoRunge[12] = k4;

                A = A + h / 6 * (k1 + 2 * k2 + 2 * k3 + k4);
                x += decimal.Parse("0.1");

                vEstadoRunge[13] = Math.Round(x,2);
                vEstadoRunge[14] = A;

                form_Runge.agregarLinea(vEstadoRunge);
            }

            return double.Parse(""+x) * 9;
        }


        public static double generarDuracionBloqueoClientes(double reloj, Form_RungeKutta form_Runge, dynamic[] vActual)
        {
            //DL / dt = -(L / 0.8 * t^2) - L
            //L(0) = reloj de la simulacion
            //t = 5 mins

            double l = reloj;

            double h = 0.1;
            decimal x = 0;

            double k1, k2, k3, k4;

            dynamic[] vEstadoRunge = new dynamic [15];

            vEstadoRunge[0] = "Duracion Bloqueo Clientes";
            vEstadoRunge[1] = "/";
            vEstadoRunge[2] = "/";
            vEstadoRunge[3] = "/";
            vEstadoRunge[4] = "/";
            vEstadoRunge[5] = "/";
            vEstadoRunge[6] = "/";
            vEstadoRunge[7] = "/";
            vEstadoRunge[8] = "/";
            vEstadoRunge[9] = "/";
            vEstadoRunge[10] = "/";
            vEstadoRunge[11] = "/";
            vEstadoRunge[12] = "/";
            vEstadoRunge[13] = 0;
            vEstadoRunge[14] = l;

            form_Runge.agregarLinea(vEstadoRunge);

            do
            {
                k1 = -(l / 0.8 * Math.Pow(double.Parse("" + x),2)) - l;
                double x_mas_h_sobre_2 = double.Parse("" + x) + h / 2;

                double y_mas_k1_x_h_sobre_2 = l + k1 * h / 2;

                k2 = -(y_mas_k1_x_h_sobre_2 / 0.8 * Math.Pow(x_mas_h_sobre_2, 2)) - y_mas_k1_x_h_sobre_2;

                double y_mas_k2_x_h_sobre_2 = l + k2 * h / 2;

                k3 = -(y_mas_k2_x_h_sobre_2 / 0.8 * Math.Pow(x_mas_h_sobre_2,2)) - y_mas_k2_x_h_sobre_2;

                double y_mas_k3_x_h = l + k3 * h;

                double x_mas_h = double.Parse("" + x) + h;

                k4 = -(y_mas_k3_x_h / 0.8 * Math.Pow(x_mas_h,2)) - y_mas_k3_x_h;

                vEstadoRunge[1] = x;
                vEstadoRunge[2] = l;
                vEstadoRunge[3] = k1;
                vEstadoRunge[4] = x_mas_h_sobre_2;
                vEstadoRunge[5] = y_mas_k1_x_h_sobre_2;
                vEstadoRunge[6] = k2;
                vEstadoRunge[7] = x_mas_h_sobre_2;
                vEstadoRunge[8] = y_mas_k2_x_h_sobre_2;
                vEstadoRunge[9] = k3;
                vEstadoRunge[10] = x_mas_h;
                vEstadoRunge[11] = y_mas_k3_x_h;
                vEstadoRunge[12] = k4;

                l = l + h / 6 * (k1 + 2 * k2 + 2 * k3 + k4);
                x += decimal.Parse("0.1");

                vEstadoRunge[13] = Math.Round(x, 2);
                vEstadoRunge[14] = l;

                form_Runge.agregarLinea(vEstadoRunge);
            }
            while ((vEstadoRunge[14] - vEstadoRunge[2]) >= 1);

            return double.Parse("" + x) * 5;
        }

        public static double generarAtaqueServidores(double reloj,Form_RungeKutta form_Runge, dynamic[] vActual)
        {
            // DS / dt = (0.2 * S) + 3 - t
            //S(0) = reloj de la simulacion
            //t = 2 mins

            double h = 0.1;
            decimal x = 0;
            double s = reloj;

            double tope = reloj * 135 / 100;

            double k1, k2, k3, k4;

            dynamic[] vEstadoRunge = new dynamic[15];

            vEstadoRunge[0] = "Duracion Ataque Servidores";
            vEstadoRunge[1] = "/";
            vEstadoRunge[2] = "/";
            vEstadoRunge[3] = "/";
            vEstadoRunge[4] = "/";
            vEstadoRunge[5] = "/";
            vEstadoRunge[6] = "/";
            vEstadoRunge[7] = "/";
            vEstadoRunge[8] = "/";
            vEstadoRunge[9] = "/";
            vEstadoRunge[10] = "/";
            vEstadoRunge[11] = "/";
            vEstadoRunge[12] = "/";
            vEstadoRunge[13] = 0;
            vEstadoRunge[14] = s;

            form_Runge.agregarLinea(vEstadoRunge);

            while (s < tope)
            {
                k1 = (0.2 * s) + 3 - double.Parse("" + x);
                double x_mas_h_sobre_2 = double.Parse("" + x) + h / 2;
 
                double y_mas_k1_x_h_sobre_2 = s + k1 * h / 2;

                k2 = (0.2 * y_mas_k1_x_h_sobre_2) + 3 - x_mas_h_sobre_2;

                double y_mas_k2_x_h_sobre_2 = s + k2 * h / 2;

                k3 = (y_mas_k2_x_h_sobre_2 * 0.2) + 3 - x_mas_h_sobre_2;

                double y_mas_k3_x_h = s + k3 * h;

                double x_mas_h = double.Parse("" + x) + h;
                k4 = (y_mas_k3_x_h * 0.2) + 3 - x_mas_h;

                vEstadoRunge[1] = x;
                vEstadoRunge[2] = s;
                vEstadoRunge[3] = k1;
                vEstadoRunge[4] = x_mas_h_sobre_2;
                vEstadoRunge[5] = y_mas_k1_x_h_sobre_2;
                vEstadoRunge[6] = k2;
                vEstadoRunge[7] = x_mas_h_sobre_2;
                vEstadoRunge[8] = y_mas_k2_x_h_sobre_2;
                vEstadoRunge[9] = k3;
                vEstadoRunge[10] = x_mas_h;
                vEstadoRunge[11] = y_mas_k3_x_h;
                vEstadoRunge[12] = k4;

                s = s + h / 6 * (k1 + 2 * k2 + 2 * k3 + k4);
                x += decimal.Parse("0.1");

                vEstadoRunge[13] = Math.Round(x, 2);
                vEstadoRunge[14] = s;

                form_Runge.agregarLinea(vEstadoRunge);
            }

            return double.Parse("" + x) * 2;
        }
    }
}
