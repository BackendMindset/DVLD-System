using DVLD.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Users
{
    public partial class frmManageUsers : Form
    {
        private List<UserListView> _allUsers;

        public frmManageUsers()
        {
            InitializeComponent();
        }

        private async Task LoadDataAsync()
        {
            List<UserListDto> users = await UserService.GetUsersForListAsync();

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
            using (frmAddEditUser frm = new frmAddEditUser())
            {
                if (frm.ShowDialog(this) == DialogResult.OK)
                {
                    _ = LoadDataAsync();
                }
            }
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

            List<UserListView> result = FilterHelper.ApplyFilter(
                _allUsers,
                cbFilterBy.SelectedItem?.ToString(),
                txtFilter.Text,
                map);

            dGVUsers.DataSource = null;
            dGVUsers.DataSource = result;
            dGVUsers.Columns["FullName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        }
    }
}
