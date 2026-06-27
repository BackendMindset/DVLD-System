using DVLD.BusinessLayer;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Drivers
{
    public partial class frmDriverDetails : Form
    {
        private readonly int _driverId;

        public frmDriverDetails(int driverId)
        {
            _driverId = driverId;
            InitializeComponent();
        }

        private async void frmDriverDetails_Load(object sender, EventArgs e)
        {
            await LoadDriverAsync();
        }

        private async Task LoadDriverAsync()
        {
            DriverService driver = await DriverService.FindByIDAsync(_driverId);
            if (driver == null || !driver.IsFound)
            {
                MessageBox.Show("Driver not found.", "Drivers", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }

            PersonService person = await PersonService.FindByIDAsync(driver.PersonID);
            lblDriverIdValue.Text = driver.ID.ToString();
            lblPersonIdValue.Text = driver.PersonID.ToString();
            lblCreatedDateValue.Text = driver.CreatedDate.ToString("dd/MM/yyyy");
            lblFullNameValue.Text = person == null ? "N/A" : $"{person.FirstName} {person.SecondName} {person.LastName}";
            lblNationalIdValue.Text = person?.NationalID ?? "N/A";
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
