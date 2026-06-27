namespace DVLD.LocalDrivingLicenseApplications
{
    partial class frmLocalDrivingLicenseApplicationDetails
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtNotes = new System.Windows.Forms.TextBox();
            this.lblLicenseClassIdValue = new System.Windows.Forms.Label();
            this.lblApplicationIdValue = new System.Windows.Forms.Label();
            this.lblIdValue = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Roboto Slab", 18.8F);
            this.lblTitle.Location = new System.Drawing.Point(33, 21);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(470, 43);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Local Driving License Application Details";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtNotes);
            this.groupBox1.Controls.Add(this.lblLicenseClassIdValue);
            this.groupBox1.Controls.Add(this.lblApplicationIdValue);
            this.groupBox1.Controls.Add(this.lblIdValue);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(23, 87);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(487, 219);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Information";
            // 
            // txtNotes
            // 
            this.txtNotes.Location = new System.Drawing.Point(145, 102);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.ReadOnly = true;
            this.txtNotes.Size = new System.Drawing.Size(306, 91);
            this.txtNotes.TabIndex = 7;
            // 
            // lblLicenseClassIdValue
            // 
            this.lblLicenseClassIdValue.AutoSize = true;
            this.lblLicenseClassIdValue.Location = new System.Drawing.Point(142, 74);
            this.lblLicenseClassIdValue.Name = "lblLicenseClassIdValue";
            this.lblLicenseClassIdValue.Size = new System.Drawing.Size(14, 16);
            this.lblLicenseClassIdValue.TabIndex = 6;
            this.lblLicenseClassIdValue.Text = "0";
            // 
            // lblApplicationIdValue
            // 
            this.lblApplicationIdValue.AutoSize = true;
            this.lblApplicationIdValue.Location = new System.Drawing.Point(142, 49);
            this.lblApplicationIdValue.Name = "lblApplicationIdValue";
            this.lblApplicationIdValue.Size = new System.Drawing.Size(14, 16);
            this.lblApplicationIdValue.TabIndex = 5;
            this.lblApplicationIdValue.Text = "0";
            // 
            // lblIdValue
            // 
            this.lblIdValue.AutoSize = true;
            this.lblIdValue.Location = new System.Drawing.Point(142, 24);
            this.lblIdValue.Name = "lblIdValue";
            this.lblIdValue.Size = new System.Drawing.Size(14, 16);
            this.lblIdValue.TabIndex = 4;
            this.lblIdValue.Text = "0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(29, 105);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 16);
            this.label4.TabIndex = 3;
            this.label4.Text = "Notes :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(29, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(107, 16);
            this.label3.TabIndex = 2;
            this.label3.Text = "License Class ID :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(29, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "Application ID :";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "ID :";
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(393, 322);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(117, 43);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // frmLocalDrivingLicenseApplicationDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(533, 381);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblTitle);
            this.Name = "frmLocalDrivingLicenseApplicationDetails";
            this.Text = "Local Driving License Application Details";
            this.Load += new System.EventHandler(this.frmLocalDrivingLicenseApplicationDetails_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtNotes;
        private System.Windows.Forms.Label lblLicenseClassIdValue;
        private System.Windows.Forms.Label lblApplicationIdValue;
        private System.Windows.Forms.Label lblIdValue;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnClose;
    }
}
