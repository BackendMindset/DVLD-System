namespace DVLD.Applications
{
    partial class frmApplicationDetails
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
            this.lblDateValue = new System.Windows.Forms.Label();
            this.lblFeesValue = new System.Windows.Forms.Label();
            this.lblStatusValue = new System.Windows.Forms.Label();
            this.lblTypeIdValue = new System.Windows.Forms.Label();
            this.lblApplicantIdValue = new System.Windows.Forms.Label();
            this.lblApplicationIdValue = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Roboto Slab", 20.8F);
            this.lblTitle.Location = new System.Drawing.Point(140, 18);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(271, 48);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Application Details";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblDateValue);
            this.groupBox1.Controls.Add(this.lblFeesValue);
            this.groupBox1.Controls.Add(this.lblStatusValue);
            this.groupBox1.Controls.Add(this.lblTypeIdValue);
            this.groupBox1.Controls.Add(this.lblApplicantIdValue);
            this.groupBox1.Controls.Add(this.lblApplicationIdValue);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(22, 81);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(500, 245);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Information";
            // 
            // lblDateValue
            // 
            this.lblDateValue.AutoSize = true;
            this.lblDateValue.Location = new System.Drawing.Point(183, 217);
            this.lblDateValue.Name = "lblDateValue";
            this.lblDateValue.Size = new System.Drawing.Size(14, 16);
            this.lblDateValue.TabIndex = 11;
            this.lblDateValue.Text = "0";
            // 
            // lblFeesValue
            // 
            this.lblFeesValue.AutoSize = true;
            this.lblFeesValue.Location = new System.Drawing.Point(183, 181);
            this.lblFeesValue.Name = "lblFeesValue";
            this.lblFeesValue.Size = new System.Drawing.Size(14, 16);
            this.lblFeesValue.TabIndex = 10;
            this.lblFeesValue.Text = "0";
            // 
            // lblStatusValue
            // 
            this.lblStatusValue.AutoSize = true;
            this.lblStatusValue.Location = new System.Drawing.Point(183, 145);
            this.lblStatusValue.Name = "lblStatusValue";
            this.lblStatusValue.Size = new System.Drawing.Size(14, 16);
            this.lblStatusValue.TabIndex = 9;
            this.lblStatusValue.Text = "0";
            // 
            // lblTypeIdValue
            // 
            this.lblTypeIdValue.AutoSize = true;
            this.lblTypeIdValue.Location = new System.Drawing.Point(183, 109);
            this.lblTypeIdValue.Name = "lblTypeIdValue";
            this.lblTypeIdValue.Size = new System.Drawing.Size(14, 16);
            this.lblTypeIdValue.TabIndex = 8;
            this.lblTypeIdValue.Text = "0";
            // 
            // lblApplicantIdValue
            // 
            this.lblApplicantIdValue.AutoSize = true;
            this.lblApplicantIdValue.Location = new System.Drawing.Point(183, 73);
            this.lblApplicantIdValue.Name = "lblApplicantIdValue";
            this.lblApplicantIdValue.Size = new System.Drawing.Size(14, 16);
            this.lblApplicantIdValue.TabIndex = 7;
            this.lblApplicantIdValue.Text = "0";
            // 
            // lblApplicationIdValue
            // 
            this.lblApplicationIdValue.AutoSize = true;
            this.lblApplicationIdValue.Location = new System.Drawing.Point(183, 37);
            this.lblApplicationIdValue.Name = "lblApplicationIdValue";
            this.lblApplicationIdValue.Size = new System.Drawing.Size(14, 16);
            this.lblApplicationIdValue.TabIndex = 6;
            this.lblApplicationIdValue.Text = "0";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(34, 217);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(113, 16);
            this.label12.TabIndex = 5;
            this.label12.Text = "Application Date :";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(34, 181);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(72, 16);
            this.label10.TabIndex = 4;
            this.label10.Text = "Paid Fees :";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(34, 145);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(49, 16);
            this.label8.TabIndex = 3;
            this.label8.Text = "Status :";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(34, 109);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(126, 16);
            this.label6.TabIndex = 2;
            this.label6.Text = "Application Type ID :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(34, 73);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(79, 16);
            this.label4.TabIndex = 1;
            this.label4.Text = "Applicant ID :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(34, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 16);
            this.label2.TabIndex = 0;
            this.label2.Text = "Application ID :";
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(405, 337);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(117, 39);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // frmApplicationDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(545, 391);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblTitle);
            this.Name = "frmApplicationDetails";
            this.Text = "Application Details";
            this.Load += new System.EventHandler(this.frmApplicationDetails_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblDateValue;
        private System.Windows.Forms.Label lblFeesValue;
        private System.Windows.Forms.Label lblStatusValue;
        private System.Windows.Forms.Label lblTypeIdValue;
        private System.Windows.Forms.Label lblApplicantIdValue;
        private System.Windows.Forms.Label lblApplicationIdValue;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnClose;
    }
}
