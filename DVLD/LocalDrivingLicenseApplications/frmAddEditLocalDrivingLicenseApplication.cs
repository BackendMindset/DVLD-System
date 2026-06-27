using DVLD.BusinessLayer;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.LocalDrivingLicenseApplications
{
    public partial class frmAddEditLocalDrivingLicenseApplication : Form
    {
        private readonly int _id;
        private LocalDrivingLicenseApplicationService _item;

        public frmAddEditLocalDrivingLicenseApplication(int id = -1)
        {
            _id = id;
            InitializeComponent();
            lblMode.Text = _id > 0 ? "Edit Local Driving License Application" : "Add New Local Driving License Application";
            lblItemID.Text = _id > 0 ? _id.ToString() : "New";
        }

        private async void frmAddEditLocalDrivingLicenseApplication_Load(object sender, EventArgs e)
        {
            if (_id <= 0)
                return;

            _item = await LocalDrivingLicenseApplicationService.FindByIDAsync(_id);
            if (_item == null)
            {
                MessageBox.Show("Record not found.", "Local Driving License Applications", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }

            txtApplicationId.Text = _item.ApplicationID.ToString();
            txtLicenseClassId.Text = _item.LicenseClassID.ToString();
            txtNotes.Text = _item.Notes;
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtApplicationId.Text.Trim(), out int applicationId) || applicationId <= 0)
            {
                MessageBox.Show("Valid Application ID is required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtLicenseClassId.Text.Trim(), out int licenseClassId) || licenseClassId <= 0)
            {
                MessageBox.Show("Valid License Class ID is required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_item == null)
                _item = await LocalDrivingLicenseApplicationService.FindByIDAsync(_id) ?? CreateNewItem();

            _item.ApplicationID = applicationId;
            _item.LicenseClassID = licenseClassId;
            _item.Notes = txtNotes.Text.Trim();

            Result<bool> result = await _item.SaveAsync();
            MessageBox.Show(result.Success ? "Saved successfully." : result.Message, "Local Driving License Applications",
                MessageBoxButtons.OK, result.Success ? MessageBoxIcon.Information : MessageBoxIcon.Error);

            if (result.Success)
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private LocalDrivingLicenseApplicationService CreateNewItem()
        {
            return (LocalDrivingLicenseApplicationService)Activator.CreateInstance(typeof(LocalDrivingLicenseApplicationService), true);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
