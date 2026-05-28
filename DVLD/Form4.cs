using DVLD.Business;
using DVLD.BusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD
{
    public partial class Form4 : Form
    {
        private List<UserListView> _allUsers;
        public Form4()
        {
            InitializeComponent();
        }
        private async Task LoadDataAsync()
        {
            List<UserListDTO>users = await clsUser.GetUsersForListAsync();

            _allUsers = users.Select(u => new UserListView
            {
                UserID = u.UserID,
                PersonID = u.PersonID,
                FullName = u.FullName,
                UserName = u.UserName,
                IsActive = u.IsActive,
            }).ToList();
            dGVUsers.DataSource = _allUsers;
        }
        private async void Form4_Load(object sender, EventArgs e)
        {
            cbFilterBy.Items.AddRange(new string[]
            {
                "None",
                "UserID",
                "PersonID",
                "FullName",
                "UserName",
                "IsActive"
            });

            cbFilterBy.SelectedIndex = 0;
            await LoadDataAsync();
            dGVUsers.Columns["FullName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        }
        
        private void dGVUsers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }
        private void ApplyFilter()
        {
            Dictionary<string, Func<UserListView, string>> map = new Dictionary<string, Func<UserListView, string>>
            {
                ["UserID"] = u => u.UserID.ToString(),
                ["PersonID"] = u => u.PersonID.ToString(),
                ["UserName"] = u => u.UserName,
                ["FullName"] = u => u.FullName,
                ["IsActive"] = u => u.IsActive ? "Active" : "Inactive"
            };

            var result = FilterHelper.ApplyFilter(
                _allUsers,
                cbFilterBy.SelectedItem?.ToString(),
                txtFilter.Text,
                map
            );

            dGVUsers.DataSource = null;
            dGVUsers.DataSource = result;
            dGVUsers.Columns["FullName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        }
    }
}
