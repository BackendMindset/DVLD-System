namespace DVLD.Drivers
{
    partial class frmDriverDetails
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
            this.lblNationalIdValue = new System.Windows.Forms.Label();
            this.lblFullNameValue = new System.Windows.Forms.Label();
            this.lblCreatedDateValue = new System.Windows.Forms.Label();
            this.lblPersonIdValue = new System.Windows.Forms.Label();
            this.lblDriverIdValue = new System.Windows.Forms.Label();
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
            this.lblTitle.Location = new System.Drawing.Point(177, 19);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(204, 48);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Driver Details";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblNationalIdValue);
            this.groupBox1.Controls.Add(this.lblFullNameValue);
            this.groupBox1.Controls.Add(this.lblCreatedDateValue);
            this.groupBox1.Controls.Add(this.lblPersonIdValue);
            this.groupBox1.Controls.Add(this.lblDriverIdValue);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(22, 81);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(500, 226);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Information";
            // 
            // lblNationalIdValue
            // 
            this.lblNationalIdValue.AutoSize = true;
            this.lblNationalIdValue.Location = new System.Drawing.Point(182, 184);
            this.lblNationalIdValue.Name = "lblNationalIdValue";
            this.lblNationalIdValue.Size = new System.Drawing.Size(14, 16);
            this.lblNationalIdValue.TabIndex = 9;
            this.lblNationalIdValue.Text = "0";
            // 
            // lblFullNameValue
            // 
            this.lblFullNameValue.AutoSize = true;
            this.lblFullNameValue.Location = new System.Drawing.Point(182, 146);
            this.lblFullNameValue.Name = "lblFullNameValue";
            this.lblFullNameValue.Size = new System.Drawing.Size(14, 16);
            this.lblFullNameValue.TabIndex = 8;
            this.lblFullNameValue.Text = "0";
            // 
            // lblCreatedDateValue
            // 
            this.lblCreatedDateValue.AutoSize = true;
            this.lblCreatedDateValue.Location = new System.Drawing.Point(182, 108);
            this.lblCreatedDateValue.Name = "lblCreatedDateValue";
            this.lblCreatedDateValue.Size = new System.Drawing.Size(14, 16);
            this.lblCreatedDateValue.TabIndex = 7;
            this.lblCreatedDateValue.Text = "0";
            // 
            // lblPersonIdValue
            // 
            this.lblPersonIdValue.AutoSize = true;
            this.lblPersonIdValue.Location = new System.Drawing.Point(182, 71);
            this.lblPersonIdValue.Name = "lblPersonIdValue";
            this.lblPersonIdValue.Size = new System.Drawing.Size(14, 16);
            this.lblPersonIdValue.TabIndex = 6;
            this.lblPersonIdValue.Text = "0";
            // 
            // lblDriverIdValue
            // 
            this.lblDriverIdValue.AutoSize = true;
            this.lblDriverIdValue.Location = new System.Drawing.Point(182, 35);
            this.lblDriverIdValue.Name = "lblDriverIdValue";
            this.lblDriverIdValue.Size = new System.Drawing.Size(14, 16);
            this.lblDriverIdValue.TabIndex = 5;
            this.lblDriverIdValue.Text = "0";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(34, 184);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(75, 16);
            this.label10.TabIndex = 4;
            this.label10.Text = "National ID :";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(34, 146);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(70, 16);
            this.label8.TabIndex = 3;
            this.label8.Text = "Full Name :";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(34, 108);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(91, 16);
            this.label6.TabIndex = 2;
            this.label6.Text = "Created Date :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(34, 71);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(68, 16);
            this.label4.TabIndex = 1;
            this.label4.Text = "Person ID :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(34, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 16);
            this.label2.TabIndex = 0;
            this.label2.Text = "Driver ID : ";
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(405, 322);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(117, 39);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // frmDriverDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(545, 376);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblTitle);
            this.Name = "frmDriverDetails";
            this.Text = "Driver Details";
            this.Load += new System.EventHandler(this.frmDriverDetails_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblNationalIdValue;
        private System.Windows.Forms.Label lblFullNameValue;
        private System.Windows.Forms.Label lblCreatedDateValue;
        private System.Windows.Forms.Label lblPersonIdValue;
        private System.Windows.Forms.Label lblDriverIdValue;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnClose;
    }
}
