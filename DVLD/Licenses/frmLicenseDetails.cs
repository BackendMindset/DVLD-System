using DVLD.BusinessLayer;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Licenses
{
    public partial class frmLicenseDetails : Form
    {
        private readonly int _licenseId;

        public frmLicenseDetails(int licenseId)
        {
            _licenseId = licenseId;
            InitializeComponent();
        }

        private async void frmLicenseDetails_Load(object sender, EventArgs e)
        {
            await LoadLicenseAsync();
        }

        private async Task LoadLicenseAsync()
        {
            LicenseService license = await LicenseService.FindByIDAsync(_licenseId);
            if (license == null)
            {
                MessageBox.Show("License not found.", "Licenses", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }

            lblLicenseIdValue.Text = license.ID.ToString();
            lblDriverIdValue.Text = license.DriverID.ToString();
            lblClassIdValue.Text = license.LicenseClassID.ToString();
            lblIssueDateValue.Text = license.IssueDate.ToString("dd/MM/yyyy");
            lblExpirationDateValue.Text = license.ExpirationDate.ToString("dd/MM/yyyy");
            lblStatusValue.Text = license.LicenseStatus;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
