namespace DVLD.Violations
{
    partial class frmManageViolations
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
            this.components = new System.ComponentModel.Container();
            this.lblTitle = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cbFilterBy = new System.Windows.Forms.ComboBox();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.dgvViolations = new System.Windows.Forms.DataGridView();
            this.cmsViolations = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showDetailsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lblRecords = new System.Windows.Forms.Label();
            this.lblRecordsValue = new System.Windows.Forms.Label();
            this.btnAddViolation = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvViolations)).BeginInit();
            this.cmsViolations.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Roboto Slab", 20.8F);
            this.lblTitle.Location = new System.Drawing.Point(278, 18);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(298, 48);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Manage Violations";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Roboto Slab", 10.8F);
            this.label2.Location = new System.Drawing.Point(18, 96);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 24);
            this.label2.TabIndex = 1;
            this.label2.Text = "Filter By :";
            // 
            // cbFilterBy
            // 
            this.cbFilterBy.FormattingEnabled = true;
            this.cbFilterBy.Location = new System.Drawing.Point(116, 96);
            this.cbFilterBy.Name = "cbFilterBy";
            this.cbFilterBy.Size = new System.Drawing.Size(173, 24);
            this.cbFilterBy.TabIndex = 2;
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(295, 96);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(216, 24);
            this.txtSearch.TabIndex = 3;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // dgvViolations
            // 
            this.dgvViolations.AllowUserToAddRows = false;
            this.dgvViolations.AllowUserToDeleteRows = false;
            this.dgvViolations.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvViolations.ContextMenuStrip = this.cmsViolations;
            this.dgvViolations.Location = new System.Drawing.Point(12, 136);
            this.dgvViolations.Name = "dgvViolations";
            this.dgvViolations.ReadOnly = true;
            this.dgvViolations.RowHeadersWidth = 51;
            this.dgvViolations.RowTemplate.Height = 26;
            this.dgvViolations.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvViolations.Size = new System.Drawing.Size(839, 372);
            this.dgvViolations.TabIndex = 4;
            // 
            // cmsViolations
            // 
            this.cmsViolations.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cmsViolations.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showDetailsToolStripMenuItem});
            this.cmsViolations.Name = "cmsViolations";
            this.cmsViolations.Size = new System.Drawing.Size(157, 28);
            // 
            // showDetailsToolStripMenuItem
            // 
            this.showDetailsToolStripMenuItem.Name = "showDetailsToolStripMenuItem";
            this.showDetailsToolStripMenuItem.Size = new System.Drawing.Size(156, 24);
            this.showDetailsToolStripMenuItem.Text = "Show Details";
            this.showDetailsToolStripMenuItem.Click += new System.EventHandler(this.showDetailsToolStripMenuItem_Click);
            // 
            // lblRecords
            // 
            this.lblRecords.AutoSize = true;
            this.lblRecords.Location = new System.Drawing.Point(19, 524);
            this.lblRecords.Name = "lblRecords";
            this.lblRecords.Size = new System.Drawing.Size(106, 16);
            this.lblRecords.TabIndex = 5;
            this.lblRecords.Text = "Record Count : ";
            // 
            // lblRecordsValue
            // 
            this.lblRecordsValue.AutoSize = true;
            this.lblRecordsValue.Location = new System.Drawing.Point(131, 524);
            this.lblRecordsValue.Name = "lblRecordsValue";
            this.lblRecordsValue.Size = new System.Drawing.Size(14, 16);
            this.lblRecordsValue.TabIndex = 6;
            this.lblRecordsValue.Text = "0";
            // 
            // btnAddViolation
            // 
            this.btnAddViolation.Font = new System.Drawing.Font("Roboto Slab", 12.8F);
            this.btnAddViolation.Location = new System.Drawing.Point(635, 86);
            this.btnAddViolation.Name = "btnAddViolation";
            this.btnAddViolation.Size = new System.Drawing.Size(216, 41);
            this.btnAddViolation.TabIndex = 7;
            this.btnAddViolation.Text = "Add New Violation";
            this.btnAddViolation.UseVisualStyleBackColor = true;
            this.btnAddViolation.Click += new System.EventHandler(this.btnAddViolation_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(648, 517);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(96, 31);
            this.btnRefresh.TabIndex = 8;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(755, 517);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(96, 31);
            this.btnClose.TabIndex = 9;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // frmManageViolations
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(863, 560);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnAddViolation);
            this.Controls.Add(this.lblRecordsValue);
            this.Controls.Add(this.lblRecords);
            this.Controls.Add(this.dgvViolations);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.cbFilterBy);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblTitle);
            this.Name = "frmManageViolations";
            this.Text = "Manage Violations";
            this.Load += new System.EventHandler(this.frmManageViolations_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvViolations)).EndInit();
            this.cmsViolations.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbFilterBy;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.DataGridView dgvViolations;
        private System.Windows.Forms.ContextMenuStrip cmsViolations;
        private System.Windows.Forms.ToolStripMenuItem showDetailsToolStripMenuItem;
        private System.Windows.Forms.Label lblRecords;
        private System.Windows.Forms.Label lblRecordsValue;
        private System.Windows.Forms.Button btnAddViolation;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnClose;
    }
}
