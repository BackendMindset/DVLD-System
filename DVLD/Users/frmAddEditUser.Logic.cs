using DVLD.BusinessLayer;
using DVLD.People;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Users
{
    public partial class frmAddEditUser : Form
    {
        private readonly int _userId;
        private UserService _user;

        public frmAddEditUser(int userId = -1)
        {
            _userId = userId;
            InitializeComponent();
            lblMode.Text = IsEditMode ? "Edit User" : "Add New User";
            lblUserID.Text = IsEditMode ? _userId.ToString() : "New";
            lblPasswordHint.Text = IsEditMode
                ? "Leave blank to keep current password."
                : "Password is required for new users.";
        }

        private bool IsEditMode => _userId > 0;

        private async Task LoadUserAsync()
        {
            if (!IsEditMode)
                return;

            try
            {
                _user = await UserService.FindByIDAsync(_userId);
                if (_user == null)
                {
                    MessageBox.Show("User not found.", "Users", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Close();
                    return;
                }

                txtPersonId.Text = _user.PersonID.ToString();
                txtPersonId.Enabled = false;
                txtUserName.Text = _user.UserName;
                chkIsActive.Checked = _user.IsActive;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Load User");
                MessageBox.Show(ex.Message, "Users", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }

        private void btnBrowsePeople_Click(object sender, EventArgs e)
        {
            using (frmManagePeople frm = new frmManagePeople())
                frm.ShowDialog(this);
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            await SaveAsync();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private async Task SaveAsync()
        {
            if (!int.TryParse(txtPersonId.Text.Trim(), out int personId) || personId <= 0)
            {
                MessageBox.Show("Enter a valid Person ID.", "Users", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPersonId.Focus();
                return;
            }

            string userName = txtUserName.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(userName))
            {
                MessageBox.Show("Username is required.", "Users", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUserName.Focus();
                return;
            }

            if (!IsEditMode && string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Password is required for a new user.", "Users", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return;
            }

            btnSave.Enabled = false;
            Cursor = Cursors.WaitCursor;

            try
            {
                if (IsEditMode)
                    await SaveExistingUserAsync(userName, password);
                else
                    await SaveNewUserAsync(personId, userName, password);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Save User");
                MessageBox.Show(ex.Message, "Users", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnSave.Enabled = true;
                Cursor = Cursors.Default;
            }
        }

        private async Task SaveNewUserAsync(int personId, string userName, string password)
        {
            Result<int> createResult = await UserService.RegisterUserAsync(userName, password, personId);
            if (!createResult.Success)
            {
                MessageBox.Show(createResult.Message, "Users", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            UserService createdUser = await UserService.FindByIDAsync(createResult.Data);
            if (createdUser != null && createdUser.IsActive != chkIsActive.Checked)
            {
                Result<bool> activeResult = await createdUser.SetActiveAsync(chkIsActive.Checked);
                if (!activeResult.Success)
                {
                    MessageBox.Show(activeResult.Message, "Users", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            MessageBox.Show("User created successfully.", "Users", MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult = DialogResult.OK;
            Close();
        }

        private async Task SaveExistingUserAsync(string userName, string password)
        {
            bool activeChanged = _user.IsActive != chkIsActive.Checked;

            _user.UserName = userName;
            _user.IsActive = chkIsActive.Checked;

            Result<bool> saveResult = await _user.SaveAsync();
            if (!saveResult.Success)
            {
                MessageBox.Show(saveResult.Message, "Users", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!string.IsNullOrWhiteSpace(password))
            {
                Result<bool> passwordResult = await _user.ChangePasswordAsync(password);
                if (!passwordResult.Success)
                {
                    MessageBox.Show(passwordResult.Message, "Users", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            if (activeChanged)
            {
                Result<bool> activeResult = await _user.SetActiveAsync(chkIsActive.Checked);
                if (!activeResult.Success)
                {
                    MessageBox.Show(activeResult.Message, "Users", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            MessageBox.Show("User updated successfully.", "Users", MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
