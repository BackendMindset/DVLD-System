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
    public partial class Contacts : Form
    {
        private List<PersonListView> _allContacts;
        public Contacts()
        {
            
            InitializeComponent();
        }
        private async Task LoadContactsAsync()
        {
            List<PersonListDTO> list = await clsPerson.GetPersonsForList();
            _allContacts = list.Select(p => new PersonListView
            {
                    PersonID = p.PersonID,
                    NationalNO = p.NationalNO,
                    FirstName = p.FirstName,
                    SecondName = p.SecondName,
                    ThirdName = p.ThirdName,
                    LastName = p.LastName,
                    Gender = p.Gender,
                    DateOfBirth = p.DateOfBirth,
                    CountryName = p.CountryName,
                    Phone = p.Phone,
                    Email = p.Email
                })
                .ToList();
            dGVContactes.DataSource = _allContacts;
        }
        private async void Form2_Load(object sender, EventArgs e)
        {
            cbFilterBy.Items.AddRange(new string[]
            {
                "None",
                "PersonID",
                "NationalNO",
                "FirstName",
                "LastName",
                "Phone",
                "Email",
                "CountryName"
            });
            cbFilterBy.SelectedIndex = 0;
            try
            {
                btnAddNew.Enabled = false;
                await LoadContactsAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading contacts: " + ex.Message);
            }
            finally
            {
                btnAddNew.Enabled = true;
            }
        }
        private void dGVContactes_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                DataGridView.HitTestInfo hit = dGVContactes.HitTest(e.X, e.Y);

                if (hit.RowIndex >= 0)
                {
                    dGVContactes.ClearSelection();

                    dGVContactes.Rows[hit.RowIndex].Selected = true;

                    dGVContactes.CurrentCell =
                        dGVContactes.Rows[hit.RowIndex].Cells[0];
                }
            }
        }
        private async void btnAddNew_Click(object sender, EventArgs e)
        {
            using (frmAddEditPerson frm = new frmAddEditPerson(-1))
                frm.ShowDialog();
            await LoadContactsAsync();
        }
        private void dGVContactes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void label1_Click(object sender, EventArgs e)
        {

        }
        private async void editeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dGVContactes.SelectedRows.Count == 0)
                return;
            int id = Convert.ToInt32(dGVContactes.SelectedRows[0].Cells[0].Value);
            using (frmAddEditPerson frm = new frmAddEditPerson(id))
            {
                frm.ShowDialog();
            }
            await LoadContactsAsync();
        }
        private async void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (dGVContactes.SelectedRows.Count == 0)
                return;
            int id = Convert.ToInt32(dGVContactes.SelectedRows[0].Cells[0].Value);

            if (MessageBox.Show(
                $"Are you sure you want to delete contact [{id}]?",
                "Confirm Delete",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Warning) != DialogResult.OK)
                return;

            if (await clsPerson.DeletePerson(id))
            {
                MessageBox.Show("Contact deleted successfully.", "Success");
            }
            else
            {
                MessageBox.Show("Failed to delete contact.", "Error");
            }
            await LoadContactsAsync();
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }
        private void ApplyFilter()
        {
            Dictionary<string, Func<PersonListView, string>> map = new Dictionary<string, Func<PersonListView, string>>
            {
                ["PersonID"] = p => p.PersonID.ToString(),
                ["NationalNO"] = p => p.NationalNO,
                ["FirstName"] = p => p.FirstName,
                ["LastName"] = p => p.LastName,
                ["Phone"] = p => p.Phone,
                ["Email"] = p => p.Email,
                ["CountryName"] = p => p.CountryName
            };
            List<PersonListView> result = FilterHelper.ApplyFilter(
                _allContacts,
                cbFilterBy.SelectedItem?.ToString(),
                txtFilter.Text,
                map
            );
            dGVContactes.DataSource = null;
            dGVContactes.DataSource = result;
        }
    }
}

