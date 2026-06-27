using System;
using System.Windows.Forms;
using DVLD.Applications;
using DVLD.Drivers;
using DVLD.People;
using DVLD.Users;

namespace DVLD.Main
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (Form frm = new frmManagePeople())
                frm.ShowDialog();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
        }

        private void button4_Click(object sender, EventArgs e)
        {
            using (Form frm = new frmManageUsers())
                frm.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (Form frm = new frmManageApplications())
                frm.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (Form frm = new frmManageDrivers())
                frm.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }
    }
}
