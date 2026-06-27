namespace DVLD.People
{
    partial class frmAddEditPerson
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
            this.labele = new System.Windows.Forms.Label();
            this.lblPersonID = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.personCardControl1 = new DVLD.Shared.PersonCardControl();
            this.SuspendLayout();
            // 
            // lblMode
            // 
            this.lblMode.AutoSize = true;
            this.lblMode.Font = new System.Drawing.Font("Roboto Slab", 25.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMode.Location = new System.Drawing.Point(201, 9);
            this.lblMode.Name = "lblMode";
            this.lblMode.Size = new System.Drawing.Size(386, 59);
            this.lblMode.TabIndex = 1;
            this.lblMode.Text = "Add New Contact";
            // 
            // labele
            // 
            this.labele.AutoSize = true;
            this.labele.Font = new System.Drawing.Font("PMingLiU-ExtB", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labele.Location = new System.Drawing.Point(12, 97);
            this.labele.Name = "labele";
            this.labele.Size = new System.Drawing.Size(105, 23);
            this.labele.TabIndex = 2;
            this.labele.Text = "Person Id :";
            // 
            // lblPersonID
            // 
            this.lblPersonID.AutoSize = true;
            this.lblPersonID.Font = new System.Drawing.Font("PMingLiU-ExtB", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPersonID.Location = new System.Drawing.Point(123, 97);
            this.lblPersonID.Name = "lblPersonID";
            this.lblPersonID.Size = new System.Drawing.Size(50, 23);
            this.lblPersonID.TabIndex = 3;
            this.lblPersonID.Text = "........";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(313, 395);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(117, 43);
            this.btnSave.TabIndex = 34;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // personCardControl1
            // 
            this.personCardControl1.Location = new System.Drawing.Point(0, 136);
            this.personCardControl1.Name = "personCardControl1";
            this.personCardControl1.Size = new System.Drawing.Size(822, 317);
            this.personCardControl1.TabIndex = 4;
            // 
            // frmAddEditPerson
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.ClientSize = new System.Drawing.Size(824, 450);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.personCardControl1);
            this.Controls.Add(this.lblPersonID);
            this.Controls.Add(this.labele);
            this.Controls.Add(this.lblMode);
            this.Name = "frmAddEditPerson";
            this.Text = "Contact Management System";
            this.Load += new System.EventHandler(this.frmAddEditPerson_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.Label labele;
        private System.Windows.Forms.Label lblPersonID;
        private DVLD.Shared.PersonCardControl personCardControl1;
        private System.Windows.Forms.Button btnSave;
    }
}
