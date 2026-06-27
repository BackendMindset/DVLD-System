using DVLD.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Violations
{
    public partial class frmManageViolations : Form
    {
        private List<ViolationListDto> _violations = new List<ViolationListDto>();

        public frmManageViolations()
        {
            InitializeComponent();
        }

        private async void frmManageViolations_Load(object sender, EventArgs e)
        {
            cbFilterBy.Items.AddRange(new object[] { "All", "Keyword" });
            cbFilterBy.SelectedIndex = 0;
            await LoadViolationsAsync();
        }

        private async Task LoadViolationsAsync(string keyword = "")
        {
            _violations = string.IsNullOrWhiteSpace(keyword)
                ? await ViolationService.GetViolationsForListAsync()
                : await ViolationService.SearchViolationsAsync(keyword);

            dgvViolations.DataSource = null;
            dgvViolations.DataSource = _violations;
            lblRecordsValue.Text = _violations.Count.ToString();
        }

        private async void txtSearch_TextChanged(object sender, EventArgs e)
        {
            await LoadViolationsAsync(txtSearch.Text.Trim());
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            await LoadViolationsAsync();
        }

        private void btnAddViolation_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Violation add flow will be connected in the next UI batch.", "Violations", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvViolations.CurrentRow == null)
                return;

            int violationId = Convert.ToInt32(dgvViolations.CurrentRow.Cells["ViolationID"].Value);
            using (frmViolationDetails frm = new frmViolationDetails(violationId))
            {
                frm.ShowDialog(this);
            }
        }
    }
}
