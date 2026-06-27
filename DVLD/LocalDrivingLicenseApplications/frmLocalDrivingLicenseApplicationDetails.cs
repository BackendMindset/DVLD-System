using DVLD.BusinessLayer;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.LocalDrivingLicenseApplications
{
    public partial class frmLocalDrivingLicenseApplicationDetails : Form
    {
        private readonly int _id;

        public frmLocalDrivingLicenseApplicationDetails(int id)
        {
            _id = id;
            InitializeComponent();
        }

        private async void frmLocalDrivingLicenseApplicationDetails_Load(object sender, EventArgs e)
        {
            LocalDrivingLicenseApplicationService item = await LocalDrivingLicenseApplicationService.FindByIDAsync(_id);
            if (item == null)
            {
                MessageBox.Show("Record not found.", "Local Driving License Applications", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }

            lblIdValue.Text = item.ID.ToString();
            lblApplicationIdValue.Text = item.ApplicationID.ToString();
            lblLicenseClassIdValue.Text = item.LicenseClassID.ToString();
            txtNotes.Text = item.Notes;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
