using DVLD.BusinessLayer;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Applications
{
    public partial class frmApplicationDetails : Form
    {
        private readonly int _applicationId;

        public frmApplicationDetails(int applicationId)
        {
            _applicationId = applicationId;
            InitializeComponent();
        }

        private async void frmApplicationDetails_Load(object sender, EventArgs e)
        {
            await LoadApplicationAsync();
        }

        private async Task LoadApplicationAsync()
        {
            ApplicationService application = await ApplicationService.FindByIDAsync(_applicationId);
            if (application == null)
            {
                MessageBox.Show("Application not found.", "Applications", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }

            lblApplicationIdValue.Text = application.ID.ToString();
            lblApplicantIdValue.Text = application.ApplicantPersonID.ToString();
            lblTypeIdValue.Text = application.ApplicationTypeID.ToString();
            lblStatusValue.Text = application.ApplicationStatus.ToString();
            lblFeesValue.Text = application.PaidFees.ToString("0.00");
            lblDateValue.Text = application.ApplicationDate.ToString("dd/MM/yyyy");
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
