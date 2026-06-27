using DVLD.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Applications
{
    public partial class frmManageApplications : Form
    {
        private List<ApplicationListDto> _applications = new List<ApplicationListDto>();

        public frmManageApplications()
        {
            InitializeComponent();
        }

        private async void frmManageApplications_Load(object sender, EventArgs e)
        {
            cbFilterBy.Items.AddRange(new object[] { "All", "Keyword" });
            cbFilterBy.SelectedIndex = 0;
            await LoadApplicationsAsync();
        }

        private async Task LoadApplicationsAsync(string keyword = "")
        {
            _applications = string.IsNullOrWhiteSpace(keyword)
                ? await ApplicationService.GetApplicationsForListAsync()
                : await ApplicationService.SearchApplicationsAsync(keyword);

            dgvApplications.DataSource = null;
            dgvApplications.DataSource = _applications;
            lblRecordsValue.Text = _applications.Count.ToString();
        }

        private async void txtSearch_TextChanged(object sender, EventArgs e)
        {
            await LoadApplicationsAsync(txtSearch.Text.Trim());
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            await LoadApplicationsAsync();
        }

        private void btnAddApplication_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Application add flow will be connected in the next UI batch.", "Applications", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvApplications.CurrentRow == null)
                return;

            int applicationId = Convert.ToInt32(dgvApplications.CurrentRow.Cells["ApplicationID"].Value);
            using (frmApplicationDetails frm = new frmApplicationDetails(applicationId))
            {
                frm.ShowDialog(this);
            }
        }
    }
}
