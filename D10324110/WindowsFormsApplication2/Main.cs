using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            Apply apply = new Apply();
            apply.Show();
            this.Hide();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            Adjust aj = new Adjust();
            aj.Show();
            this.Hide();
        }
    }
}
