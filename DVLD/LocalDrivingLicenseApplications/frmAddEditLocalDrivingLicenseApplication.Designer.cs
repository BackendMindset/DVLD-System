namespace DVLD.LocalDrivingLicenseApplications
{
    partial class frmAddEditLocalDrivingLicenseApplication
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
            this.lblMode = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblItemID = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtNotes = new System.Windows.Forms.TextBox();
            this.txtLicenseClassId = new System.Windows.Forms.TextBox();
            this.txtApplicationId = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblMode
            // 
            this.lblMode.AutoSize = true;
            this.lblMode.Font = new System.Drawing.Font("Roboto Slab", 20.8F);
            this.lblMode.Location = new System.Drawing.Point(54, 19);
            this.lblMode.Name = "lblMode";
            this.lblMode.Size = new System.Drawing.Size(301, 48);
            this.lblMode.TabIndex = 0;
            this.lblMode.Text = "Add New Record";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("PMingLiU-ExtB", 13.8F);
            this.label1.Location = new System.Drawing.Point(19, 88);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "ID :";
            // 
            // lblItemID
            // 
            this.lblItemID.AutoSize = true;
            this.lblItemID.Font = new System.Drawing.Font("PMingLiU-ExtB", 13.8F);
            this.lblItemID.Location = new System.Drawing.Point(68, 88);
            this.lblItemID.Name = "lblItemID";
            this.lblItemID.Size = new System.Drawing.Size(50, 23);
            this.lblItemID.TabIndex = 2;
            this.lblItemID.Text = "New";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtNotes);
            this.groupBox1.Controls.Add(this.txtLicenseClassId);
            this.groupBox1.Controls.Add(this.txtApplicationId);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(23, 129);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(482, 174);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Information";
            // 
            // txtNotes
            // 
            this.txtNotes.Location = new System.Drawing.Point(158, 96);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new System.Drawing.Size(280, 55);
            this.txtNotes.TabIndex = 5;
            // 
            // txtLicenseClassId
            // 
            this.txtLicenseClassId.Location = new System.Drawing.Point(158, 63);
            this.txtLicenseClassId.Name = "txtLicenseClassId";
            this.txtLicenseClassId.Size = new System.Drawing.Size(280, 24);
            this.txtLicenseClassId.TabIndex = 4;
            // 
            // txtApplicationId
            // 
            this.txtApplicationId.Location = new System.Drawing.Point(158, 30);
            this.txtApplicationId.Name = "txtApplicationId";
            this.txtApplicationId.Size = new System.Drawing.Size(280, 24);
            this.txtApplicationId.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(28, 99);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 16);
            this.label4.TabIndex = 2;
            this.label4.Text = "Notes :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(28, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(107, 16);
            this.label3.TabIndex = 1;
            this.label3.Text = "License Class ID :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(28, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 16);
            this.label2.TabIndex = 0;
            this.label2.Text = "Application ID :";
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(388, 319);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(117, 43);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(260, 319);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(117, 43);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // frmAddEditLocalDrivingLicenseApplication
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(528, 377);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblItemID);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblMode);
            this.Name = "frmAddEditLocalDrivingLicenseApplication";
            this.Text = "Local Driving License Application";
            this.Load += new System.EventHandler(this.frmAddEditLocalDrivingLicenseApplication_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblItemID;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtNotes;
        private System.Windows.Forms.TextBox txtLicenseClassId;
        private System.Windows.Forms.TextBox txtApplicationId;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSave;
    }
}
