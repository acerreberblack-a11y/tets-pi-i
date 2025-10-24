namespace IPWhiteListManager.Forms
{
    partial class AddIPForm
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
            this.lblSystem = new System.Windows.Forms.Label();
            this.cmbSystem = new System.Windows.Forms.ComboBox();
            this.btnEditSystem = new System.Windows.Forms.Button();
            this.lblIPAddress = new System.Windows.Forms.Label();
            this.txtIPAddress = new System.Windows.Forms.TextBox();
            this.lblIPAddressHint = new System.Windows.Forms.Label();
            this.lblEnvironment = new System.Windows.Forms.Label();
            this.chkProduction = new System.Windows.Forms.CheckBox();
            this.chkTest = new System.Windows.Forms.CheckBox();
            this.chkCombined = new System.Windows.Forms.CheckBox();
            this.lblEnvironmentHint = new System.Windows.Forms.Label();
            this.grpSystemDetails = new System.Windows.Forms.GroupBox();
            this.txtSystemDetails = new System.Windows.Forms.TextBox();
            this.lblRequestNumber = new System.Windows.Forms.Label();
            this.txtRequestNumber = new System.Windows.Forms.TextBox();
            this.chkRegisterNamen = new System.Windows.Forms.CheckBox();
            this.lblNamenStatus = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.grpSystemDetails.SuspendLayout();
            this.SuspendLayout();
            //
            // lblSystem
            //
            this.lblSystem.AutoSize = true;
            this.lblSystem.Location = new System.Drawing.Point(20, 20);
            this.lblSystem.Name = "lblSystem";
            this.lblSystem.Size = new System.Drawing.Size(46, 13);
            this.lblSystem.TabIndex = 0;
            this.lblSystem.Text = "Система:";
            //
            // cmbSystem
            //
            this.cmbSystem.FormattingEnabled = true;
            this.cmbSystem.Location = new System.Drawing.Point(100, 17);
            this.cmbSystem.Name = "cmbSystem";
            this.cmbSystem.Size = new System.Drawing.Size(220, 21);
            this.cmbSystem.TabIndex = 1;
            //
            // btnEditSystem
            //
            this.btnEditSystem.Location = new System.Drawing.Point(330, 15);
            this.btnEditSystem.Name = "btnEditSystem";
            this.btnEditSystem.Size = new System.Drawing.Size(120, 25);
            this.btnEditSystem.TabIndex = 2;
            this.btnEditSystem.Text = "Инфо об ИС...";
            this.btnEditSystem.UseVisualStyleBackColor = true;
            //
            // lblIPAddress
            //
            this.lblIPAddress.AutoSize = true;
            this.lblIPAddress.Location = new System.Drawing.Point(20, 60);
            this.lblIPAddress.Name = "lblIPAddress";
            this.lblIPAddress.Size = new System.Drawing.Size(54, 13);
            this.lblIPAddress.TabIndex = 2;
            this.lblIPAddress.Text = "IP-адрес:";
            //
            // txtIPAddress
            //
            this.txtIPAddress.Location = new System.Drawing.Point(100, 57);
            this.txtIPAddress.Name = "txtIPAddress";
            this.txtIPAddress.Size = new System.Drawing.Size(200, 20);
            this.txtIPAddress.TabIndex = 3;
            //
            // lblIPAddressHint
            //
            this.lblIPAddressHint.AutoSize = true;
            this.lblIPAddressHint.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblIPAddressHint.Location = new System.Drawing.Point(100, 80);
            this.lblIPAddressHint.Name = "lblIPAddressHint";
            this.lblIPAddressHint.Size = new System.Drawing.Size(200, 13);
            this.lblIPAddressHint.TabIndex = 4;
            this.lblIPAddressHint.Text = "Например: 192.168.1.100 или 10.0.0.5";
            //
            // lblEnvironment
            //
            this.lblEnvironment.AutoSize = true;
            this.lblEnvironment.Location = new System.Drawing.Point(20, 110);
            this.lblEnvironment.Name = "lblEnvironment";
            this.lblEnvironment.Size = new System.Drawing.Size(57, 13);
            this.lblEnvironment.TabIndex = 4;
            this.lblEnvironment.Text = "Окружение:";
            //
            // chkProduction
            //
            this.chkProduction.AutoSize = true;
            this.chkProduction.Location = new System.Drawing.Point(100, 110);
            this.chkProduction.Name = "chkProduction";
            this.chkProduction.Size = new System.Drawing.Size(89, 17);
            this.chkProduction.TabIndex = 5;
            this.chkProduction.Text = "Продуктивный";
            this.chkProduction.UseVisualStyleBackColor = true;
            //
            // chkTest
            //
            this.chkTest.AutoSize = true;
            this.chkTest.Location = new System.Drawing.Point(210, 110);
            this.chkTest.Name = "chkTest";
            this.chkTest.Size = new System.Drawing.Size(71, 17);
            this.chkTest.TabIndex = 6;
            this.chkTest.Text = "Тестовый";
            this.chkTest.UseVisualStyleBackColor = true;
            //
            // chkCombined
            //
            this.chkCombined.AutoSize = true;
            this.chkCombined.Location = new System.Drawing.Point(100, 135);
            this.chkCombined.Name = "chkCombined";
            this.chkCombined.Size = new System.Drawing.Size(154, 17);
            this.chkCombined.TabIndex = 7;
            this.chkCombined.Text = "Объединить тест и прод";
            this.chkCombined.UseVisualStyleBackColor = true;
            //
            // lblEnvironmentHint
            //
            this.lblEnvironmentHint.AutoSize = true;
            this.lblEnvironmentHint.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblEnvironmentHint.Location = new System.Drawing.Point(100, 155);
            this.lblEnvironmentHint.Name = "lblEnvironmentHint";
            this.lblEnvironmentHint.Size = new System.Drawing.Size(241, 13);
            this.lblEnvironmentHint.TabIndex = 8;
            this.lblEnvironmentHint.Text = "Отметьте оба варианта, если IP используется везде";
            //
            // grpSystemDetails
            //
            this.grpSystemDetails.Controls.Add(this.txtSystemDetails);
            this.grpSystemDetails.Location = new System.Drawing.Point(20, 180);
            this.grpSystemDetails.Name = "grpSystemDetails";
            this.grpSystemDetails.Size = new System.Drawing.Size(430, 130);
            this.grpSystemDetails.TabIndex = 9;
            this.grpSystemDetails.TabStop = false;
            this.grpSystemDetails.Text = "Контакты и описание ИС";
            //
            // txtSystemDetails
            //
            this.txtSystemDetails.BackColor = System.Drawing.SystemColors.Control;
            this.txtSystemDetails.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtSystemDetails.Location = new System.Drawing.Point(15, 22);
            this.txtSystemDetails.Multiline = true;
            this.txtSystemDetails.Name = "txtSystemDetails";
            this.txtSystemDetails.ReadOnly = true;
            this.txtSystemDetails.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtSystemDetails.Size = new System.Drawing.Size(399, 95);
            this.txtSystemDetails.TabIndex = 0;
            this.txtSystemDetails.TabStop = false;
            this.txtSystemDetails.Text = "Выберите систему, чтобы увидеть подробности.";
            //
            // lblRequestNumber
            //
            this.lblRequestNumber.AutoSize = true;
            this.lblRequestNumber.Location = new System.Drawing.Point(20, 325);
            this.lblRequestNumber.Name = "lblRequestNumber";
            this.lblRequestNumber.Size = new System.Drawing.Size(104, 13);
            this.lblRequestNumber.TabIndex = 8;
            this.lblRequestNumber.Text = "№ заявки в namen:";
            //
            // txtRequestNumber
            //
            this.txtRequestNumber.Location = new System.Drawing.Point(140, 322);
            this.txtRequestNumber.Name = "txtRequestNumber";
            this.txtRequestNumber.Size = new System.Drawing.Size(200, 20);
            this.txtRequestNumber.TabIndex = 9;
            //
            // chkRegisterNamen
            //
            this.chkRegisterNamen.AutoSize = true;
            this.chkRegisterNamen.Checked = true;
            this.chkRegisterNamen.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRegisterNamen.Location = new System.Drawing.Point(20, 355);
            this.chkRegisterNamen.Name = "chkRegisterNamen";
            this.chkRegisterNamen.Size = new System.Drawing.Size(154, 17);
            this.chkRegisterNamen.TabIndex = 10;
            this.chkRegisterNamen.Text = "Зарегистрировать в namen";
            this.chkRegisterNamen.UseVisualStyleBackColor = true;
            //
            // lblNamenStatus
            //
            this.lblNamenStatus.AutoSize = true;
            this.lblNamenStatus.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblNamenStatus.Location = new System.Drawing.Point(180, 356);
            this.lblNamenStatus.Name = "lblNamenStatus";
            this.lblNamenStatus.Size = new System.Drawing.Size(170, 13);
            this.lblNamenStatus.TabIndex = 11;
            this.lblNamenStatus.Text = "Статус регистрации: новая запись";
            //
            // btnSave
            //
            this.btnSave.Location = new System.Drawing.Point(100, 395);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 30);
            this.btnSave.TabIndex = 12;
            this.btnSave.Text = "Сохранить";
            this.btnSave.UseVisualStyleBackColor = true;
            //
            // btnCancel
            //
            this.btnCancel.Location = new System.Drawing.Point(210, 395);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 30);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            //
            // AddIPForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(475, 450);
            this.Controls.Add(this.lblNamenStatus);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.chkRegisterNamen);
            this.Controls.Add(this.txtRequestNumber);
            this.Controls.Add(this.lblRequestNumber);
            this.Controls.Add(this.grpSystemDetails);
            this.Controls.Add(this.lblEnvironmentHint);
            this.Controls.Add(this.chkCombined);
            this.Controls.Add(this.chkTest);
            this.Controls.Add(this.chkProduction);
            this.Controls.Add(this.lblEnvironment);
            this.Controls.Add(this.lblIPAddressHint);
            this.Controls.Add(this.txtIPAddress);
            this.Controls.Add(this.lblIPAddress);
            this.Controls.Add(this.btnEditSystem);
            this.Controls.Add(this.cmbSystem);
            this.Controls.Add(this.lblSystem);
            this.Name = "AddIPForm";
            this.Text = "Добавление IP-адреса";
            this.grpSystemDetails.ResumeLayout(false);
            this.grpSystemDetails.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblSystem;
        private System.Windows.Forms.ComboBox cmbSystem;
        private System.Windows.Forms.Button btnEditSystem;
        private System.Windows.Forms.Label lblIPAddress;
        private System.Windows.Forms.TextBox txtIPAddress;
        private System.Windows.Forms.Label lblIPAddressHint;
        private System.Windows.Forms.Label lblEnvironment;
        private System.Windows.Forms.CheckBox chkProduction;
        private System.Windows.Forms.CheckBox chkTest;
        private System.Windows.Forms.CheckBox chkCombined;
        private System.Windows.Forms.Label lblEnvironmentHint;
        private System.Windows.Forms.GroupBox grpSystemDetails;
        private System.Windows.Forms.TextBox txtSystemDetails;
        private System.Windows.Forms.Label lblRequestNumber;
        private System.Windows.Forms.TextBox txtRequestNumber;
        private System.Windows.Forms.CheckBox chkRegisterNamen;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblNamenStatus;
        private System.Windows.Forms.ToolTip toolTip;
    }
}