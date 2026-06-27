using DVLD.BusinessLayer;
using DVLD.Shared;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.People
{
    public partial class frmAddEditPerson : Form
    {
        public enum enMode { AddNew = 0, Update = 1 }

        private enMode _Mode;
        private int _PersonId;
        private PersonService _Person;

        public frmAddEditPerson(int PersonId)
        {
            InitializeComponent();
            _PersonId = PersonId;
            _Mode = (_PersonId == -1) ? enMode.AddNew : enMode.Update;
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateChildren())
                return;

            _Person = personCardControl1.GetPerson();
            Result<bool> result = await _Person.SaveAsync();
            if (result.Success)
            {
                MessageBox.Show("Saved Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                Logger.LogWarning($"Failed to save person: {result.Message}", "AddEditPerson");
                MessageBox.Show($"Error while saving: {result.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadExistingPersonAsync()
        {
            _Person = await PersonService.FindByIDAsync(_PersonId);
            if (_Person == null)
            {
                MessageBox.Show($"No Person with ID = {_PersonId}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }

            _Mode = enMode.Update;
            personCardControl1.SetPerson(_Person);
            lblMode.Text = $"Edit Contact ID = {_PersonId}";
            lblPersonID.Text = _PersonId.ToString();
        }

        private async void frmAddEditPerson_Load(object sender, EventArgs e)
        {
            if (_Mode == enMode.AddNew)
            {
                lblMode.Text = "Add New Contact";
                _Person = new PersonService();
                personCardControl1.SetPerson(_Person);
                return;
            }

            await LoadExistingPersonAsync();
        }
    }
}
