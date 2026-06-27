using System.Windows.Forms;

namespace DVLD.Users
{
    public partial class frmAddEditUser : Form
    {
        private async void frmAddEditUser_Load(object sender, System.EventArgs e)
        {
            await LoadUserAsync();
        }
    }
}
