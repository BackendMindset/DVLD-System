using DVLD.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Licenses
{
    public partial class frmManageLicenses : Form
    {
        private List<LicenseListDto> _licenses = new List<LicenseListDto>();

        public frmManageLicenses()
        {
            InitializeComponent();
        }

        private async void frmManageLicenses_Load(object sender, EventArgs e)
        {
            cbFilterBy.Items.AddRange(new object[] { "All", "Keyword" });
            cbFilterBy.SelectedIndex = 0;
            await LoadLicensesAsync();
        }

        private async Task LoadLicensesAsync(string keyword = "")
        {
            _licenses = string.IsNullOrWhiteSpace(keyword)
                ? await LicenseService.GetLicensesForListAsync()
                : await LicenseService.SearchLicensesAsync(keyword);

            dgvLicenses.DataSource = null;
            dgvLicenses.DataSource = _licenses;
            lblRecordsValue.Text = _licenses.Count.ToString();
        }

        private async void txtSearch_TextChanged(object sender, EventArgs e)
        {
            await LoadLicensesAsync(txtSearch.Text.Trim());
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            await LoadLicensesAsync();
        }

        private void btnAddLicense_Click(object sender, EventArgs e)
        {
            MessageBox.Show("License issue flow will be connected in the next UI batch.", "Licenses", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvLicenses.CurrentRow == null)
                return;

            int licenseId = Convert.ToInt32(dgvLicenses.CurrentRow.Cells["LicenseID"].Value);
            using (frmLicenseDetails frm = new frmLicenseDetails(licenseId))
            {
                frm.ShowDialog(this);
            }
        }
    }
}
