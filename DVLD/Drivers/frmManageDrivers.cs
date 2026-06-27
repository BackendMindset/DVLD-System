using DVLD.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Drivers
{
    public partial class frmManageDrivers : Form
    {
        private List<DriverListDto> _drivers = new List<DriverListDto>();

        public frmManageDrivers()
        {
            InitializeComponent();
        }

        private async void frmManageDrivers_Load(object sender, EventArgs e)
        {
            cbFilterBy.Items.AddRange(new object[] { "All", "Keyword" });
            cbFilterBy.SelectedIndex = 0;
            await LoadDriversAsync();
        }

        private async Task LoadDriversAsync(string keyword = "")
        {
            _drivers = string.IsNullOrWhiteSpace(keyword)
                ? await DriverService.GetDriversForListAsync()
                : await DriverService.SearchDriversAsync(keyword);

            dgvDrivers.DataSource = null;
            dgvDrivers.DataSource = _drivers;
            lblRecordsValue.Text = _drivers.Count.ToString();
        }

        private async void txtSearch_TextChanged(object sender, EventArgs e)
        {
            await LoadDriversAsync(txtSearch.Text.Trim());
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            await LoadDriversAsync();
        }

        private void btnAddDriver_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Driver add flow will be connected in the next UI batch.", "Drivers", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvDrivers.CurrentRow == null)
                return;

            int driverId = Convert.ToInt32(dgvDrivers.CurrentRow.Cells["DriverID"].Value);
            using (frmDriverDetails frm = new frmDriverDetails(driverId))
            {
                frm.ShowDialog(this);
            }
        }
    }
}
