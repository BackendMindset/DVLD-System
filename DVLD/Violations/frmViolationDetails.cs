using DVLD.BusinessLayer;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Violations
{
    public partial class frmViolationDetails : Form
    {
        private readonly int _violationId;

        public frmViolationDetails(int violationId)
        {
            _violationId = violationId;
            InitializeComponent();
        }

        private async void frmViolationDetails_Load(object sender, EventArgs e)
        {
            await LoadViolationAsync();
        }

        private async Task LoadViolationAsync()
        {
            ViolationService violation = await ViolationService.FindByIDAsync(_violationId);
            if (violation == null)
            {
                MessageBox.Show("Violation not found.", "Violations", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }

            lblViolationIdValue.Text = violation.ID.ToString();
            lblPersonIdValue.Text = violation.PersonID.ToString();
            lblTypeIdValue.Text = violation.ViolationTypeID.ToString();
            lblStatusValue.Text = violation.Status;
            lblFineValue.Text = violation.FineAmount.ToString("0.00");
            lblIssueDateValue.Text = violation.IssueDate.ToString("dd/MM/yyyy");
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
