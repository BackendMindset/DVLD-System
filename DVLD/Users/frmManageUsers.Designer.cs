namespace DVLD.Users
{
    partial class frmManageUsers
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.dGVUsers = new System.Windows.Forms.DataGridView();
            this.cbFilterBy = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnAddNewUser = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.txtFilter = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dGVUsers)).BeginInit();
            this.SuspendLayout();
            // 
            // dGVUsers
            // 
            this.dGVUsers.AllowUserToAddRows = false;
            this.dGVUsers.AllowUserToDeleteRows = false;
            this.dGVUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dGVUsers.Location = new System.Drawing.Point(3, 131);
            this.dGVUsers.Name = "dGVUsers";
            this.dGVUsers.ReadOnly = true;
            this.dGVUsers.RowHeadersWidth = 51;
            this.dGVUsers.RowTemplate.Height = 26;
            this.dGVUsers.Size = new System.Drawing.Size(736, 385);
            this.dGVUsers.TabIndex = 1;
            this.dGVUsers.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dGVUsers_CellContentClick);
            // 
            // cbFilterBy
            // 
            this.cbFilterBy.FormattingEnabled = true;
            this.cbFilterBy.Location = new System.Drawing.Point(91, 101);
            this.cbFilterBy.Name = "cbFilterBy";
            this.cbFilterBy.Size = new System.Drawing.Size(170, 24);
            this.cbFilterBy.TabIndex = 2;
            this.cbFilterBy.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Roboto Slab", 10.8F);
            this.label1.Location = new System.Drawing.Point(-1, 101);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 24);
            this.label1.TabIndex = 3;
            this.label1.Text = "FilterBy :";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Roboto Slab", 20.8F);
            this.label2.Location = new System.Drawing.Point(254, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(255, 48);
            this.label2.TabIndex = 4;
            this.label2.Text = "Manage Users";
            // 
            // btnAddNewUser
            // 
            this.btnAddNewUser.Font = new System.Drawing.Font("Roboto Slab", 12.8F);
            this.btnAddNewUser.Location = new System.Drawing.Point(536, 81);
            this.btnAddNewUser.Name = "btnAddNewUser";
            this.btnAddNewUser.Size = new System.Drawing.Size(203, 44);
            this.btnAddNewUser.TabIndex = 5;
            this.btnAddNewUser.Text = "Add New User";
            this.btnAddNewUser.UseVisualStyleBackColor = true;
            this.btnAddNewUser.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Roboto Slab", 12.8F);
            this.button2.Location = new System.Drawing.Point(596, 522);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(122, 44);
            this.button2.TabIndex = 6;
            this.button2.Text = "Close";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // txtFilter
            // 
            this.txtFilter.Location = new System.Drawing.Point(267, 101);
            this.txtFilter.Name = "txtFilter";
            this.txtFilter.Size = new System.Drawing.Size(182, 24);
            this.txtFilter.TabIndex = 7;
            this.txtFilter.TextChanged += new System.EventHandler(this.txtFilter_TextChanged);
            // 
            // frmManageUsers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(738, 567);
            this.Controls.Add(this.txtFilter);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btnAddNewUser);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbFilterBy);
            this.Controls.Add(this.dGVUsers);
            this.Name = "frmManageUsers";
            this.Text = "Manage Users";
            this.Load += new System.EventHandler(this.Form4_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dGVUsers)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.DataGridView dGVUsers;
        private System.Windows.Forms.ComboBox cbFilterBy;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnAddNewUser;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox txtFilter;
    }
}
