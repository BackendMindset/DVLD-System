using DVLD.BusinessLayer;
using DVLD.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD
{
    public partial class Form1 : Form
    {
        public Form1()
        {

            InitializeComponent();
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            using (Form frm = new Contacts())
                frm.ShowDialog();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            using (Form frm = new Form4())
                frm.ShowDialog();
        }
    }
}
