using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SchoolManager
{
    public partial class Expanded : Form
    {
        public Expanded()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void Form1_Deactivate(object sender, EventArgs e)
        {
            Hide();
        }
    }
}
