using DVLD.BusinessLayer;
using System;
using System.Windows.Forms;

namespace DVLD
{
    public partial class frmMainDashboard : Form
    {
        public frmMainDashboard()
        {
            InitializeComponent();
        }

        private void frmMainDashboard_Load(object sender, EventArgs e)
        {
            lblUser.Text = $"Welcome: {Session.CurrentUser?.UserName ?? "Guest"}";
        }

        private void btnApplications_Click(object sender, EventArgs e)
        {
            using (var frm = new frmApplicationsList()) frm.ShowDialog();
        }

        private void btnLicenses_Click(object sender, EventArgs e)
        {
            using (var frm = new frmLicensesList()) frm.ShowDialog();
        }

        private void btnDrivers_Click(object sender, EventArgs e)
        {
            using (var frm = new frmDriversList()) frm.ShowDialog();
        }

        private void btnTests_Click(object sender, EventArgs e)
        {
            using (var frm = new frmTestAppointmentsList()) frm.ShowDialog();
        }

        private void btnViolations_Click(object sender, EventArgs e)
        {
            using (var frm = new frmViolationsList()) frm.ShowDialog();
        }

        private void btnLDLA_Click(object sender, EventArgs e)
        {
            using (var frm = new frmLocalDrivingLicenseApplicationsList()) frm.ShowDialog();
        }

        private void btnDetainedLicenses_Click(object sender, EventArgs e)
        {
            using (var frm = new frmDetainedLicensesList()) frm.ShowDialog();
        }

        private void btnInternationalLicenses_Click(object sender, EventArgs e)
        {
            using (var frm = new frmInternationalLicensesList()) frm.ShowDialog();
        }

        private void btnUsers_Click(object sender, EventArgs e)
        {
            using (var frm = new frmUsersList()) frm.ShowDialog();
        }

        private void btnRoles_Click(object sender, EventArgs e)
        {
            using (var frm = new frmRolesList()) frm.ShowDialog();
        }

        private void btnMedicalCenters_Click(object sender, EventArgs e)
        {
            using (var frm = new frmMedicalCentersList()) frm.ShowDialog();
        }

        private void btnPayments_Click(object sender, EventArgs e)
        {
            using (var frm = new frmPaymentsList()) frm.ShowDialog();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Session.End();
            this.Close();
        }
    }
}
