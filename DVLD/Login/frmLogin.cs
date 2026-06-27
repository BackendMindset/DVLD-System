using DVLD.BusinessLayer;
using DVLD.Main;
using System;
using System.Windows.Forms;

namespace DVLD.Login
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string userName = txtUsername.Text?.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter username and password", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnLogin.Enabled = false;
            Cursor = Cursors.WaitCursor;

            try
            {
                UserService user = await UserService.LoginAsync(userName, password);

                if (user != null)
                {
                    Logger.LogInfo($"User '{userName}' logged in successfully", "Login");
                    Session.Start(user);
                    Hide();
                    using (frmMain frm = new frmMain())
                    {
                        frm.ShowDialog();
                    }
                    Close();
                }
                else
                {
                    Logger.LogWarning($"Failed login attempt for user '{userName}'", "Login");
                    MessageBox.Show("Invalid username or password", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Login");
                MessageBox.Show($"An error occurred during login:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnLogin.Enabled = true;
                Cursor = Cursors.Default;
            }
        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {
        }
    }
}
