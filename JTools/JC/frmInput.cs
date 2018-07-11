using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JC
{
    public partial class frmInput : Form
    {
        public string addInput;

        public frmInput()
        {
            InitializeComponent();
        }

        private void btInput_Click(object sender, EventArgs e)
        {
            addInput = " " + tbArgument.Text;
            Close();
        }

        private void tbArgument_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btInput_Click(sender, e);
        }

        private void frmInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        }

        private void frmInput_Load(object sender, EventArgs e)
        {
            tbArgument.Text = addInput;
        }
    }
}
