namespace IPWhiteListManager.Forms
{
    partial class AddSystemForm
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
            this.lblSystemName = new System.Windows.Forms.Label();
            this.txtSystemName = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.chkCombined = new System.Windows.Forms.CheckBox();
            this.grpOwner = new System.Windows.Forms.GroupBox();
            this.txtOwnerEmail = new System.Windows.Forms.TextBox();
            this.lblOwnerEmail = new System.Windows.Forms.Label();
            this.txtOwnerName = new System.Windows.Forms.TextBox();
            this.lblOwnerName = new System.Windows.Forms.Label();
            this.grpTechnical = new System.Windows.Forms.GroupBox();
            this.txtTechnicalEmail = new System.Windows.Forms.TextBox();
            this.lblTechnicalEmail = new System.Windows.Forms.Label();
            this.txtTechnicalName = new System.Windows.Forms.TextBox();
            this.lblTechnicalName = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.grpOwner.SuspendLayout();
            this.grpTechnical.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblSystemName
            // 
            this.lblSystemName.AutoSize = true;
            this.lblSystemName.Location = new System.Drawing.Point(20, 20);
            this.lblSystemName.Name = "lblSystemName";
            this.lblSystemName.Size = new System.Drawing.Size(83, 13);
            this.lblSystemName.TabIndex = 0;
            this.lblSystemName.Text = "Наименование";
            // 
            // txtSystemName
            // 
            this.txtSystemName.Location = new System.Drawing.Point(20, 36);
            this.txtSystemName.Name = "txtSystemName";
            this.txtSystemName.Size = new System.Drawing.Size(420, 20);
            this.txtSystemName.TabIndex = 1;
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(20, 69);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(57, 13);
            this.lblDescription.TabIndex = 2;
            this.lblDescription.Text = "Описание";
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(20, 85);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDescription.Size = new System.Drawing.Size(420, 60);
            this.txtDescription.TabIndex = 3;
            // 
            // chkCombined
            // 
            this.chkCombined.AutoSize = true;
            this.chkCombined.Location = new System.Drawing.Point(23, 151);
            this.chkCombined.Name = "chkCombined";
            this.chkCombined.Size = new System.Drawing.Size(205, 17);
            this.chkCombined.TabIndex = 4;
            this.chkCombined.Text = "Тест и прод объединены (Both)";
            this.chkCombined.UseVisualStyleBackColor = true;
            // 
            // grpOwner
            // 
            this.grpOwner.Controls.Add(this.txtOwnerEmail);
            this.grpOwner.Controls.Add(this.lblOwnerEmail);
            this.grpOwner.Controls.Add(this.txtOwnerName);
            this.grpOwner.Controls.Add(this.lblOwnerName);
            this.grpOwner.Location = new System.Drawing.Point(20, 180);
            this.grpOwner.Name = "grpOwner";
            this.grpOwner.Size = new System.Drawing.Size(420, 90);
            this.grpOwner.TabIndex = 5;
            this.grpOwner.TabStop = false;
            this.grpOwner.Text = "Владелец системы";
            // 
            // txtOwnerEmail
            // 
            this.txtOwnerEmail.Location = new System.Drawing.Point(90, 55);
            this.txtOwnerEmail.Name = "txtOwnerEmail";
            this.txtOwnerEmail.Size = new System.Drawing.Size(310, 20);
            this.txtOwnerEmail.TabIndex = 3;
            // 
            // lblOwnerEmail
            // 
            this.lblOwnerEmail.AutoSize = true;
            this.lblOwnerEmail.Location = new System.Drawing.Point(15, 58);
            this.lblOwnerEmail.Name = "lblOwnerEmail";
            this.lblOwnerEmail.Size = new System.Drawing.Size(38, 13);
            this.lblOwnerEmail.TabIndex = 2;
            this.lblOwnerEmail.Text = "Почта";
            // 
            // txtOwnerName
            // 
            this.txtOwnerName.Location = new System.Drawing.Point(90, 25);
            this.txtOwnerName.Name = "txtOwnerName";
            this.txtOwnerName.Size = new System.Drawing.Size(310, 20);
            this.txtOwnerName.TabIndex = 1;
            // 
            // lblOwnerName
            // 
            this.lblOwnerName.AutoSize = true;
            this.lblOwnerName.Location = new System.Drawing.Point(15, 28);
            this.lblOwnerName.Name = "lblOwnerName";
            this.lblOwnerName.Size = new System.Drawing.Size(29, 13);
            this.lblOwnerName.TabIndex = 0;
            this.lblOwnerName.Text = "ФИО";
            // 
            // grpTechnical
            // 
            this.grpTechnical.Controls.Add(this.txtTechnicalEmail);
            this.grpTechnical.Controls.Add(this.lblTechnicalEmail);
            this.grpTechnical.Controls.Add(this.txtTechnicalName);
            this.grpTechnical.Controls.Add(this.lblTechnicalName);
            this.grpTechnical.Location = new System.Drawing.Point(20, 280);
            this.grpTechnical.Name = "grpTechnical";
            this.grpTechnical.Size = new System.Drawing.Size(420, 90);
            this.grpTechnical.TabIndex = 6;
            this.grpTechnical.TabStop = false;
            this.grpTechnical.Text = "Технический специалист";
            // 
            // txtTechnicalEmail
            // 
            this.txtTechnicalEmail.Location = new System.Drawing.Point(90, 55);
            this.txtTechnicalEmail.Name = "txtTechnicalEmail";
            this.txtTechnicalEmail.Size = new System.Drawing.Size(310, 20);
            this.txtTechnicalEmail.TabIndex = 3;
            // 
            // lblTechnicalEmail
            // 
            this.lblTechnicalEmail.AutoSize = true;
            this.lblTechnicalEmail.Location = new System.Drawing.Point(15, 58);
            this.lblTechnicalEmail.Name = "lblTechnicalEmail";
            this.lblTechnicalEmail.Size = new System.Drawing.Size(38, 13);
            this.lblTechnicalEmail.TabIndex = 2;
            this.lblTechnicalEmail.Text = "Почта";
            // 
            // txtTechnicalName
            // 
            this.txtTechnicalName.Location = new System.Drawing.Point(90, 25);
            this.txtTechnicalName.Name = "txtTechnicalName";
            this.txtTechnicalName.Size = new System.Drawing.Size(310, 20);
            this.txtTechnicalName.TabIndex = 1;
            // 
            // lblTechnicalName
            // 
            this.lblTechnicalName.AutoSize = true;
            this.lblTechnicalName.Location = new System.Drawing.Point(15, 28);
            this.lblTechnicalName.Name = "lblTechnicalName";
            this.lblTechnicalName.Size = new System.Drawing.Size(29, 13);
            this.lblTechnicalName.TabIndex = 0;
            this.lblTechnicalName.Text = "ФИО";
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.SteelBlue;
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(254, 385);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(90, 27);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "Сохранить";
            this.btnSave.UseVisualStyleBackColor = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(350, 385);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 27);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // AddSystemForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 431);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.grpTechnical);
            this.Controls.Add(this.grpOwner);
            this.Controls.Add(this.chkCombined);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.txtSystemName);
            this.Controls.Add(this.lblSystemName);
            this.AcceptButton = this.btnSave;
            this.CancelButton = this.btnCancel;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddSystemForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Добавление ИС";
            this.grpOwner.ResumeLayout(false);
            this.grpOwner.PerformLayout();
            this.grpTechnical.ResumeLayout(false);
            this.grpTechnical.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblSystemName;
        private System.Windows.Forms.TextBox txtSystemName;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.CheckBox chkCombined;
        private System.Windows.Forms.GroupBox grpOwner;
        private System.Windows.Forms.TextBox txtOwnerEmail;
        private System.Windows.Forms.Label lblOwnerEmail;
        private System.Windows.Forms.TextBox txtOwnerName;
        private System.Windows.Forms.Label lblOwnerName;
        private System.Windows.Forms.GroupBox grpTechnical;
        private System.Windows.Forms.TextBox txtTechnicalEmail;
        private System.Windows.Forms.Label lblTechnicalEmail;
        private System.Windows.Forms.TextBox txtTechnicalName;
        private System.Windows.Forms.Label lblTechnicalName;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
    }
}
