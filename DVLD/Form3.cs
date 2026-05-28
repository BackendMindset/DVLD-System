using DVLD.BusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD
{
    public partial class frmAddEditPerson : Form
    {
        public enum enMode { AddNew = 0, Update = 1 };
        private enMode _Mode;
        private int _PersonId;
        private clsPerson _Person;
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
            bool result = await _Person.SaveAsync();
            if (result)
            {
                MessageBox.Show("Saved Successfully");
                DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Error while saving");
            }
        }
        private async Task LoadExistingPersonAsync()
        {
            _Person = await clsPerson.FindByIDAsync(_PersonId);
            if (_Person == null)
            {
                MessageBox.Show($"No Person with ID = {_PersonId}","Error",
                                  MessageBoxButtons.OK,MessageBoxIcon.Error);
                this.Close();
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
                _Person = new clsPerson();
                personCardControl1.SetPerson(_Person);
                return;
            }
            await LoadExistingPersonAsync();
        }
    }
}
