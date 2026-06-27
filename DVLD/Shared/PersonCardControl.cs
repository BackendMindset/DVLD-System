using DVLD.BusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Shared
{
    public partial class PersonCardControl : UserControl
    {
        public enum enMode { AddNew = 0, Update = 1 }

        private enMode _Mode;
        private int _PersonID;
        private PersonService _Person;

        public PersonCardControl()
        {
            InitializeComponent();
        }

        public PersonService GetPerson()
        {
            if (_Person == null)
                _Person = new PersonService();

            _Person.FirstName = txtFirstName.Text;
            _Person.SecondName = txtSecondName.Text;
            _Person.ThirdName = txtThirdName.Text;
            _Person.LastName = txtLastName.Text;
            _Person.NationalID = txtNationalNo.Text;
            _Person.Email = txtEmail.Text;
            _Person.Gender = rbMale.Checked ? PersonService.GenderType.Male : PersonService.GenderType.Female;
            _Person.Phone = txtPhone.Text;
            _Person.Address = txtAddress.Text;
            _Person.DateOfBirth = dtpDateOfBirth.Value;
            _Person.ImagePath = pictureBox2.ImageLocation;
            _Person.CountryID = cbCountry.SelectedValue is int id ? id : -1;

            return _Person;
        }

        public void SetPerson(PersonService person)
        {
            _Person = person;

            if (_Person == null)
                return;

            txtFirstName.Text = _Person.FirstName;
            txtSecondName.Text = _Person.SecondName;
            txtThirdName.Text = _Person.ThirdName;
            txtLastName.Text = _Person.LastName;
            txtEmail.Text = _Person.Email;
            txtPhone.Text = _Person.Phone;
            txtAddress.Text = _Person.Address;
            txtNationalNo.Text = _Person.NationalID;

            DateTime maxDate = dtpDateOfBirth.MaxDate;
            DateTime minDate = dtpDateOfBirth.MinDate;
            DateTime dob = _Person.DateOfBirth;

            if (dob > maxDate)
                dob = maxDate;
            if (dob < minDate)
                dob = minDate;

            dtpDateOfBirth.Value = dob;
            rbMale.Checked = _Person.Gender == PersonService.GenderType.Male;
            rbFemale.Checked = _Person.Gender == PersonService.GenderType.Female;
            cbCountry.SelectedValue = _Person.CountryID;
            pictureBox2.Image = null;
            pictureBox2.ImageLocation = null;

            if (!string.IsNullOrEmpty(_Person.ImagePath))
            {
                try
                {
                    if (System.IO.File.Exists(_Person.ImagePath))
                    {
                        pictureBox2.Load(_Person.ImagePath);
                        llRemoveImage.Visible = true;
                    }
                    else
                    {
                        llRemoveImage.Visible = false;
                    }
                }
                catch
                {
                    pictureBox2.Image = null;
                    pictureBox2.ImageLocation = null;
                    llRemoveImage.Visible = false;
                }
            }
            else
            {
                llRemoveImage.Visible = false;
            }
        }

        private async Task FillCountriesAsync()
        {
            List<CountryService> countries = await CountryService.GetAllAsync();
            if (countries == null)
                return;

            cbCountry.DataSource = countries;
            cbCountry.DisplayMember = "CountryName";
            cbCountry.ValueMember = "ID";
        }

        private async void PersonCardControl_Load(object sender, EventArgs e)
        {
            await FillCountriesAsync();
            dtpDateOfBirth.MaxDate = DateTime.Today.AddYears(-18);

            if (_Person != null)
                SetPerson(_Person);
        }

        private void llOpenFileDialog_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            openFileDialog1.FilterIndex = 1;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                pictureBox2.Load(openFileDialog1.FileName);
        }

        private void llRemoveImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            pictureBox2.Image = null;
            pictureBox2.ImageLocation = null;
            llRemoveImage.Visible = false;
        }

        private async void txtNationalNo_Validating_1(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNationalNo.Text))
            {
                errorProvider1.SetError(txtNationalNo, "National number is required");
                e.Cancel = true;
                return;
            }

            bool exists = await PersonService.ExistsNationalIDAsync(txtNationalNo.Text.Trim());
            if (exists)
            {
                errorProvider1.SetError(txtNationalNo, "National number already exists");
                e.Cancel = true;
            }
            else
            {
                errorProvider1.SetError(txtNationalNo, "");
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {
        }

        private void label12_Click(object sender, EventArgs e)
        {
        }
    }
}
