namespace DVLD
{
    partial class frmMainDashboard
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.Button btnApplications;
        private System.Windows.Forms.Button btnLicenses;
        private System.Windows.Forms.Button btnDrivers;
        private System.Windows.Forms.Button btnTests;
        private System.Windows.Forms.Button btnViolations;
        private System.Windows.Forms.Button btnLDLA;
        private System.Windows.Forms.Button btnDetainedLicenses;
        private System.Windows.Forms.Button btnInternationalLicenses;
        private System.Windows.Forms.Button btnUsers;
        private System.Windows.Forms.Button btnRoles;
        private System.Windows.Forms.Button btnMedicalCenters;
        private System.Windows.Forms.Button btnPayments;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Panel panelMenu;

        private void InitializeComponent()
        {
            this.panelMenu = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblUser = new System.Windows.Forms.Label();
            this.btnApplications = new System.Windows.Forms.Button();
            this.btnLicenses = new System.Windows.Forms.Button();
            this.btnDrivers = new System.Windows.Forms.Button();
            this.btnTests = new System.Windows.Forms.Button();
            this.btnViolations = new System.Windows.Forms.Button();
            this.btnLDLA = new System.Windows.Forms.Button();
            this.btnDetainedLicenses = new System.Windows.Forms.Button();
            this.btnInternationalLicenses = new System.Windows.Forms.Button();
            this.btnUsers = new System.Windows.Forms.Button();
            this.btnRoles = new System.Windows.Forms.Button();
            this.btnMedicalCenters = new System.Windows.Forms.Button();
            this.btnPayments = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.panelMenu.SuspendLayout();
            this.SuspendLayout();
            // panelMenu
            this.panelMenu.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);
            this.panelMenu.Controls.Add(this.btnLogout);
            this.panelMenu.Controls.Add(this.btnPayments);
            this.panelMenu.Controls.Add(this.btnMedicalCenters);
            this.panelMenu.Controls.Add(this.btnRoles);
            this.panelMenu.Controls.Add(this.btnUsers);
            this.panelMenu.Controls.Add(this.btnInternationalLicenses);
            this.panelMenu.Controls.Add(this.btnDetainedLicenses);
            this.panelMenu.Controls.Add(this.btnLDLA);
            this.panelMenu.Controls.Add(this.btnViolations);
            this.panelMenu.Controls.Add(this.btnTests);
            this.panelMenu.Controls.Add(this.btnDrivers);
            this.panelMenu.Controls.Add(this.btnLicenses);
            this.panelMenu.Controls.Add(this.btnApplications);
            this.panelMenu.Controls.Add(this.lblUser);
            this.panelMenu.Controls.Add(this.lblTitle);
            this.panelMenu.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelMenu.Location = new System.Drawing.Point(0, 0);
            this.panelMenu.Name = "panelMenu";
            this.panelMenu.Size = new System.Drawing.Size(220, 600);
            this.panelMenu.TabIndex = 0;
            // lblTitle
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(10, 10);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(200, 30);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "DVLD System";
            // lblUser
            this.lblUser.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblUser.ForeColor = System.Drawing.Color.LightGray;
            this.lblUser.Location = new System.Drawing.Point(10, 45);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(200, 20);
            this.lblUser.TabIndex = 1;
            this.lblUser.Text = "Welcome";
            // buttons
            int y = 80;
            string[] btnNames = { "Applications", "Licenses", "Drivers", "Tests", "Violations", 
                                    "LDLA", "DetainedLicenses", "InternationalLicenses", "Users", "Roles",
                                    "MedicalCenters", "Payments" };
            foreach (var name in btnNames)
            {
                var btn = (System.Windows.Forms.Button)this.GetType().GetField("btn" + name, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(this);
                btn.BackColor = System.Drawing.Color.FromArgb(62, 62, 66);
                btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                btn.Font = new System.Drawing.Font("Segoe UI", 9F);
                btn.ForeColor = System.Drawing.Color.White;
                btn.Location = new System.Drawing.Point(10, y);
                btn.Name = "btn" + name;
                btn.Size = new System.Drawing.Size(200, 30);
                btn.TabIndex = 2 + btnNames.ToList().IndexOf(name);
                btn.Text = name.Replace("Licenses", " Licenses").Replace("LDLA", "Local Driving License");
                btn.UseVisualStyleBackColor = false;
                btn.Click += new System.EventHandler(this.GetType().GetMethod("btn" + name + "_Click", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).CreateDelegate(typeof(System.EventHandler), this));
                y += 35;
            }
            // btnLogout
            this.btnLogout.BackColor = System.Drawing.Color.FromArgb(192, 0, 0);
            this.btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogout.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnLogout.ForeColor = System.Drawing.Color.White;
            this.btnLogout.Location = new System.Drawing.Point(10, 540);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(200, 35);
            this.btnLogout.TabIndex = 20;
            this.btnLogout.Text = "Logout";
            this.btnLogout.UseVisualStyleBackColor = false;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // frmMainDashboard
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(900, 600);
            this.Controls.Add(this.panelMenu);
            this.Name = "frmMainDashboard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DVLD - Main Dashboard";
            this.Load += new System.EventHandler(this.frmMainDashboard_Load);
            this.panelMenu.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}
