using DVLD.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.LocalDrivingLicenseApplications
{
    public partial class frmManageLocalDrivingLicenseApplications : Form
    {
        private List<LocalDrivingLicenseApplicationListDto> _items = new List<LocalDrivingLicenseApplicationListDto>();

        public frmManageLocalDrivingLicenseApplications()
        {
            InitializeComponent();
        }

        private async void frmManageLocalDrivingLicenseApplications_Load(object sender, EventArgs e)
        {
            cbFilterBy.Items.AddRange(new object[] { "All", "Keyword" });
            cbFilterBy.SelectedIndex = 0;
            await LoadItemsAsync();
        }

        private async Task LoadItemsAsync(string keyword = "")
        {
            _items = string.IsNullOrWhiteSpace(keyword)
                ? await LocalDrivingLicenseApplicationService.GetForListAsync()
                : await LocalDrivingLicenseApplicationService.SearchAsync(keyword);

            dgvItems.DataSource = null;
            dgvItems.DataSource = _items;
            lblRecordsValue.Text = _items.Count.ToString();
        }

        private async void txtSearch_TextChanged(object sender, EventArgs e)
        {
            await LoadItemsAsync(txtSearch.Text.Trim());
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            await LoadItemsAsync();
        }

        private async void btnAddNew_Click(object sender, EventArgs e)
        {
            using (frmAddEditLocalDrivingLicenseApplication frm = new frmAddEditLocalDrivingLicenseApplication())
            {
                if (frm.ShowDialog(this) == DialogResult.OK)
                    await LoadItemsAsync();
            }
        }

        private async void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvItems.CurrentRow == null)
                return;

            int id = Convert.ToInt32(dgvItems.CurrentRow.Cells["LocalDrivingLicenseApplicationID"].Value);
            using (frmAddEditLocalDrivingLicenseApplication frm = new frmAddEditLocalDrivingLicenseApplication(id))
            {
                if (frm.ShowDialog(this) == DialogResult.OK)
                    await LoadItemsAsync();
            }
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvItems.CurrentRow == null)
                return;

            int id = Convert.ToInt32(dgvItems.CurrentRow.Cells["LocalDrivingLicenseApplicationID"].Value);
            using (frmLocalDrivingLicenseApplicationDetails frm = new frmLocalDrivingLicenseApplicationDetails(id))
            {
                frm.ShowDialog(this);
            }
        }

        private async void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvItems.CurrentRow == null)
                return;

            int id = Convert.ToInt32(dgvItems.CurrentRow.Cells["LocalDrivingLicenseApplicationID"].Value);
            if (MessageBox.Show($"Delete Local Driving License Application [{id}]?", "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != DialogResult.OK)
                return;

            Result<bool> result = await LocalDrivingLicenseApplicationService.DeleteAsync(id);
            MessageBox.Show(result.Success ? "Deleted successfully." : result.Message, "Local Driving License Applications",
                MessageBoxButtons.OK, result.Success ? MessageBoxIcon.Information : MessageBoxIcon.Error);

            if (result.Success)
                await LoadItemsAsync();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
