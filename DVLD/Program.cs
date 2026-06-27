using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD.BusinessLayer;
using DVLD.Login;

namespace DVLD
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.ThreadException += (sender, e) =>
            {
                Logger.LogError(e.Exception, "UI Thread Exception");
                MessageBox.Show(
                    $"An unexpected error occurred:\n{e.Exception.Message}\n\nPlease check the log file for details.",
                    "Application Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            };

            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                var ex = e.ExceptionObject as Exception;
                Logger.LogError(ex, "Unhandled Domain Exception");
            };

            TaskScheduler.UnobservedTaskException += (sender, e) =>
            {
                Logger.LogError(e.Exception, "Unobserved Task Exception");
                e.SetObserved();
            };

            Application.Run(new frmLogin());
        }
    }
}
