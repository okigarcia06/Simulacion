using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SIM_TP5_VERSION_CHONA.Forms
{
    public partial class Form_RungeKutta : Form
    {
        public Form_RungeKutta()
        {
            InitializeComponent();
        }


        public void agregarLinea(dynamic[] v)
        {
            dataGridView1.Rows.Add(v[0], v[1], v[2], v[3], v[4], v[5], v[6], v[7], v[8], v[9], v[10], v[11], v[12], v[13], v[14]);
        }
    }
}
