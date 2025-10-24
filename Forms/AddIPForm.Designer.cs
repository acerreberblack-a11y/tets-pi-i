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
            this.lblSystem = new System.Windows.Forms.Label();
            this.cmbSystem = new System.Windows.Forms.ComboBox();
            this.lblIPAddress = new System.Windows.Forms.Label();
            this.txtIPAddress = new System.Windows.Forms.TextBox();
            this.lblEnvironment = new System.Windows.Forms.Label();
            this.chkProduction = new System.Windows.Forms.CheckBox();
            this.chkTest = new System.Windows.Forms.CheckBox();
            this.lblRequestNumber = new System.Windows.Forms.Label();
            this.txtRequestNumber = new System.Windows.Forms.TextBox();
            this.chkRegisterNamen = new System.Windows.Forms.CheckBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
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
            this.cmbSystem.Size = new System.Drawing.Size(300, 21);
            this.cmbSystem.TabIndex = 1;
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
            this.txtIPAddress.Text = "192.168.1.100";
            // 
            // lblEnvironment
            // 
            this.lblEnvironment.AutoSize = true;
            this.lblEnvironment.Location = new System.Drawing.Point(20, 100);
            this.lblEnvironment.Name = "lblEnvironment";
            this.lblEnvironment.Size = new System.Drawing.Size(57, 13);
            this.lblEnvironment.TabIndex = 4;
            this.lblEnvironment.Text = "Окружение:";
            // 
            // chkProduction
            // 
            this.chkProduction.AutoSize = true;
            this.chkProduction.Location = new System.Drawing.Point(100, 100);
            this.chkProduction.Name = "chkProduction";
            this.chkProduction.Size = new System.Drawing.Size(89, 17);
            this.chkProduction.TabIndex = 5;
            this.chkProduction.Text = "Продуктивный";
            this.chkProduction.UseVisualStyleBackColor = true;
            // 
            // chkTest
            // 
            this.chkTest.AutoSize = true;
            this.chkTest.Location = new System.Drawing.Point(210, 100);
            this.chkTest.Name = "chkTest";
            this.chkTest.Size = new System.Drawing.Size(71, 17);
            this.chkTest.TabIndex = 6;
            this.chkTest.Text = "Тестовый";
            this.chkTest.UseVisualStyleBackColor = true;
            // 
            // lblRequestNumber
            // 
            this.lblRequestNumber.AutoSize = true;
            this.lblRequestNumber.Location = new System.Drawing.Point(20, 140);
            this.lblRequestNumber.Name = "lblRequestNumber";
            this.lblRequestNumber.Size = new System.Drawing.Size(104, 13);
            this.lblRequestNumber.TabIndex = 7;
            this.lblRequestNumber.Text = "№ заявки в namen:";
            // 
            // txtRequestNumber
            // 
            this.txtRequestNumber.Location = new System.Drawing.Point(140, 137);
            this.txtRequestNumber.Name = "txtRequestNumber";
            this.txtRequestNumber.Size = new System.Drawing.Size(200, 20);
            this.txtRequestNumber.TabIndex = 8;
            this.txtRequestNumber.Text = "NAMEN-20240115-1234";
            // 
            // chkRegisterNamen
            // 
            this.chkRegisterNamen.AutoSize = true;
            this.chkRegisterNamen.Checked = true;
            this.chkRegisterNamen.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRegisterNamen.Location = new System.Drawing.Point(20, 180);
            this.chkRegisterNamen.Name = "chkRegisterNamen";
            this.chkRegisterNamen.Size = new System.Drawing.Size(154, 17);
            this.chkRegisterNamen.TabIndex = 9;
            this.chkRegisterNamen.Text = "Зарегистрировать в namen";
            this.chkRegisterNamen.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(100, 220);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 30);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "Сохранить";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(210, 220);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 30);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // AddIPForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(450, 350);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.chkRegisterNamen);
            this.Controls.Add(this.txtRequestNumber);
            this.Controls.Add(this.lblRequestNumber);
            this.Controls.Add(this.chkTest);
            this.Controls.Add(this.chkProduction);
            this.Controls.Add(this.lblEnvironment);
            this.Controls.Add(this.txtIPAddress);
            this.Controls.Add(this.lblIPAddress);
            this.Controls.Add(this.cmbSystem);
            this.Controls.Add(this.lblSystem);
            this.Name = "AddIPForm";
            this.Text = "Добавление IP-адреса";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblSystem;
        private System.Windows.Forms.ComboBox cmbSystem;
        private System.Windows.Forms.Label lblIPAddress;
        private System.Windows.Forms.TextBox txtIPAddress;
        private System.Windows.Forms.Label lblEnvironment;
        private System.Windows.Forms.CheckBox chkProduction;
        private System.Windows.Forms.CheckBox chkTest;
        private System.Windows.Forms.Label lblRequestNumber;
        private System.Windows.Forms.TextBox txtRequestNumber;
        private System.Windows.Forms.CheckBox chkRegisterNamen;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
    }
}